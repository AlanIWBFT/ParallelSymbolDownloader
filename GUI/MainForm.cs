using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GUI
{
    public partial class MainForm : Form
    {
        const uint PROCESS_QUERY_INFORMATION = 0x0400;
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);
        private bool TestIfProcessIsAccessible(Process process)
        {
            return OpenProcess(PROCESS_QUERY_INFORMATION, false, (uint)process.Id) != 0;
        }
        private List<Process> RefreshProcessList()
        {
            int CompareProcess(Process a, Process b)
            {
                int similarityA = MemoryExtensions.CommonPrefixLength(searchBox.Text.ToLower().AsSpan(), a.ProcessName.ToLower().AsSpan());
                int similarityB = MemoryExtensions.CommonPrefixLength(searchBox.Text.ToLower().AsSpan(), b.ProcessName.ToLower().AsSpan());
                if (similarityA != similarityB)
                {
                    return similarityB - similarityA;
                }
                else
                {
                    return a.Id - b.Id;
                }
            }

            Process[] processes = Process.GetProcesses();
            Array.Sort(processes, CompareProcess);
            return processes.Where(TestIfProcessIsAccessible).ToList();
        }

        private void PopulateProcessListView(List<Process> processes, bool retainTopItem)
        {
            int topItemIndex = processListView.TopItem?.Index ?? 0;

            processListView.BeginUpdate();
            processListView.Items.Clear();

            ListViewItem listViewItem;
            foreach (Process process in processes)
            {
                listViewItem = new ListViewItem();
                listViewItem.Text = process.Id.ToString();
                listViewItem.SubItems.Add(process.ProcessName);
                processListView.Items.Add(listViewItem);
            }

            processListView.EndUpdate();

            try
            {
                if (retainTopItem)
                {
                    processListView.TopItem = processListView.Items[topItemIndex];
                }
            }
            catch (Exception) { }

            processListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            processListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        public MainForm()
        {
            InitializeComponent();

            PopulateProcessListView(RefreshProcessList(), true);
        }
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            PopulateProcessListView(RefreshProcessList(), false);
        }

        private void searchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                string pid = processListView.Items[0].Text;
                Process process = Process.GetProcesses().Where(x => x.Id.ToString() == pid).First();
                refreshTimer.Stop();
                using (DownloadProgressForm form = new DownloadProgressForm(process, (int)numWorkers.Value, RefreshSymbolSearchPathParameter()))
                {
                    form.ShowDialog();
                }
                refreshTimer.Start();
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            PopulateProcessListView(RefreshProcessList(), true);
        }

        private void processListView_MouseUp(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo listViewHitTestInfo = processListView.HitTest(e.X, e.Y);
            ListViewItem item = listViewHitTestInfo.Item;
            if (item != null)
            {
                string pid = item.Text;
                Process process = Process.GetProcesses().Where(x => x.Id.ToString() == pid).First();
                refreshTimer.Stop();
                using (DownloadProgressForm form = new DownloadProgressForm(process, (int)numWorkers.Value, RefreshSymbolSearchPathParameter()))
                {
                    form.ShowDialog();
                }
                refreshTimer.Start();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            symbolServerListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            RefreshSymbolSearchPathParameter();
        }

        private string RefreshSymbolSearchPathParameter()
        {
            string symbolSearchPath = "";
            symbolSearchPath += "cache*" + localCachePath.Text;
            foreach (ListViewItem item in symbolServerListView.CheckedItems)
            {
                symbolSearchPath += ";srv*" + item.Text;
            }
            symbolServerListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            expandedSymbolSearchPath.Text = "Dbghelp symbol search path expands to: " + symbolSearchPath;

            return symbolSearchPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in symbolServerListView.SelectedItems)
            {
                symbolServerListView.Items.Remove(item);
            }
            RefreshSymbolSearchPathParameter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem newSymbolServerItem = new ListViewItem();
            symbolServerListView.Items.Add(newSymbolServerItem);
            newSymbolServerItem.Checked = true;
            newSymbolServerItem.BeginEdit();
        }

        private void symbolServerListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == "")
            {
                e.CancelEdit = true;
                symbolServerListView.Items.RemoveAt(e.Item);
            }
            else
            {
                // Hack: the update of text is after this event and RefreshSymbolSearchPathParameter gets empty string
                symbolServerListView.Items[e.Item].Text = e.Label;
            }

            RefreshSymbolSearchPathParameter();
        }

        private void localCachePath_TextChanged(object sender, EventArgs e)
        {
            RefreshSymbolSearchPathParameter();
        }

        private void symbolServerListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            RefreshSymbolSearchPathParameter();
        }
    }
}
