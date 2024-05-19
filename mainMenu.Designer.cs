namespace Manga_Library_Manager
{
    partial class mainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainMenu));
            layoutTable = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tagsButtons = new Button();
            filterButton = new Button();
            scanButton = new Button();
            addMangaButton = new Button();
            mangaList = new ListBox();
            searchTextBox = new TextBox();
            mangaDescLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            mangaDescCover = new PictureBox();
            mangaDescTitle = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            ongoingOnlineLabel = new Label();
            linkTextBox = new ComboBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            label1 = new Label();
            resetButton = new Button();
            tableLayoutPanel8 = new TableLayoutPanel();
            lastChapterNumber = new NumericUpDown();
            tableLayoutPanel7 = new TableLayoutPanel();
            label2 = new Label();
            ongoingCheckbox = new CheckBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            deleteEntryButton = new Button();
            checkOnlineButton = new Button();
            openInExplorerButton = new Button();
            lastChapterOnlineLabel = new Label();
            tableLayoutPanel9 = new TableLayoutPanel();
            editTagsButton = new Button();
            tagsTextBox = new TextBox();
            ratingLabel = new Label();
            openFileDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            checkOnlineTimer = new System.Windows.Forms.Timer(components);
            folderBrowserDialog = new FolderBrowserDialog();
            thread1 = new System.ComponentModel.BackgroundWorker();
            layoutTable.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            mangaDescLayoutPanel.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mangaDescCover).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lastChapterNumber).BeginInit();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            SuspendLayout();
            // 
            // layoutTable
            // 
            layoutTable.ColumnCount = 2;
            layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layoutTable.Controls.Add(tableLayoutPanel1, 0, 0);
            layoutTable.Controls.Add(mangaDescLayoutPanel, 1, 0);
            layoutTable.Dock = DockStyle.Fill;
            layoutTable.Location = new Point(0, 0);
            layoutTable.Margin = new Padding(0);
            layoutTable.Name = "layoutTable";
            layoutTable.RowCount = 1;
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layoutTable.Size = new Size(1124, 656);
            layoutTable.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(mangaList, 0, 2);
            tableLayoutPanel1.Controls.Add(searchTextBox, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 89F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(786, 656);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(tagsButtons, 3, 0);
            tableLayoutPanel2.Controls.Add(filterButton, 2, 0);
            tableLayoutPanel2.Controls.Add(scanButton, 1, 0);
            tableLayoutPanel2.Controls.Add(addMangaButton, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(786, 45);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tagsButtons
            // 
            tagsButtons.BackColor = Color.FromArgb(248, 200, 220);
            tagsButtons.Dock = DockStyle.Fill;
            tagsButtons.FlatAppearance.BorderSize = 0;
            tagsButtons.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            tagsButtons.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            tagsButtons.FlatStyle = FlatStyle.Flat;
            tagsButtons.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tagsButtons.ForeColor = Color.Black;
            tagsButtons.Location = new Point(595, 5);
            tagsButtons.Margin = new Padding(7, 5, 7, 5);
            tagsButtons.Name = "tagsButtons";
            tagsButtons.Size = new Size(184, 35);
            tagsButtons.TabIndex = 6;
            tagsButtons.Text = "Filter by Tags";
            tagsButtons.UseVisualStyleBackColor = false;
            tagsButtons.Click += tagsButtons_Click;
            // 
            // filterButton
            // 
            filterButton.BackColor = Color.FromArgb(248, 200, 220);
            filterButton.Dock = DockStyle.Fill;
            filterButton.FlatAppearance.BorderSize = 0;
            filterButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            filterButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            filterButton.FlatStyle = FlatStyle.Flat;
            filterButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterButton.ForeColor = Color.Black;
            filterButton.Location = new Point(399, 5);
            filterButton.Margin = new Padding(7, 5, 7, 5);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(182, 35);
            filterButton.TabIndex = 2;
            filterButton.Text = "Showing All";
            filterButton.UseVisualStyleBackColor = false;
            filterButton.Click += filterButton_Click;
            // 
            // scanButton
            // 
            scanButton.BackColor = Color.FromArgb(248, 200, 220);
            scanButton.Dock = DockStyle.Fill;
            scanButton.FlatAppearance.BorderSize = 0;
            scanButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            scanButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            scanButton.FlatStyle = FlatStyle.Flat;
            scanButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            scanButton.ForeColor = Color.Black;
            scanButton.Location = new Point(203, 5);
            scanButton.Margin = new Padding(7, 5, 7, 5);
            scanButton.Name = "scanButton";
            scanButton.Size = new Size(182, 35);
            scanButton.TabIndex = 1;
            scanButton.Text = "Scan Directory";
            scanButton.UseVisualStyleBackColor = false;
            scanButton.Click += scanButton_Click;
            // 
            // addMangaButton
            // 
            addMangaButton.BackColor = Color.FromArgb(248, 200, 220);
            addMangaButton.Dock = DockStyle.Fill;
            addMangaButton.FlatAppearance.BorderSize = 0;
            addMangaButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            addMangaButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            addMangaButton.FlatStyle = FlatStyle.Flat;
            addMangaButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addMangaButton.ForeColor = Color.Black;
            addMangaButton.Location = new Point(7, 5);
            addMangaButton.Margin = new Padding(7, 5, 7, 5);
            addMangaButton.Name = "addMangaButton";
            addMangaButton.Size = new Size(182, 35);
            addMangaButton.TabIndex = 0;
            addMangaButton.Text = "Add Manga";
            addMangaButton.UseVisualStyleBackColor = false;
            addMangaButton.Click += addMangaButton_Click;
            // 
            // mangaList
            // 
            mangaList.BackColor = Color.DimGray;
            mangaList.Dock = DockStyle.Fill;
            mangaList.Font = new Font("Calibri", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            mangaList.ForeColor = Color.White;
            mangaList.FormattingEnabled = true;
            mangaList.ItemHeight = 26;
            mangaList.Location = new Point(5, 79);
            mangaList.Margin = new Padding(5, 8, 5, 1);
            mangaList.Name = "mangaList";
            mangaList.Size = new Size(776, 576);
            mangaList.TabIndex = 1;
            mangaList.SelectedIndexChanged += mangaList_SelectedIndexChanged;
            mangaList.MouseDown += mangaList_MouseDown;
            // 
            // searchTextBox
            // 
            searchTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            searchTextBox.Dock = DockStyle.Fill;
            searchTextBox.Location = new Point(5, 50);
            searchTextBox.Margin = new Padding(5);
            searchTextBox.MaxLength = 500;
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search Manga...";
            searchTextBox.Size = new Size(776, 27);
            searchTextBox.TabIndex = 2;
            searchTextBox.WordWrap = false;
            searchTextBox.KeyDown += searchTextBox_KeyDown;
            searchTextBox.Leave += searchTextBox_Leave;
            // 
            // mangaDescLayoutPanel
            // 
            mangaDescLayoutPanel.ColumnCount = 1;
            mangaDescLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mangaDescLayoutPanel.Controls.Add(tableLayoutPanel4, 0, 0);
            mangaDescLayoutPanel.Controls.Add(tableLayoutPanel3, 0, 1);
            mangaDescLayoutPanel.Dock = DockStyle.Fill;
            mangaDescLayoutPanel.Location = new Point(786, 0);
            mangaDescLayoutPanel.Margin = new Padding(0);
            mangaDescLayoutPanel.Name = "mangaDescLayoutPanel";
            mangaDescLayoutPanel.RowCount = 2;
            mangaDescLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
            mangaDescLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 82F));
            mangaDescLayoutPanel.Size = new Size(338, 656);
            mangaDescLayoutPanel.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            tableLayoutPanel4.Controls.Add(mangaDescCover, 1, 0);
            tableLayoutPanel4.Controls.Add(mangaDescTitle, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(338, 118);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // mangaDescCover
            // 
            mangaDescCover.BackgroundImageLayout = ImageLayout.Zoom;
            mangaDescCover.Dock = DockStyle.Fill;
            mangaDescCover.Location = new Point(256, 0);
            mangaDescCover.Margin = new Padding(0);
            mangaDescCover.Name = "mangaDescCover";
            mangaDescCover.Size = new Size(82, 118);
            mangaDescCover.TabIndex = 0;
            mangaDescCover.TabStop = false;
            // 
            // mangaDescTitle
            // 
            mangaDescTitle.AutoSize = true;
            mangaDescTitle.Dock = DockStyle.Fill;
            mangaDescTitle.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mangaDescTitle.Location = new Point(0, 0);
            mangaDescTitle.Margin = new Padding(0, 0, 10, 0);
            mangaDescTitle.Name = "mangaDescTitle";
            mangaDescTitle.Size = new Size(246, 118);
            mangaDescTitle.TabIndex = 1;
            mangaDescTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(ongoingOnlineLabel, 0, 5);
            tableLayoutPanel3.Controls.Add(linkTextBox, 0, 2);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel6, 0, 1);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel7, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 0, 3);
            tableLayoutPanel3.Controls.Add(lastChapterOnlineLabel, 0, 4);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel9, 0, 6);
            tableLayoutPanel3.Controls.Add(tagsTextBox, 0, 8);
            tableLayoutPanel3.Controls.Add(ratingLabel, 0, 7);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 118);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 9;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));
            tableLayoutPanel3.Size = new Size(338, 538);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // ongoingOnlineLabel
            // 
            ongoingOnlineLabel.AutoSize = true;
            ongoingOnlineLabel.Dock = DockStyle.Fill;
            ongoingOnlineLabel.Location = new Point(0, 256);
            ongoingOnlineLabel.Margin = new Padding(0);
            ongoingOnlineLabel.Name = "ongoingOnlineLabel";
            ongoingOnlineLabel.Size = new Size(338, 32);
            ongoingOnlineLabel.TabIndex = 5;
            ongoingOnlineLabel.TextAlign = ContentAlignment.MiddleCenter;
            ongoingOnlineLabel.Visible = false;
            // 
            // linkTextBox
            // 
            linkTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            linkTextBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            linkTextBox.Dock = DockStyle.Fill;
            linkTextBox.Location = new Point(5, 131);
            linkTextBox.Margin = new Padding(5, 3, 5, 5);
            linkTextBox.MaxLength = 800;
            linkTextBox.Name = "linkTextBox";
            linkTextBox.Size = new Size(328, 27);
            linkTextBox.TabIndex = 3;
            linkTextBox.TextChanged += linkTextBox_TextChanged;
            linkTextBox.MouseDown += linkTextBox_MouseDoubleClick;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel6.Controls.Add(label1, 0, 0);
            tableLayoutPanel6.Controls.Add(resetButton, 2, 0);
            tableLayoutPanel6.Controls.Add(tableLayoutPanel8, 1, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 64);
            tableLayoutPanel6.Margin = new Padding(0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(338, 64);
            tableLayoutPanel6.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Calibri", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(4, 4);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(194, 56);
            label1.TabIndex = 0;
            label1.Text = "Last chapter:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // resetButton
            // 
            resetButton.BackColor = Color.FromArgb(248, 200, 220);
            resetButton.BackgroundImage = (Image)resources.GetObject("resetButton.BackgroundImage");
            resetButton.BackgroundImageLayout = ImageLayout.Zoom;
            resetButton.Dock = DockStyle.Fill;
            resetButton.FlatAppearance.BorderSize = 0;
            resetButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            resetButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            resetButton.FlatStyle = FlatStyle.Flat;
            resetButton.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            resetButton.ForeColor = Color.Black;
            resetButton.Location = new Point(303, 0);
            resetButton.Margin = new Padding(0);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(35, 64);
            resetButton.TabIndex = 3;
            toolTip.SetToolTip(resetButton, "Reset last chapter from file");
            resetButton.UseVisualStyleBackColor = false;
            resetButton.Click += resetButton_Click;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Controls.Add(lastChapterNumber, 0, 1);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(202, 0);
            tableLayoutPanel8.Margin = new Padding(0);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 3;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle());
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Size = new Size(101, 64);
            tableLayoutPanel8.TabIndex = 6;
            // 
            // lastChapterNumber
            // 
            lastChapterNumber.BackColor = Color.DimGray;
            lastChapterNumber.DecimalPlaces = 1;
            lastChapterNumber.Dock = DockStyle.Fill;
            lastChapterNumber.Font = new Font("Calibri", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lastChapterNumber.ForeColor = Color.White;
            lastChapterNumber.Location = new Point(2, 16);
            lastChapterNumber.Margin = new Padding(2, 0, 2, 0);
            lastChapterNumber.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            lastChapterNumber.Name = "lastChapterNumber";
            lastChapterNumber.Size = new Size(97, 32);
            lastChapterNumber.TabIndex = 1;
            lastChapterNumber.TextAlign = HorizontalAlignment.Center;
            lastChapterNumber.ValueChanged += lastChapterNumber_ValueChanged;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Controls.Add(label2, 0, 0);
            tableLayoutPanel7.Controls.Add(ongoingCheckbox, 0, 1);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(0, 0);
            tableLayoutPanel7.Margin = new Padding(0);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Size = new Size(338, 64);
            tableLayoutPanel7.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(4, 4);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(330, 24);
            label2.TabIndex = 0;
            label2.Text = "Ongoing?";
            label2.TextAlign = ContentAlignment.BottomCenter;
            // 
            // ongoingCheckbox
            // 
            ongoingCheckbox.AutoSize = true;
            ongoingCheckbox.CheckAlign = ContentAlignment.TopCenter;
            ongoingCheckbox.Dock = DockStyle.Fill;
            ongoingCheckbox.Font = new Font("Calibri", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ongoingCheckbox.Location = new Point(3, 35);
            ongoingCheckbox.Name = "ongoingCheckbox";
            ongoingCheckbox.Size = new Size(332, 26);
            ongoingCheckbox.TabIndex = 1;
            ongoingCheckbox.UseVisualStyleBackColor = true;
            ongoingCheckbox.CheckedChanged += ongoingCheckbox_CheckedChanged;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.Controls.Add(deleteEntryButton, 0, 0);
            tableLayoutPanel5.Controls.Add(checkOnlineButton, 0, 0);
            tableLayoutPanel5.Controls.Add(openInExplorerButton, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 160);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(338, 64);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // deleteEntryButton
            // 
            deleteEntryButton.BackColor = Color.FromArgb(248, 200, 220);
            deleteEntryButton.Dock = DockStyle.Fill;
            deleteEntryButton.FlatAppearance.BorderSize = 0;
            deleteEntryButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            deleteEntryButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            deleteEntryButton.FlatStyle = FlatStyle.Flat;
            deleteEntryButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            deleteEntryButton.ForeColor = Color.Black;
            deleteEntryButton.Location = new Point(231, 5);
            deleteEntryButton.Margin = new Padding(7, 5, 7, 5);
            deleteEntryButton.Name = "deleteEntryButton";
            deleteEntryButton.Size = new Size(100, 54);
            deleteEntryButton.TabIndex = 4;
            deleteEntryButton.Text = "Delete\r\nEntry";
            deleteEntryButton.UseVisualStyleBackColor = false;
            deleteEntryButton.Click += deleteEntryButton_Click;
            // 
            // checkOnlineButton
            // 
            checkOnlineButton.BackColor = Color.FromArgb(248, 200, 220);
            checkOnlineButton.Dock = DockStyle.Fill;
            checkOnlineButton.FlatAppearance.BorderSize = 0;
            checkOnlineButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            checkOnlineButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            checkOnlineButton.FlatStyle = FlatStyle.Flat;
            checkOnlineButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkOnlineButton.ForeColor = Color.Black;
            checkOnlineButton.Location = new Point(119, 5);
            checkOnlineButton.Margin = new Padding(7, 5, 7, 5);
            checkOnlineButton.Name = "checkOnlineButton";
            checkOnlineButton.Size = new Size(98, 54);
            checkOnlineButton.TabIndex = 3;
            checkOnlineButton.Text = "Check\r\nOnline";
            checkOnlineButton.UseVisualStyleBackColor = false;
            checkOnlineButton.Click += checkOnlineButton_Click;
            // 
            // openInExplorerButton
            // 
            openInExplorerButton.BackColor = Color.FromArgb(248, 200, 220);
            openInExplorerButton.Dock = DockStyle.Fill;
            openInExplorerButton.FlatAppearance.BorderSize = 0;
            openInExplorerButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            openInExplorerButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            openInExplorerButton.FlatStyle = FlatStyle.Flat;
            openInExplorerButton.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            openInExplorerButton.ForeColor = Color.Black;
            openInExplorerButton.Location = new Point(7, 5);
            openInExplorerButton.Margin = new Padding(7, 5, 7, 5);
            openInExplorerButton.Name = "openInExplorerButton";
            openInExplorerButton.Size = new Size(98, 54);
            openInExplorerButton.TabIndex = 2;
            openInExplorerButton.Text = "Open in\r\nFile Explorer";
            openInExplorerButton.UseVisualStyleBackColor = false;
            openInExplorerButton.Click += openInExplorerButton_Click;
            // 
            // lastChapterOnlineLabel
            // 
            lastChapterOnlineLabel.AutoSize = true;
            lastChapterOnlineLabel.Dock = DockStyle.Fill;
            lastChapterOnlineLabel.Location = new Point(0, 224);
            lastChapterOnlineLabel.Margin = new Padding(0);
            lastChapterOnlineLabel.Name = "lastChapterOnlineLabel";
            lastChapterOnlineLabel.Size = new Size(338, 32);
            lastChapterOnlineLabel.TabIndex = 4;
            lastChapterOnlineLabel.Text = "Last chapter online: ...";
            lastChapterOnlineLabel.TextAlign = ContentAlignment.MiddleCenter;
            lastChapterOnlineLabel.Visible = false;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 3;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Controls.Add(editTagsButton, 1, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(0, 288);
            tableLayoutPanel9.Margin = new Padding(0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel9.Size = new Size(338, 43);
            tableLayoutPanel9.TabIndex = 6;
            // 
            // editTagsButton
            // 
            editTagsButton.BackColor = Color.FromArgb(248, 200, 220);
            editTagsButton.Dock = DockStyle.Fill;
            editTagsButton.FlatAppearance.BorderSize = 0;
            editTagsButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            editTagsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            editTagsButton.FlatStyle = FlatStyle.Flat;
            editTagsButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            editTagsButton.ForeColor = Color.Black;
            editTagsButton.Location = new Point(120, 5);
            editTagsButton.Margin = new Padding(7, 5, 7, 5);
            editTagsButton.Name = "editTagsButton";
            editTagsButton.Size = new Size(98, 33);
            editTagsButton.TabIndex = 4;
            editTagsButton.Text = "Edit Tags";
            editTagsButton.UseVisualStyleBackColor = false;
            editTagsButton.Click += editTagsButton_Click;
            // 
            // tagsTextBox
            // 
            tagsTextBox.BackColor = Color.DimGray;
            tagsTextBox.BorderStyle = BorderStyle.None;
            tagsTextBox.Dock = DockStyle.Fill;
            tagsTextBox.ForeColor = Color.White;
            tagsTextBox.Location = new Point(0, 363);
            tagsTextBox.Margin = new Padding(0);
            tagsTextBox.Multiline = true;
            tagsTextBox.Name = "tagsTextBox";
            tagsTextBox.ReadOnly = true;
            tagsTextBox.ScrollBars = ScrollBars.Vertical;
            tagsTextBox.Size = new Size(338, 175);
            tagsTextBox.TabIndex = 7;
            // 
            // ratingLabel
            // 
            ratingLabel.AutoSize = true;
            ratingLabel.Dock = DockStyle.Fill;
            ratingLabel.Location = new Point(3, 331);
            ratingLabel.Name = "ratingLabel";
            ratingLabel.Size = new Size(332, 32);
            ratingLabel.TabIndex = 8;
            ratingLabel.Text = "Rating: Unknown";
            ratingLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "Select Manga(s)";
            openFileDialog.Filter = "Mangas (*.epub)|*.epub";
            openFileDialog.Multiselect = true;
            // 
            // checkOnlineTimer
            // 
            checkOnlineTimer.Interval = 3000;
            checkOnlineTimer.Tick += checkOnlineTimer_Tick;
            // 
            // folderBrowserDialog
            // 
            folderBrowserDialog.AddToRecent = false;
            folderBrowserDialog.Description = "Select the folder to be searched";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            // 
            // thread1
            // 
            thread1.DoWork += thread1_DoWork;
            thread1.RunWorkerCompleted += thread1_RunWorkerCompleted;
            // 
            // mainMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1124, 656);
            Controls.Add(layoutTable);
            DoubleBuffered = true;
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            Name = "mainMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manga Library Reader";
            FormClosing += mainMenu_FormClosing;
            Load += mainMenu_Load;
            layoutTable.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            mangaDescLayoutPanel.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mangaDescCover).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)lastChapterNumber).EndInit();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layoutTable;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel mangaDescLayoutPanel;
        private TableLayoutPanel tableLayoutPanel4;
        private PictureBox mangaDescCover;
        private Label mangaDescTitle;
        private TableLayoutPanel tableLayoutPanel3;
        private ListBox mangaList;
        private Button addMangaButton;
        private Button filterButton;
        private Button scanButton;
        private TextBox searchTextBox;
        private TableLayoutPanel tableLayoutPanel5;
        private Button deleteEntryButton;
        private Button checkOnlineButton;
        private Button openInExplorerButton;
        private TableLayoutPanel tableLayoutPanel7;
        private Label label2;
        private CheckBox ongoingCheckbox;
        private OpenFileDialog openFileDialog;
        private ComboBox linkTextBox;
        private ToolTip toolTip;
        private Label ongoingOnlineLabel;
        private Label lastChapterOnlineLabel;
        private System.Windows.Forms.Timer checkOnlineTimer;
        private FolderBrowserDialog folderBrowserDialog;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label1;
        private Button resetButton;
        private NumericUpDown lastChapterNumber;
        private TableLayoutPanel tableLayoutPanel8;
        private System.ComponentModel.BackgroundWorker thread1;
        private Button tagsButtons;
        private TableLayoutPanel tableLayoutPanel9;
        private Button editTagsButton;
        private TextBox tagsTextBox;
        private Label ratingLabel;
    }
}
