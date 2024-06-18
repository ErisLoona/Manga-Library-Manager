namespace Manga_Library_Manager
{
    partial class userSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(userSettings));
            donateButton = new Button();
            creditsLabel = new Label();
            label1 = new Label();
            languageDropDown = new ComboBox();
            warnCheckBox = new CheckBox();
            label2 = new Label();
            importButton = new Button();
            toolTip = new ToolTip(components);
            checkUpdatesCheckbox = new CheckBox();
            SuspendLayout();
            // 
            // donateButton
            // 
            donateButton.BackColor = Color.MediumPurple;
            donateButton.FlatAppearance.BorderSize = 0;
            donateButton.FlatAppearance.MouseDownBackColor = Color.Indigo;
            donateButton.FlatAppearance.MouseOverBackColor = Color.DarkSlateBlue;
            donateButton.FlatStyle = FlatStyle.Flat;
            donateButton.Font = new Font("Calibri", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            donateButton.ForeColor = Color.Black;
            donateButton.Location = new Point(156, 175);
            donateButton.Margin = new Padding(7, 5, 7, 5);
            donateButton.Name = "donateButton";
            donateButton.Size = new Size(50, 22);
            donateButton.TabIndex = 11;
            donateButton.Text = "Donate";
            toolTip.SetToolTip(donateButton, "Thanks for considering! Will open:\r\nhttps://ko-fi.com/erisloona");
            donateButton.UseVisualStyleBackColor = false;
            donateButton.Click += donateButton_Click;
            // 
            // creditsLabel
            // 
            creditsLabel.AutoSize = true;
            creditsLabel.Font = new Font("Calibri", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            creditsLabel.ForeColor = Color.FromArgb(248, 200, 220);
            creditsLabel.Location = new Point(107, 198);
            creditsLabel.Name = "creditsLabel";
            creditsLabel.Size = new Size(103, 14);
            creditsLabel.TabIndex = 12;
            creditsLabel.Text = "made by Eris Loona";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(37, 9);
            label1.Name = "label1";
            label1.Size = new Size(136, 19);
            label1.TabIndex = 13;
            label1.Text = "Preferred Language";
            // 
            // languageDropDown
            // 
            languageDropDown.AutoCompleteMode = AutoCompleteMode.Suggest;
            languageDropDown.AutoCompleteSource = AutoCompleteSource.ListItems;
            languageDropDown.FormattingEnabled = true;
            languageDropDown.Location = new Point(21, 33);
            languageDropDown.Name = "languageDropDown";
            languageDropDown.Size = new Size(169, 27);
            languageDropDown.TabIndex = 14;
            languageDropDown.SelectedIndexChanged += languageDropDown_SelectedIndexChanged;
            // 
            // warnCheckBox
            // 
            warnCheckBox.AutoSize = true;
            warnCheckBox.Checked = true;
            warnCheckBox.CheckState = CheckState.Checked;
            warnCheckBox.Font = new Font("Calibri", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            warnCheckBox.Location = new Point(38, 69);
            warnCheckBox.Name = "warnCheckBox";
            warnCheckBox.Size = new Size(134, 34);
            warnCheckBox.TabIndex = 15;
            warnCheckBox.Text = "Show warning if the\r\nfile no longer exists";
            warnCheckBox.UseVisualStyleBackColor = true;
            warnCheckBox.CheckedChanged += warnCheckBox_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(-1, 187);
            label2.Name = "label2";
            label2.Size = new Size(92, 26);
            label2.TabIndex = 16;
            label2.Text = "Using the\r\nMangaDex.org API";
            // 
            // importButton
            // 
            importButton.BackColor = Color.FromArgb(248, 200, 220);
            importButton.FlatAppearance.BorderSize = 0;
            importButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            importButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            importButton.FlatStyle = FlatStyle.Flat;
            importButton.Font = new Font("Calibri", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            importButton.ForeColor = Color.Black;
            importButton.Location = new Point(61, 139);
            importButton.Margin = new Padding(7, 5, 7, 5);
            importButton.Name = "importButton";
            importButton.Size = new Size(89, 24);
            importButton.TabIndex = 17;
            importButton.Text = "Import Library";
            toolTip.SetToolTip(importButton, "Import a library JSON string (i.e. from a failed write)\r\nWill only work with this program's format!");
            importButton.UseVisualStyleBackColor = false;
            importButton.Click += importButton_Click;
            // 
            // checkUpdatesCheckbox
            // 
            checkUpdatesCheckbox.AutoSize = true;
            checkUpdatesCheckbox.Font = new Font("Calibri", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkUpdatesCheckbox.Location = new Point(43, 109);
            checkUpdatesCheckbox.Name = "checkUpdatesCheckbox";
            checkUpdatesCheckbox.Size = new Size(124, 19);
            checkUpdatesCheckbox.TabIndex = 18;
            checkUpdatesCheckbox.Text = "Check for updates";
            checkUpdatesCheckbox.UseVisualStyleBackColor = true;
            checkUpdatesCheckbox.CheckedChanged += checkUpdatesCheckbox_CheckedChanged;
            // 
            // userSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(210, 215);
            Controls.Add(checkUpdatesCheckbox);
            Controls.Add(importButton);
            Controls.Add(label2);
            Controls.Add(warnCheckBox);
            Controls.Add(languageDropDown);
            Controls.Add(label1);
            Controls.Add(creditsLabel);
            Controls.Add(donateButton);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "userSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            Load += userSettings_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button donateButton;
        private Label creditsLabel;
        private Label label1;
        private ComboBox languageDropDown;
        private CheckBox warnCheckBox;
        private Label label2;
        private Button importButton;
        private ToolTip toolTip;
        private CheckBox checkUpdatesCheckbox;
    }
}