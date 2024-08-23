namespace Manga_Library_Manager
{
    partial class mangaDownloader
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mangaDownloader));
            tableLayoutPanel1 = new TableLayoutPanel();
            downloadButton = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            linkTextBox = new TextBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            titleSelectionDropDown = new ComboBox();
            searchButton = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            addToManagerCheckBox = new CheckBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            preferenceComboBox = new ComboBox();
            locationButton = new Button();
            tableLayoutPanel8 = new TableLayoutPanel();
            removeExtrasButton = new Button();
            formatDropDown = new ComboBox();
            qualityDropDown = new ComboBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            selectedChaptersList = new CheckedListBox();
            flowLayout = new FlowLayoutPanel();
            folderBrowserDialog = new FolderBrowserDialog();
            toolTip = new ToolTip(components);
            downloaderThread = new System.ComponentModel.BackgroundWorker();
            epubMakerThread = new System.ComponentModel.BackgroundWorker();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(downloadButton, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 82F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.Size = new Size(914, 576);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // downloadButton
            // 
            downloadButton.BackColor = Color.FromArgb(248, 200, 220);
            downloadButton.Dock = DockStyle.Fill;
            downloadButton.Enabled = false;
            downloadButton.FlatAppearance.BorderSize = 0;
            downloadButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            downloadButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            downloadButton.FlatStyle = FlatStyle.Flat;
            downloadButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            downloadButton.ForeColor = Color.Black;
            downloadButton.Location = new Point(3, 543);
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(908, 30);
            downloadButton.TabIndex = 7;
            downloadButton.Text = "Start Downloading";
            downloadButton.UseVisualStyleBackColor = false;
            downloadButton.Click += downloadButton_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel2.Controls.Add(linkTextBox, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel7, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(914, 34);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // linkTextBox
            // 
            linkTextBox.Dock = DockStyle.Fill;
            linkTextBox.Location = new Point(3, 3);
            linkTextBox.MaxLength = 800;
            linkTextBox.Name = "linkTextBox";
            linkTextBox.PlaceholderText = "MangaDex Link";
            linkTextBox.Size = new Size(313, 27);
            linkTextBox.TabIndex = 0;
            linkTextBox.WordWrap = false;
            linkTextBox.TextChanged += linkTextBox_TextChanged;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(titleSelectionDropDown, 1, 0);
            tableLayoutPanel7.Controls.Add(searchButton, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(319, 0);
            tableLayoutPanel7.Margin = new Padding(0);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Size = new Size(595, 34);
            tableLayoutPanel7.TabIndex = 1;
            // 
            // titleSelectionDropDown
            // 
            titleSelectionDropDown.Dock = DockStyle.Fill;
            titleSelectionDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            titleSelectionDropDown.Enabled = false;
            titleSelectionDropDown.FormattingEnabled = true;
            titleSelectionDropDown.Items.AddRange(new object[] { "Select which title to use:" });
            titleSelectionDropDown.Location = new Point(37, 3);
            titleSelectionDropDown.Name = "titleSelectionDropDown";
            titleSelectionDropDown.Size = new Size(555, 27);
            titleSelectionDropDown.TabIndex = 1;
            titleSelectionDropDown.SelectedIndexChanged += titleSelectionDropDown_SelectedIndexChanged;
            // 
            // searchButton
            // 
            searchButton.BackColor = Color.FromArgb(248, 200, 220);
            searchButton.BackgroundImage = (Image)resources.GetObject("searchButton.BackgroundImage");
            searchButton.BackgroundImageLayout = ImageLayout.Zoom;
            searchButton.Dock = DockStyle.Fill;
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            searchButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            searchButton.ForeColor = Color.Black;
            searchButton.Location = new Point(3, 3);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(28, 28);
            searchButton.TabIndex = 6;
            toolTip.SetToolTip(searchButton, "Defaults to wherever the executable is located");
            searchButton.UseVisualStyleBackColor = false;
            searchButton.Click += searchButton_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 2, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel6, 1, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel8, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 34);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(914, 34);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(addToManagerCheckBox, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(730, 0);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(184, 34);
            tableLayoutPanel4.TabIndex = 6;
            // 
            // addToManagerCheckBox
            // 
            addToManagerCheckBox.AutoSize = true;
            addToManagerCheckBox.Checked = true;
            addToManagerCheckBox.CheckState = CheckState.Checked;
            addToManagerCheckBox.Dock = DockStyle.Fill;
            addToManagerCheckBox.Enabled = false;
            addToManagerCheckBox.Font = new Font("Calibri", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addToManagerCheckBox.Location = new Point(7, 3);
            addToManagerCheckBox.Name = "addToManagerCheckBox";
            addToManagerCheckBox.Size = new Size(170, 28);
            addToManagerCheckBox.TabIndex = 6;
            addToManagerCheckBox.Text = "Add to Library Manager";
            addToManagerCheckBox.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(preferenceComboBox, 1, 0);
            tableLayoutPanel6.Controls.Add(locationButton, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(365, 0);
            tableLayoutPanel6.Margin = new Padding(0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Size = new Size(365, 34);
            tableLayoutPanel6.TabIndex = 7;
            // 
            // preferenceComboBox
            // 
            preferenceComboBox.Dock = DockStyle.Fill;
            preferenceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            preferenceComboBox.Enabled = false;
            preferenceComboBox.FormattingEnabled = true;
            preferenceComboBox.Items.AddRange(new object[] { "Most in manga", "Most up to chapter" });
            preferenceComboBox.Location = new Point(185, 3);
            preferenceComboBox.Name = "preferenceComboBox";
            preferenceComboBox.Size = new Size(177, 27);
            preferenceComboBox.TabIndex = 2;
            toolTip.SetToolTip(preferenceComboBox, "Choose whether to auto-select chapters based on\r\nwhich scanlator did most of the manga, or most up\r\nto each chapter.");
            preferenceComboBox.SelectedIndexChanged += preferenceComboBox_SelectedIndexChanged;
            // 
            // locationButton
            // 
            locationButton.BackColor = Color.FromArgb(248, 200, 220);
            locationButton.Dock = DockStyle.Fill;
            locationButton.Enabled = false;
            locationButton.FlatAppearance.BorderSize = 0;
            locationButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            locationButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            locationButton.FlatStyle = FlatStyle.Flat;
            locationButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            locationButton.ForeColor = Color.Black;
            locationButton.Location = new Point(3, 3);
            locationButton.Name = "locationButton";
            locationButton.Size = new Size(176, 28);
            locationButton.TabIndex = 5;
            locationButton.Text = "Set Download Location";
            toolTip.SetToolTip(locationButton, "Defaults to wherever the executable is located");
            locationButton.UseVisualStyleBackColor = false;
            locationButton.Click += locationButton_Click;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 3;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel8.Controls.Add(removeExtrasButton, 2, 0);
            tableLayoutPanel8.Controls.Add(formatDropDown, 1, 0);
            tableLayoutPanel8.Controls.Add(qualityDropDown, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(0, 0);
            tableLayoutPanel8.Margin = new Padding(0);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Size = new Size(365, 34);
            tableLayoutPanel8.TabIndex = 8;
            // 
            // removeExtrasButton
            // 
            removeExtrasButton.BackColor = Color.FromArgb(248, 200, 220);
            removeExtrasButton.Dock = DockStyle.Fill;
            removeExtrasButton.Enabled = false;
            removeExtrasButton.FlatAppearance.BorderSize = 0;
            removeExtrasButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            removeExtrasButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            removeExtrasButton.FlatStyle = FlatStyle.Flat;
            removeExtrasButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            removeExtrasButton.ForeColor = Color.Black;
            removeExtrasButton.Location = new Point(247, 3);
            removeExtrasButton.Name = "removeExtrasButton";
            removeExtrasButton.Size = new Size(115, 28);
            removeExtrasButton.TabIndex = 6;
            removeExtrasButton.Text = "Exclude Extras";
            toolTip.SetToolTip(removeExtrasButton, "Deselects all the chapters with decimals.");
            removeExtrasButton.UseVisualStyleBackColor = false;
            removeExtrasButton.Click += removeExtrasButton_Click;
            // 
            // formatDropDown
            // 
            formatDropDown.Dock = DockStyle.Fill;
            formatDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            formatDropDown.Enabled = false;
            formatDropDown.FormattingEnabled = true;
            formatDropDown.Items.AddRange(new object[] { "EPUB", "CBZ" });
            formatDropDown.Location = new Point(127, 3);
            formatDropDown.Name = "formatDropDown";
            formatDropDown.Size = new Size(114, 27);
            formatDropDown.TabIndex = 1;
            // 
            // qualityDropDown
            // 
            qualityDropDown.Dock = DockStyle.Fill;
            qualityDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            qualityDropDown.Enabled = false;
            qualityDropDown.FormattingEnabled = true;
            qualityDropDown.Items.AddRange(new object[] { "Original Quality", "Data-Saver" });
            qualityDropDown.Location = new Point(3, 3);
            qualityDropDown.Name = "qualityDropDown";
            qualityDropDown.Size = new Size(118, 27);
            qualityDropDown.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(selectedChaptersList, 0, 0);
            tableLayoutPanel5.Controls.Add(flowLayout, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 68);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(914, 472);
            tableLayoutPanel5.TabIndex = 2;
            // 
            // selectedChaptersList
            // 
            selectedChaptersList.BackColor = Color.DimGray;
            selectedChaptersList.CheckOnClick = true;
            selectedChaptersList.Dock = DockStyle.Fill;
            selectedChaptersList.Enabled = false;
            selectedChaptersList.ForeColor = Color.White;
            selectedChaptersList.FormattingEnabled = true;
            selectedChaptersList.Location = new Point(3, 3);
            selectedChaptersList.Name = "selectedChaptersList";
            selectedChaptersList.Size = new Size(451, 466);
            selectedChaptersList.TabIndex = 0;
            selectedChaptersList.SelectedIndexChanged += selectedChaptersList_SelectedIndexChanged;
            // 
            // flowLayout
            // 
            flowLayout.AutoScroll = true;
            flowLayout.Dock = DockStyle.Fill;
            flowLayout.Location = new Point(460, 3);
            flowLayout.Name = "flowLayout";
            flowLayout.Size = new Size(451, 466);
            flowLayout.TabIndex = 1;
            // 
            // folderBrowserDialog
            // 
            folderBrowserDialog.AddToRecent = false;
            folderBrowserDialog.Description = "Select the folder to be searched";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            // 
            // downloaderThread
            // 
            downloaderThread.WorkerReportsProgress = true;
            downloaderThread.WorkerSupportsCancellation = true;
            downloaderThread.DoWork += downloaderThread_DoWork;
            downloaderThread.ProgressChanged += downloaderThread_ProgressChanged;
            downloaderThread.RunWorkerCompleted += downloaderThread_RunWorkerCompleted;
            // 
            // epubMakerThread
            // 
            epubMakerThread.WorkerSupportsCancellation = true;
            epubMakerThread.DoWork += epubMakerThread_DoWork;
            epubMakerThread.RunWorkerCompleted += epubMakerThread_RunWorkerCompleted;
            // 
            // mangaDownloader
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(914, 576);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            Name = "mangaDownloader";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Download a Manga";
            FormClosing += mangaDownloader_FormClosing;
            Load += mangaDownloader_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox linkTextBox;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBox qualityDropDown;
        private Button locationButton;
        private FolderBrowserDialog folderBrowserDialog;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox addToManagerCheckBox;
        private ToolTip toolTip;
        private TableLayoutPanel tableLayoutPanel5;
        private CheckedListBox selectedChaptersList;
        private FlowLayoutPanel flowLayout;
        private Button downloadButton;
        private System.ComponentModel.BackgroundWorker downloaderThread;
        private System.ComponentModel.BackgroundWorker epubMakerThread;
        private ComboBox titleSelectionDropDown;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private Button searchButton;
        private Button removeExtrasButton;
        private TableLayoutPanel tableLayoutPanel8;
        private ComboBox formatDropDown;
        private ComboBox preferenceComboBox;
    }
}