namespace GUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListViewItem listViewItem1 = new ListViewItem("https://msdl.microsoft.com/download/symbols");
            processListView = new ListView();
            PID = new ColumnHeader();
            ProcessName = new ColumnHeader();
            label1 = new Label();
            searchBox = new TextBox();
            label2 = new Label();
            refreshTimer = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            localCachePath = new TextBox();
            label4 = new Label();
            symbolServerListView = new ListView();
            TheOnlyColumn = new ColumnHeader();
            button1 = new Button();
            button2 = new Button();
            expandedSymbolSearchPath = new Label();
            label5 = new Label();
            numWorkers = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numWorkers).BeginInit();
            SuspendLayout();
            // 
            // processListView
            // 
            processListView.Columns.AddRange(new ColumnHeader[] { PID, ProcessName });
            processListView.LabelWrap = false;
            processListView.Location = new Point(12, 73);
            processListView.MultiSelect = false;
            processListView.Name = "processListView";
            processListView.Size = new Size(338, 365);
            processListView.TabIndex = 0;
            processListView.UseCompatibleStateImageBehavior = false;
            processListView.View = View.Details;
            processListView.MouseUp += processListView_MouseUp;
            // 
            // PID
            // 
            PID.Text = "PID";
            // 
            // ProcessName
            // 
            ProcessName.Text = "Process Name";
            // 
            // label1
            // 
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(776, 35);
            label1.TabIndex = 1;
            label1.Text = "If you are not seeing some prcesses it is probably because you don't have Administrator privilege. Click on process entry or press Enter in the search box to start symbol downloading.";
            // 
            // searchBox
            // 
            searchBox.Location = new Point(68, 44);
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(282, 23);
            searchBox.TabIndex = 0;
            searchBox.TextChanged += searchBox_TextChanged;
            searchBox.KeyPress += searchBox_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 47);
            label2.Name = "label2";
            label2.Size = new Size(50, 17);
            label2.TabIndex = 3;
            label2.Text = "Search:";
            // 
            // refreshTimer
            // 
            refreshTimer.Enabled = true;
            refreshTimer.Interval = 2000;
            refreshTimer.Tick += refreshTimer_Tick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(356, 73);
            label3.Name = "label3";
            label3.Size = new Size(154, 17);
            label3.TabIndex = 4;
            label3.Text = "Local symbol cache path:";
            // 
            // localCachePath
            // 
            localCachePath.Location = new Point(356, 93);
            localCachePath.Name = "localCachePath";
            localCachePath.Size = new Size(432, 23);
            localCachePath.TabIndex = 5;
            localCachePath.Text = "%TEMP%\\SymbolCache";
            localCachePath.TextChanged += localCachePath_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(356, 122);
            label4.Name = "label4";
            label4.Size = new Size(300, 17);
            label4.TabIndex = 6;
            label4.Text = "Symbol servers: (click slowly twice to edit in place)";
            // 
            // symbolServerListView
            // 
            symbolServerListView.CheckBoxes = true;
            symbolServerListView.Columns.AddRange(new ColumnHeader[] { TheOnlyColumn });
            symbolServerListView.HeaderStyle = ColumnHeaderStyle.None;
            listViewItem1.Checked = true;
            listViewItem1.StateImageIndex = 1;
            symbolServerListView.Items.AddRange(new ListViewItem[] { listViewItem1 });
            symbolServerListView.LabelEdit = true;
            symbolServerListView.LabelWrap = false;
            symbolServerListView.Location = new Point(356, 148);
            symbolServerListView.Name = "symbolServerListView";
            symbolServerListView.Size = new Size(432, 144);
            symbolServerListView.TabIndex = 7;
            symbolServerListView.UseCompatibleStateImageBehavior = false;
            symbolServerListView.View = View.Details;
            symbolServerListView.AfterLabelEdit += symbolServerListView_AfterLabelEdit;
            symbolServerListView.ItemChecked += symbolServerListView_ItemChecked;
            // 
            // TheOnlyColumn
            // 
            TheOnlyColumn.Text = "TheOnlyColumn";
            TheOnlyColumn.Width = 300;
            // 
            // button1
            // 
            button1.Location = new Point(654, 119);
            button1.Name = "button1";
            button1.Size = new Size(64, 23);
            button1.TabIndex = 8;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(724, 119);
            button2.Name = "button2";
            button2.Size = new Size(64, 23);
            button2.TabIndex = 9;
            button2.Text = "Remove";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // expandedSymbolSearchPath
            // 
            expandedSymbolSearchPath.Location = new Point(356, 295);
            expandedSymbolSearchPath.Name = "expandedSymbolSearchPath";
            expandedSymbolSearchPath.Size = new Size(432, 84);
            expandedSymbolSearchPath.TabIndex = 10;
            expandedSymbolSearchPath.Text = "Dbghelp symbol search path expands to:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(356, 47);
            label5.Name = "label5";
            label5.Size = new Size(258, 17);
            label5.TabIndex = 11;
            label5.Text = "Numer of symbols to download in parallel:";
            // 
            // numWorkers
            // 
            numWorkers.Location = new Point(620, 44);
            numWorkers.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            numWorkers.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWorkers.Name = "numWorkers";
            numWorkers.Size = new Size(47, 23);
            numWorkers.TabIndex = 12;
            numWorkers.Value = new decimal(new int[] { 16, 0, 0, 0 });
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(800, 450);
            Controls.Add(numWorkers);
            Controls.Add(label5);
            Controls.Add(expandedSymbolSearchPath);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(symbolServerListView);
            Controls.Add(label4);
            Controls.Add(localCachePath);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(searchBox);
            Controls.Add(label1);
            Controls.Add(processListView);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Parallel Symbol Downloader";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)numWorkers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView processListView;
        private ColumnHeader PID;
        private ColumnHeader ProcessName;
        private Label label1;
        private TextBox searchBox;
        private Label label2;
        private System.Windows.Forms.Timer refreshTimer;
        private Label label3;
        private TextBox localCachePath;
        private Label label4;
        private ListView symbolServerListView;
        private Button button1;
        private Button button2;
        private ColumnHeader TheOnlyColumn;
        private Label expandedSymbolSearchPath;
        private Label label5;
        private NumericUpDown numWorkers;
    }
}
