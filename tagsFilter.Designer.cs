namespace Manga_Library_Manager
{
    partial class tagsFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tagsFilter));
            doneButton = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            uncheckAllButton = new Button();
            checkAllButton = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            ratingList = new CheckedListBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            includeList = new CheckedListBox();
            excludeList = new CheckedListBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            inclusionMode = new ComboBox();
            exclusionMode = new ComboBox();
            tagSearch = new TextBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            occurancesLabel = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // doneButton
            // 
            doneButton.BackColor = Color.FromArgb(248, 200, 220);
            doneButton.Dock = DockStyle.Bottom;
            doneButton.FlatAppearance.BorderSize = 0;
            doneButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            doneButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            doneButton.FlatStyle = FlatStyle.Flat;
            doneButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            doneButton.ForeColor = Color.Black;
            doneButton.Location = new Point(0, 540);
            doneButton.Margin = new Padding(0);
            doneButton.Name = "doneButton";
            doneButton.Size = new Size(693, 30);
            doneButton.TabIndex = 7;
            doneButton.Text = "Done";
            doneButton.UseVisualStyleBackColor = false;
            doneButton.Click += doneButton_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 4);
            tableLayoutPanel1.Controls.Add(tagSearch, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.Size = new Size(485, 540);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(uncheckAllButton, 0, 0);
            tableLayoutPanel2.Controls.Add(checkAllButton, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(485, 54);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // uncheckAllButton
            // 
            uncheckAllButton.BackColor = Color.FromArgb(248, 200, 220);
            uncheckAllButton.Dock = DockStyle.Fill;
            uncheckAllButton.FlatAppearance.BorderSize = 0;
            uncheckAllButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            uncheckAllButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            uncheckAllButton.FlatStyle = FlatStyle.Flat;
            uncheckAllButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uncheckAllButton.ForeColor = Color.Black;
            uncheckAllButton.Location = new Point(249, 5);
            uncheckAllButton.Margin = new Padding(7, 5, 7, 5);
            uncheckAllButton.Name = "uncheckAllButton";
            uncheckAllButton.Size = new Size(229, 44);
            uncheckAllButton.TabIndex = 3;
            uncheckAllButton.Text = "Exclude All";
            uncheckAllButton.UseVisualStyleBackColor = false;
            uncheckAllButton.Click += uncheckAllButton_Click;
            // 
            // checkAllButton
            // 
            checkAllButton.BackColor = Color.FromArgb(248, 200, 220);
            checkAllButton.Dock = DockStyle.Fill;
            checkAllButton.FlatAppearance.BorderSize = 0;
            checkAllButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            checkAllButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            checkAllButton.FlatStyle = FlatStyle.Flat;
            checkAllButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkAllButton.ForeColor = Color.Black;
            checkAllButton.Location = new Point(7, 5);
            checkAllButton.Margin = new Padding(7, 5, 7, 5);
            checkAllButton.Name = "checkAllButton";
            checkAllButton.Size = new Size(228, 44);
            checkAllButton.TabIndex = 2;
            checkAllButton.Text = "Include All";
            checkAllButton.UseVisualStyleBackColor = false;
            checkAllButton.Click += checkAllButton_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(ratingList, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 54);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(485, 108);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // ratingList
            // 
            ratingList.BackColor = Color.DimGray;
            ratingList.BorderStyle = BorderStyle.None;
            ratingList.CheckOnClick = true;
            ratingList.Dock = DockStyle.Fill;
            ratingList.ForeColor = Color.White;
            ratingList.FormattingEnabled = true;
            ratingList.Items.AddRange(new object[] { "Safe", "Suggestive", "Erotica", "Pornographic" });
            ratingList.Location = new Point(182, 3);
            ratingList.Name = "ratingList";
            ratingList.Size = new Size(120, 102);
            ratingList.TabIndex = 0;
            ratingList.SelectedIndexChanged += ratingList_SelectedIndexChanged;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(includeList, 0, 0);
            tableLayoutPanel4.Controls.Add(excludeList, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 194);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(485, 313);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // includeList
            // 
            includeList.BackColor = Color.DimGray;
            includeList.CheckOnClick = true;
            includeList.Dock = DockStyle.Fill;
            includeList.ForeColor = Color.White;
            includeList.FormattingEnabled = true;
            includeList.Location = new Point(3, 3);
            includeList.Name = "includeList";
            includeList.Size = new Size(236, 307);
            includeList.TabIndex = 0;
            includeList.ItemCheck += includeList_ItemCheck;
            includeList.MouseDown += includeList_MouseDown;
            // 
            // excludeList
            // 
            excludeList.BackColor = Color.DimGray;
            excludeList.CheckOnClick = true;
            excludeList.Dock = DockStyle.Fill;
            excludeList.ForeColor = Color.White;
            excludeList.FormattingEnabled = true;
            excludeList.Location = new Point(245, 3);
            excludeList.Name = "excludeList";
            excludeList.Size = new Size(237, 307);
            excludeList.TabIndex = 1;
            excludeList.ItemCheck += excludeList_ItemCheck;
            excludeList.MouseDown += excludeList_MouseDown;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(inclusionMode, 0, 0);
            tableLayoutPanel5.Controls.Add(exclusionMode, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 507);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(485, 33);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // inclusionMode
            // 
            inclusionMode.Dock = DockStyle.Fill;
            inclusionMode.DropDownStyle = ComboBoxStyle.DropDownList;
            inclusionMode.FormattingEnabled = true;
            inclusionMode.Items.AddRange(new object[] { "and", "or" });
            inclusionMode.Location = new Point(3, 3);
            inclusionMode.Name = "inclusionMode";
            inclusionMode.Size = new Size(236, 27);
            inclusionMode.TabIndex = 0;
            // 
            // exclusionMode
            // 
            exclusionMode.Dock = DockStyle.Fill;
            exclusionMode.DropDownStyle = ComboBoxStyle.DropDownList;
            exclusionMode.FormattingEnabled = true;
            exclusionMode.Items.AddRange(new object[] { "and", "or" });
            exclusionMode.Location = new Point(245, 3);
            exclusionMode.Name = "exclusionMode";
            exclusionMode.Size = new Size(237, 27);
            exclusionMode.TabIndex = 1;
            // 
            // tagSearch
            // 
            tagSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            tagSearch.Dock = DockStyle.Fill;
            tagSearch.Location = new Point(3, 165);
            tagSearch.Name = "tagSearch";
            tagSearch.PlaceholderText = "Search for a tag";
            tagSearch.Size = new Size(479, 27);
            tagSearch.TabIndex = 4;
            tagSearch.WordWrap = false;
            tagSearch.KeyDown += tagSearch_KeyDown;
            tagSearch.Leave += tagSearch_Leave;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel6.Controls.Add(tableLayoutPanel1, 0, 0);
            tableLayoutPanel6.Controls.Add(occurancesLabel, 1, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 0);
            tableLayoutPanel6.Margin = new Padding(0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Size = new Size(693, 540);
            tableLayoutPanel6.TabIndex = 9;
            // 
            // occurancesLabel
            // 
            occurancesLabel.BackColor = Color.DimGray;
            occurancesLabel.BorderStyle = BorderStyle.None;
            occurancesLabel.DetectUrls = false;
            occurancesLabel.Dock = DockStyle.Fill;
            occurancesLabel.ForeColor = Color.White;
            occurancesLabel.Location = new Point(488, 0);
            occurancesLabel.Margin = new Padding(3, 0, 0, 0);
            occurancesLabel.Name = "occurancesLabel";
            occurancesLabel.ReadOnly = true;
            occurancesLabel.ScrollBars = RichTextBoxScrollBars.Vertical;
            occurancesLabel.Size = new Size(205, 540);
            occurancesLabel.TabIndex = 9;
            occurancesLabel.Text = "text\ntext\ntext\ntext";
            occurancesLabel.Enter += occurancesLabel_Enter;
            // 
            // tagsFilter
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(693, 570);
            Controls.Add(tableLayoutPanel6);
            Controls.Add(doneButton);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            MinimizeBox = false;
            Name = "tagsFilter";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Filter by Tags";
            FormClosing += tagsFilter_FormClosing;
            Load += tagsFilter_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button doneButton;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private Button uncheckAllButton;
        private Button checkAllButton;
        private CheckedListBox ratingList;
        private TextBox tagSearch;
        private CheckedListBox includeList;
        private CheckedListBox excludeList;
        private ComboBox inclusionMode;
        private ComboBox exclusionMode;
        private TableLayoutPanel tableLayoutPanel6;
        private RichTextBox occurancesLabel;
    }
}