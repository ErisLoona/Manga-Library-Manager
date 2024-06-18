using System.Globalization;
using System.Diagnostics;

namespace Manga_Library_Manager
{
    public partial class userSettings : Form
    {
        private Dictionary<string, string> languages = new Dictionary<string, string>();

        public userSettings()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void userSettings_Load(object sender, EventArgs e)
        {
            warnCheckBox.Checked = !mainMenu.noWarning;
            checkUpdatesCheckbox.Checked = mainMenu.checkUpdates;
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                if (languages.ContainsValue(culture.TwoLetterISOLanguageName) == false)
                    languages[culture.EnglishName] = culture.TwoLetterISOLanguageName;
            languages["Simplified Chinese"] = "zh";
            languages["Traditional Chinese"] = "zh-hk";
            languages["Brazilian Portugese"] = "pt-br";
            languages["Castilian Spanish"] = "es";
            languages["Latin American Spanish"] = "es-la";
            languages["Romanized Japanese"] = "ja-ro";
            languages["Romanized Korean"] = "ko-ro";
            languages["Romanized Chinese"] = "zh-ro";
            languageDropDown.Items.AddRange(languages.Keys.ToArray());
            languageDropDown.SelectedItem = languages.FirstOrDefault(lang => lang.Value == mainMenu.selectedLanguage).Key;
        }

        private void languageDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainMenu.selectedLanguage = languages[languageDropDown.SelectedItem.ToString()];
        }

        private void warnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainMenu.noWarning = !warnCheckBox.Checked;
        }

        private void checkUpdatesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            mainMenu.checkUpdates = checkUpdatesCheckbox.Checked;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Warning! This will overwrite your entire current library!\nAre you sure you want to continue?", "Import library", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            importLibrary import = new importLibrary();
            DialogResult importResult = import.ShowDialog();
            import.Dispose();
            if (importResult == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                this.DialogResult = DialogResult.None;
        }

        private void donateButton_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://ko-fi.com/erisloona") { UseShellExecute = true });
        }
    }
}
