using System.Data;

namespace Manga_Library_Manager
{
    public partial class tagsFilter : Form
    {
        private Dictionary<string, int> tagIndex = new Dictionary<string, int>();

        public tagsFilter()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void tagsFilter_Load(object sender, EventArgs e)
        {
            AutoCompleteStringCollection tagSearchAutocompleteStrings = new AutoCompleteStringCollection();
            ratingList.BeginUpdate();
            for (int i = 0; i < 4; i++)
                if (mainMenu.includedRatings.Contains(ratingList.Items[i].ToString()))
                    ratingList.SetItemChecked(i, true);
            ratingList.EndUpdate();
            tagSearchAutocompleteStrings.AddRange(mainMenu.uniqueTags.ToArray());
            tagSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tagSearch.AutoCompleteCustomSource = tagSearchAutocompleteStrings;
            if (mainMenu.inclusionMode == true)
                inclusionMode.SelectedIndex = 0;
            else
                inclusionMode.SelectedIndex = 1;
            if (mainMenu.exclusionMode == true)
                exclusionMode.SelectedIndex = 0;
            else
                exclusionMode.SelectedIndex = 1;
            includeList.BeginUpdate();
            excludeList.BeginUpdate();
            includeList.ItemCheck -= includeList_ItemCheck;
            excludeList.ItemCheck -= excludeList_ItemCheck;
            includeList.Items.AddRange(mainMenu.uniqueTags.ToArray());
            excludeList.Items.AddRange(mainMenu.uniqueTags.ToArray());
            int tagIndexCounter = 0;
            foreach (string tag in mainMenu.uniqueTags)
            {
                tagIndex[tag] = tagIndexCounter;
                tagIndexCounter++;
                if (mainMenu.includedTags.Contains(tag))
                {
                    includeList.SetItemChecked(includeList.Items.IndexOf(tag), true);
                    excludeList.Items.Remove(tag);
                }
                if (mainMenu.excludedTags.Contains(tag))
                {
                    excludeList.SetItemChecked(excludeList.Items.IndexOf(tag), true);
                    includeList.Items.Remove(tag);
                }
            }
            includeList.ItemCheck += includeList_ItemCheck;
            excludeList.ItemCheck += excludeList_ItemCheck;
            includeList.EndUpdate();
            excludeList.EndUpdate();
            occurancesLabel.Text = "Mangas: " + Convert.ToString(mainMenu.booksCopy.Count) + "\r\n\r\n";
            int[] ratingCount = { 0, 0, 0, 0 };
            foreach (mainMenu.eBook book in mainMenu.booksCopy)
                if (book.CotentRating == "Safe")
                    ratingCount[0]++;
                else if (book.CotentRating == "Suggestive")
                    ratingCount[1]++;
                else if (book.CotentRating == "Erotica")
                    ratingCount[2]++;
                else if (book.CotentRating == "Pornographic")
                    ratingCount[3]++;
            occurancesLabel.Text += "Safe: " + Convert.ToString(ratingCount[0]) + "\r\n";
            occurancesLabel.Text += "Suggestive: " + Convert.ToString(ratingCount[1]) + "\r\n";
            occurancesLabel.Text += "Erotica: " + Convert.ToString(ratingCount[2]) + "\r\n";
            occurancesLabel.Text += "Pornographic: " + Convert.ToString(ratingCount[3]) + "\r\n\r\n";
            occurancesLabel.Text += "No. of mangas by label:\r\n";
            foreach (KeyValuePair<string, Int32> usage in mainMenu.tagsUsage.OrderByDescending(key => key.Value))
                occurancesLabel.Text += usage.Key + ": " + Convert.ToString(usage.Value) + "\r\n";
            occurancesLabel.Select(0, 7);
            occurancesLabel.SelectionAlignment = HorizontalAlignment.Center;
            occurancesLabel.SelectionFont = new Font(occurancesLabel.Font, FontStyle.Bold);
            occurancesLabel.DeselectAll();
        }

        private void checkAllButton_Click(object sender, EventArgs e)
        {
            ratingList.BeginUpdate();
            for (int i = 0; i < ratingList.Items.Count; i++)
                ratingList.SetItemChecked(i, true);
            ratingList.EndUpdate();
            includeList.BeginUpdate();
            excludeList.BeginUpdate();
            includeList.Items.Clear();
            includeList.Items.AddRange(mainMenu.uniqueTags.ToArray());
            excludeList.Items.Clear();
            excludeList.Items.AddRange(mainMenu.uniqueTags.ToArray());
            includeList.EndUpdate();
            excludeList.EndUpdate();
        }

        private void uncheckAllButton_Click(object sender, EventArgs e)
        {
            ratingList.BeginUpdate();
            for (int i = 0; i < ratingList.Items.Count; i++)
                ratingList.SetItemChecked(i, false);
            ratingList.EndUpdate();
            includeList.BeginUpdate();
            excludeList.BeginUpdate();
            includeList.Items.Clear();
            excludeList.Items.Clear();
            excludeList.Items.AddRange(mainMenu.uniqueTags.ToArray());
            excludeList.ItemCheck -= excludeList_ItemCheck;
            for (int i = 0; i < excludeList.Items.Count; i++)
                excludeList.SetItemChecked(i, true);
            excludeList.ItemCheck += excludeList_ItemCheck;
            includeList.EndUpdate();
            excludeList.EndUpdate();
            exclusionMode.SelectedIndex = 1;
        }

        private void ratingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ratingList.SelectedIndex = -1;
        }

        private void tagSearch_Leave(object sender, EventArgs e)
        {
            if (tagSearch.Text == String.Empty)
                return;
            try
            {
                includeList.SelectedItem = tagSearch.Text;
            }
            catch { }
            try
            {
                excludeList.SelectedItem = tagSearch.Text;
            }
            catch { }
            tagSearch.Text = String.Empty;
        }

        private void tagSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = null;
        }

        private void includeList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            excludeList.SelectedIndex = -1;
            if (e.NewValue == CheckState.Checked)
                excludeList.Items.Remove(includeList.SelectedItem.ToString());
            else
            {
                if (excludeList.Items.Count == 0)
                    excludeList.Items.Add(includeList.SelectedItem.ToString());
                else if (tagIndex[includeList.SelectedItem.ToString()] < tagIndex[excludeList.Items[0].ToString()])
                    excludeList.Items.Insert(0, includeList.SelectedItem.ToString());
                else if (tagIndex[includeList.SelectedItem.ToString()] > tagIndex[excludeList.Items[excludeList.Items.Count - 1].ToString()])
                    excludeList.Items.Add(includeList.SelectedItem.ToString());
                else
                {
                    bool inserted = false;
                    for (int i = 0; i < excludeList.Items.Count - 1; i++)
                    {
                        if (tagIndex[includeList.SelectedItem.ToString()] > tagIndex[excludeList.Items[i].ToString()] && tagIndex[includeList.SelectedItem.ToString()] < tagIndex[excludeList.Items[i + 1].ToString()])
                        {

                            excludeList.Items.Insert(i + 1, includeList.SelectedItem.ToString());
                            inserted = true;
                            break;
                        }
                    }
                    if (inserted == false)
                        excludeList.Items.Add(includeList.SelectedItem.ToString());
                }
                excludeList.SelectedIndex = excludeList.SelectedIndex = excludeList.Items.IndexOf(includeList.SelectedItem.ToString());
            }
        }

        private void excludeList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            includeList.SelectedIndex = -1;
            if (e.NewValue == CheckState.Checked)
                includeList.Items.Remove(excludeList.SelectedItem.ToString());
            else
            {
                if (includeList.Items.Count == 0)
                    includeList.Items.Add(excludeList.SelectedItem.ToString());
                else if (tagIndex[excludeList.SelectedItem.ToString()] < tagIndex[includeList.Items[0].ToString()])
                    includeList.Items.Insert(0, excludeList.SelectedItem.ToString());
                else if (tagIndex[excludeList.SelectedItem.ToString()] > tagIndex[includeList.Items[includeList.Items.Count - 1].ToString()])
                    includeList.Items.Add(excludeList.SelectedItem.ToString());
                else
                {
                    bool inserted = false;
                    for (int i = 0; i < includeList.Items.Count - 1; i++)
                    {
                        if (tagIndex[excludeList.SelectedItem.ToString()] > tagIndex[includeList.Items[i].ToString()] && tagIndex[excludeList.SelectedItem.ToString()] < tagIndex[includeList.Items[i + 1].ToString()])
                        {
                            includeList.Items.Insert(i + 1, excludeList.SelectedItem.ToString());
                            inserted = true;
                            break;
                        }
                    }
                    if (inserted == false)
                        includeList.Items.Add(excludeList.SelectedItem.ToString());
                }
                includeList.SelectedIndex = includeList.Items.IndexOf(excludeList.SelectedItem.ToString());
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tagsFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainMenu.includedRatings.Clear();
            mainMenu.includedTags.Clear();
            mainMenu.excludedTags.Clear();
            for (int i = 0; i < ratingList.CheckedItems.Count; i++)
                mainMenu.includedRatings.Add(ratingList.CheckedItems[i].ToString());
            for (int i = 0; i < includeList.CheckedItems.Count; i++)
                mainMenu.includedTags.Add(includeList.CheckedItems[i].ToString());
            for (int i = 0; i < excludeList.CheckedItems.Count; i++)
                mainMenu.excludedTags.Add(excludeList.CheckedItems[i].ToString());
            mainMenu.inclusionMode = inclusionMode.SelectedIndex == 0;
            mainMenu.exclusionMode = exclusionMode.SelectedIndex == 0;
        }

        private void includeList_MouseDown(object sender, MouseEventArgs e)
        {
            includeList.SelectedIndex = includeList.IndexFromPoint(e.Location);
        }

        private void excludeList_MouseDown(object sender, MouseEventArgs e)
        {
            excludeList.SelectedIndex = excludeList.IndexFromPoint(e.Location);
        }

        private void occurancesLabel_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
