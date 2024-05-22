
namespace Manga_Library_Manager
{
    public partial class editTags : Form
    {
        private AutoCompleteStringCollection tagSearchAutocompleteStrings = new AutoCompleteStringCollection();

        public editTags()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void editTags_Load(object sender, EventArgs e)
        {
            if (mainMenu.currentlySelectedBook.CotentRating != String.Empty)
                ratingsDropdown.SelectedItem = mainMenu.currentlySelectedBook.CotentRating;
            else
                ratingsDropdown.SelectedIndex = 0;
            tagsList.BeginUpdate();
            tagsList.ItemCheck -= tagsList_ItemCheck;
            foreach (string tag in mainMenu.uniqueTags)
                tagsList.Items.Add(tag, mainMenu.currentlySelectedBook.Tags.Contains(tag));
            tagsList.ItemCheck += tagsList_ItemCheck;
            tagsList.EndUpdate();
            tagSearchAutocompleteStrings.AddRange(mainMenu.uniqueTags.ToArray());
            tagTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tagTextBox.AutoCompleteCustomSource = tagSearchAutocompleteStrings;
        }

        private void tagsList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                mainMenu.currentlySelectedBook.Tags.Add(tagsList.SelectedItem.ToString());
                mainMenu.tagsUsage[tagsList.SelectedItem.ToString()]++;
            }
            else
            {
                mainMenu.currentlySelectedBook.Tags.Remove(tagsList.SelectedItem.ToString());
                mainMenu.tagsUsage[tagsList.SelectedItem.ToString()]--;
            }
        }

        private void tagTextBox_Leave(object sender, EventArgs e)
        {
            if (tagTextBox.Text == String.Empty)
                return;
            bool found = false;
            foreach (string tag in tagsList.Items)
                if (tagTextBox.Text.ToLower() == tag.ToLower())
                {
                    tagsList.SelectedItem = tag;
                    found = true;
                    tagTextBox.Text = String.Empty;
                    break;
                }
            if (found == false)
                tagsList.SelectedIndex = -1;
        }

        private void tagTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = null;
        }

        private void newTagButton_Click(object sender, EventArgs e)
        {
            if (tagTextBox.Text == String.Empty)
                return;
            bool found = false;
            foreach (string tag in tagsList.Items)
                if (tagTextBox.Text.ToLower() == tag.ToLower())
                {
                    found = true;
                    tagTextBox.Text = String.Empty;
                    break;
                }
            if (found == true)
                return;
            string newTag = tagTextBox.Text.Substring(0, 1).ToUpper() + tagTextBox.Text.Substring(1);
            tagsList.ItemCheck -= tagsList_ItemCheck;
            tagsList.Items.Add(newTag, true);
            tagsList.ItemCheck += tagsList_ItemCheck;
            tagSearchAutocompleteStrings.Add(newTag);
            mainMenu.uniqueTags.Add(newTag);
            mainMenu.tagsUsage[newTag] = 1;
            mainMenu.currentlySelectedBook.Tags.Add(newTag);
            tagTextBox.Text = String.Empty;
            tagsList.SelectedIndex = tagsList.Items.Count - 1;
        }

        private void ratingsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainMenu.currentlySelectedBook.CotentRating = ratingsDropdown.SelectedItem.ToString();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTags_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<string> tempTags = mainMenu.uniqueTags.ToList();
            foreach (string tag in tempTags)
                if (mainMenu.tagsUsage[tag] == 0)
                {
                    mainMenu.uniqueTags.Remove(tag);
                    mainMenu.tagsUsage.Remove(tag);
                }
        }

        private void tagsList_MouseDown(object sender, MouseEventArgs e)
        {
            tagsList.SelectedIndex = tagsList.IndexFromPoint(e.Location);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will resync the tags from the API.\nAre you sure?", "Resync tags", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            mainMenu.resetTags = true;
            this.Close();
        }
    }
}
