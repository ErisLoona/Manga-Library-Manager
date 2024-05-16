namespace Manga_Library_Manager
{
    partial class dumpJSON
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dumpJSON));
            textBox = new TextBox();
            copyButton = new Button();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.BackColor = Color.DimGray;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Dock = DockStyle.Fill;
            textBox.ForeColor = Color.White;
            textBox.Location = new Point(0, 0);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.ReadOnly = true;
            textBox.Size = new Size(914, 82);
            textBox.TabIndex = 0;
            // 
            // copyButton
            // 
            copyButton.BackColor = Color.FromArgb(248, 200, 220);
            copyButton.Dock = DockStyle.Bottom;
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(188, 152, 167);
            copyButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 229, 240);
            copyButton.FlatStyle = FlatStyle.Flat;
            copyButton.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            copyButton.ForeColor = Color.Black;
            copyButton.Location = new Point(0, 49);
            copyButton.Margin = new Padding(7, 5, 7, 5);
            copyButton.Name = "copyButton";
            copyButton.Size = new Size(914, 33);
            copyButton.TabIndex = 1;
            copyButton.Text = "Copy to Clipboard";
            copyButton.UseVisualStyleBackColor = false;
            copyButton.Click += copyButton_Click;
            // 
            // dumpJSON
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(914, 82);
            Controls.Add(copyButton);
            Controls.Add(textBox);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "dumpJSON";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "JSON Dump";
            Load += dumpJSON_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox;
        private Button copyButton;
    }
}