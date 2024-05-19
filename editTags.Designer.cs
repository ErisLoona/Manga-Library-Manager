namespace Manga_Library_Manager
{
    partial class editTags
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(editTags));
            tableLayoutPanel1 = new TableLayoutPanel();
            doneButton = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            ratingsDropdown = new ComboBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            newTagButton = new Button();
            tagTextBox = new TextBox();
            resetButton = new Button();
            tagsList = new CheckedListBox();
            toolTip = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(doneButton, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Controls.Add(tagsList, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 83F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            tableLayoutPanel1.Size = new Size(364, 539);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // doneButton
            // 
            doneButton.BackColor = Color.FromArgb(248, 200, 220);
            doneButton.BackgroundImageLayout = ImageLayout.Zoom;
            doneButton.Dock = DockStyle.Fill;
            doneButton.FlatAppearance.BorderSize = 0;
            doneButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            doneButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            doneButton.FlatStyle = FlatStyle.Flat;
            doneButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            doneButton.ForeColor = Color.Black;
            doneButton.Location = new Point(0, 511);
            doneButton.Margin = new Padding(0);
            doneButton.Name = "doneButton";
            doneButton.Size = new Size(364, 28);
            doneButton.TabIndex = 5;
            doneButton.Text = "Done";
            doneButton.UseVisualStyleBackColor = false;
            doneButton.Click += doneButton_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(ratingsDropdown, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(364, 32);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // ratingsDropdown
            // 
            ratingsDropdown.Dock = DockStyle.Fill;
            ratingsDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            ratingsDropdown.FormattingEnabled = true;
            ratingsDropdown.Items.AddRange(new object[] { "Safe", "Suggestive", "Erotica", "Pornographic" });
            ratingsDropdown.Location = new Point(86, 3);
            ratingsDropdown.MinimumSize = new Size(191, 0);
            ratingsDropdown.Name = "ratingsDropdown";
            ratingsDropdown.Size = new Size(191, 27);
            ratingsDropdown.TabIndex = 0;
            ratingsDropdown.SelectedIndexChanged += ratingsDropdown_SelectedIndexChanged;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(newTagButton, 2, 0);
            tableLayoutPanel3.Controls.Add(tagTextBox, 1, 0);
            tableLayoutPanel3.Controls.Add(resetButton, 3, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 32);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(364, 32);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // newTagButton
            // 
            newTagButton.BackColor = Color.FromArgb(248, 200, 220);
            newTagButton.BackgroundImage = (Image)resources.GetObject("newTagButton.BackgroundImage");
            newTagButton.BackgroundImageLayout = ImageLayout.Zoom;
            newTagButton.Dock = DockStyle.Fill;
            newTagButton.FlatAppearance.BorderSize = 0;
            newTagButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            newTagButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            newTagButton.FlatStyle = FlatStyle.Flat;
            newTagButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newTagButton.ForeColor = Color.Black;
            newTagButton.Location = new Point(274, 4);
            newTagButton.Margin = new Padding(4);
            newTagButton.Name = "newTagButton";
            newTagButton.Size = new Size(24, 24);
            newTagButton.TabIndex = 4;
            newTagButton.UseVisualStyleBackColor = false;
            newTagButton.Click += newTagButton_Click;
            // 
            // tagTextBox
            // 
            tagTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            tagTextBox.Dock = DockStyle.Fill;
            tagTextBox.Location = new Point(65, 3);
            tagTextBox.MaxLength = 100;
            tagTextBox.Name = "tagTextBox";
            tagTextBox.PlaceholderText = "Enter tag name";
            tagTextBox.Size = new Size(202, 27);
            tagTextBox.TabIndex = 0;
            tagTextBox.WordWrap = false;
            tagTextBox.KeyDown += tagTextBox_KeyDown;
            tagTextBox.Leave += tagTextBox_Leave;
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
            resetButton.Location = new Point(306, 4);
            resetButton.Margin = new Padding(4);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(54, 24);
            resetButton.TabIndex = 5;
            toolTip.SetToolTip(resetButton, "Reset tags to online original.");
            resetButton.UseVisualStyleBackColor = false;
            resetButton.Click += resetButton_Click;
            // 
            // tagsList
            // 
            tagsList.BackColor = Color.DimGray;
            tagsList.CheckOnClick = true;
            tagsList.Dock = DockStyle.Fill;
            tagsList.ForeColor = Color.White;
            tagsList.FormattingEnabled = true;
            tagsList.Location = new Point(3, 67);
            tagsList.Name = "tagsList";
            tagsList.Size = new Size(358, 441);
            tagsList.TabIndex = 2;
            tagsList.ItemCheck += tagsList_ItemCheck;
            tagsList.MouseDown += tagsList_MouseDown;
            // 
            // editTags
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(364, 539);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "editTags";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Tags";
            FormClosing += editTags_FormClosing;
            Load += editTags_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private ComboBox ratingsDropdown;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox tagTextBox;
        private Button newTagButton;
        private CheckedListBox tagsList;
        private Button doneButton;
        private Button resetButton;
        private ToolTip toolTip;
    }
}