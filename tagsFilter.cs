using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Library_Manager
{
    public partial class tagsFilter : Form
    {
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
            foreach (string tag in mainMenu.uniqueTags)
            {
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
            if (e.NewValue == CheckState.Checked)
                excludeList.Items.Remove(includeList.SelectedItem.ToString());
            else
                excludeList.Items.Add(includeList.SelectedItem.ToString());
        }

        private void excludeList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                includeList.Items.Remove(excludeList.SelectedItem.ToString());
            else
                includeList.Items.Add(excludeList.SelectedItem.ToString());
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
    }
}
