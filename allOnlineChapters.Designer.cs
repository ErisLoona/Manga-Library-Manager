namespace Manga_Library_Manager
{
    partial class allOnlineChapters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(allOnlineChapters));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label2 = new Label();
            label1 = new Label();
            doneButton = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            statusList = new ListBox();
            nameList = new ListBox();
            loadingBar = new ProgressBar();
            thread = new System.ComponentModel.BackgroundWorker();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Controls.Add(doneButton, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 2);
            tableLayoutPanel1.Controls.Add(loadingBar, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 82F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(443, 570);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel3.Controls.Add(label2, 1, 0);
            tableLayoutPanel3.Controls.Add(label1, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(443, 34);
            tableLayoutPanel3.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Calibri", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(265, 0);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(178, 34);
            label2.TabIndex = 1;
            label2.Text = "Status";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Calibri", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(265, 34);
            label1.TabIndex = 0;
            label1.Text = "Manga name";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // doneButton
            // 
            doneButton.BackColor = Color.FromArgb(248, 200, 220);
            doneButton.Dock = DockStyle.Fill;
            doneButton.FlatAppearance.BorderSize = 0;
            doneButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            doneButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            doneButton.FlatStyle = FlatStyle.Flat;
            doneButton.Font = new Font("Calibri", 18F, FontStyle.Bold);
            doneButton.ForeColor = Color.Black;
            doneButton.Location = new Point(7, 528);
            doneButton.Margin = new Padding(7, 5, 7, 5);
            doneButton.Name = "doneButton";
            doneButton.Size = new Size(429, 37);
            doneButton.TabIndex = 11;
            doneButton.Text = "Cancel";
            doneButton.UseVisualStyleBackColor = false;
            doneButton.Click += doneButton_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel2.Controls.Add(statusList, 1, 0);
            tableLayoutPanel2.Controls.Add(nameList, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 56);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(443, 467);
            tableLayoutPanel2.TabIndex = 12;
            // 
            // statusList
            // 
            statusList.BackColor = Color.DimGray;
            statusList.BorderStyle = BorderStyle.None;
            statusList.Dock = DockStyle.Fill;
            statusList.ForeColor = Color.White;
            statusList.FormattingEnabled = true;
            statusList.ItemHeight = 19;
            statusList.Location = new Point(265, 0);
            statusList.Margin = new Padding(0);
            statusList.Name = "statusList";
            statusList.Size = new Size(178, 467);
            statusList.TabIndex = 1;
            statusList.SelectedIndexChanged += statusList_SelectedIndexChanged;
            statusList.MouseDown += displayList_MouseDown;
            // 
            // nameList
            // 
            nameList.BackColor = Color.DimGray;
            nameList.BorderStyle = BorderStyle.None;
            nameList.Dock = DockStyle.Fill;
            nameList.ForeColor = Color.White;
            nameList.FormattingEnabled = true;
            nameList.ItemHeight = 19;
            nameList.Location = new Point(0, 0);
            nameList.Margin = new Padding(0);
            nameList.Name = "nameList";
            nameList.Size = new Size(265, 467);
            nameList.TabIndex = 0;
            nameList.SelectedIndexChanged += nameList_SelectedIndexChanged;
            nameList.MouseDown += displayList_MouseDown;
            // 
            // loadingBar
            // 
            loadingBar.Dock = DockStyle.Fill;
            loadingBar.Location = new Point(3, 36);
            loadingBar.Margin = new Padding(3, 2, 3, 3);
            loadingBar.Name = "loadingBar";
            loadingBar.Size = new Size(437, 17);
            loadingBar.Step = 1;
            loadingBar.TabIndex = 14;
            // 
            // thread
            // 
            thread.WorkerReportsProgress = true;
            thread.WorkerSupportsCancellation = true;
            thread.DoWork += thread_DoWork;
            thread.ProgressChanged += thread_ProgressChanged;
            thread.RunWorkerCompleted += thread_RunWorkerCompleted;
            // 
            // allOnlineChapters
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(443, 570);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "allOnlineChapters";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Get online chapters";
            FormClosing += allOnlineChapters_FormClosing;
            Load += allOnlineChapters_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button doneButton;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label2;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel2;
        private ListBox statusList;
        private ListBox nameList;
        private ProgressBar loadingBar;
        private System.ComponentModel.BackgroundWorker thread;
    }
}