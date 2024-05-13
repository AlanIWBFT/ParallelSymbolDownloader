using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class DownloadProgressForm : Form
    {
        Process TargetProcess;
        HashSet<string> ModuleNamesToDownload;
        int NumPDBsDownloaded = 0;
        Process[] WorkerProcesses;
        int NumWorkers = 16;
        string SymbolSearchPath = "cache*%TEMP%\\SymbolCache;srv*https://msdl.microsoft.com/download/symbols";

        private void ConsoleOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string strMessage = e.Data;
                if (strMessage.IndexOf("> ") != -1)
                {
                    string modulePath = strMessage.Substring(2);
                    if (ModuleNamesToDownload.Contains(modulePath))
                    {
                        NumPDBsDownloaded++;

                        int reportingWorkerIndex = -1;
                        for (int workerIndex = 0; workerIndex < NumWorkers; workerIndex++)
                        {
                            if (WorkerProcesses[workerIndex] == sender)
                            {
                                reportingWorkerIndex = workerIndex;
                                break;
                            }
                        }

                        Invoke(delegate
                        {
                            textBox1.AppendText("worker #" + reportingWorkerIndex + strMessage + "\r\n");
                            progressBar1.Value = NumPDBsDownloaded;
                            Text = string.Format("Downloading symbols... {0:N1}%", NumPDBsDownloaded * 100.0f / ModuleNamesToDownload.Count);
                            if (ModuleNamesToDownload.Count == NumPDBsDownloaded)
                            {
                                Text = "Finished.";
                                cancelButton.Text = "Finish";
                                textBox1.AppendText("Finished.");
                            }
                        });
                    }
                    else
                    {
                        Invoke(delegate
                        {
                            textBox1.AppendText("    Module not in download list \r\n");
                        });
                    }

                }
            }
        }
        public DownloadProgressForm(Process process, int numWorkers, string symbolSearchPath)
        {
            InitializeComponent();
            TargetProcess = process;
            NumWorkers = numWorkers;
            SymbolSearchPath = symbolSearchPath;
        }
        private bool TestIfProcessIsAccessible(Process process)
        {
            bool bProcessAccessible = false;
            try
            {
                process.SafeHandle.ToString();
                bProcessAccessible = true;
            }
            catch (Exception) { }
            return bProcessAccessible;
        }
        private HashSet<string> CollectModuleNamesToDownload(Process process)
        {
            HashSet<string> moduleNamesToDownload = new HashSet<string>();

            Process p = new Process();
            p.StartInfo.FileName = "symbol-download-worker.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = "1" + " " + process.Id + " " + 0 + " " + 1 + " " + SymbolSearchPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();

            string output;
            while ((output = p.StandardOutput.ReadLine()) != null)
            {
                string modulePath = output.Substring(2);
                string potentialPdbPath = Path.ChangeExtension(modulePath, ".pdb");
                if (!System.IO.File.Exists(potentialPdbPath))
                {
                    moduleNamesToDownload.Add(modulePath);
                }
            }

            p.WaitForExit();

            return moduleNamesToDownload;
        }
        private void LaunchDownloadWorkersForProcess(Process process)
        {
            if (!TestIfProcessIsAccessible(process)) return;

            WorkerProcesses = new Process[NumWorkers];

            for (int workerIndex = 0; workerIndex < NumWorkers; workerIndex++)
            {
                WorkerProcesses[workerIndex] = new Process();
                Process p = WorkerProcesses[workerIndex];
                p.StartInfo.FileName = "symbol-download-worker.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.Arguments = "0" + " " + process.Id + " " + workerIndex + " " + NumWorkers + " " + SymbolSearchPath;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.OutputDataReceived += ConsoleOutputReceived;
                p.ErrorDataReceived += ConsoleOutputReceived;
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DownloadProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int workerIndex = 0; workerIndex < NumWorkers; workerIndex++)
            {
                Process p = WorkerProcesses[workerIndex];
                p.Kill();
            }
        }

        private void DownloadProgressForm_Load(object sender, EventArgs e)
        {
            ModuleNamesToDownload = CollectModuleNamesToDownload(TargetProcess);
            progressBar1.Maximum = ModuleNamesToDownload.Count;
            LaunchDownloadWorkersForProcess(TargetProcess);
        }
    }
}
