using Flurl;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;
using Ionic.Zip;
using System.Net.Http.Json;
using System.IO;

namespace Manga_Library_Manager
{
    public partial class mangaDownloader : Form
    {
        private bool downloading = false, excludedExtras = false, linkChanged = false;
        private List<string> tempChapterIDs = new List<string>();
        private string savingPath;
        private int startOffset = 2;
        private List<decimal> tempChapterNumbers = new List<decimal>();
        private List<int> checkedIndexes = new List<int>(), duplicateIndexes = new List<int>(), chapterNrPages = new List<int>();
        private mainMenu.eBook becomingBook = new mainMenu.eBook();

        private List<Label> progressLabels = new List<Label>();
        private List<ProgressBar> progressLoadingBars = new List<ProgressBar>();
        private List<TableLayoutPanel> progressFlowPanels = new List<TableLayoutPanel>();
        public List<string> chapterDownloads = new List<string>(), chapterFolders = new List<string>();
        public List<List<string>> pageFileNames = new List<List<string>>();
        public List<decimal> selectedChapterNumbers = new List<decimal>();
        public List<int> chapterPages = new List<int>();
        public bool dataSaving, isCbz = false;
        public string mangaID, coverFileName, cbzPrefix = String.Empty, bookAuthor, bookArtist, bookCreatedDate;
        public static Stopwatch atHome = new Stopwatch(), generalApi = new Stopwatch();
        public static TimeSpan apiSpan;
        public static int atHomeCalls = 0, generalApiCalls = 0, totalPages;

        public mangaDownloader()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void mangaDownloader_Load(object sender, EventArgs e)
        {
            qualityDropDown.SelectedIndex = 0;
            formatDropDown.SelectedIndex = 0;
            if (mainMenu.updateManga == false)
            {
                titleSelectionDropDown.SelectedIndex = 0;
                mangaID = Path.GetDirectoryName(Environment.ProcessPath) + Path.DirectorySeparatorChar;
                folderBrowserDialog.InitialDirectory = Path.GetDirectoryName(Environment.ProcessPath);
                savingPath = Path.GetDirectoryName(Environment.ProcessPath);
            }
            else
            {
                linkTextBox.Text = mainMenu.currentlySelectedBook.Link;
                titleSelectionDropDown.Items.Add(mainMenu.currentlySelectedBook.Title);
                titleSelectionDropDown.SelectedItem = mainMenu.currentlySelectedBook.Title;
                becomingBook.Title = mainMenu.currentlySelectedBook.Title;
                if (Path.IsPathRooted(mainMenu.currentlySelectedBook.Path) == true)
                    savingPath = Path.GetDirectoryName(mainMenu.currentlySelectedBook.Path);
                else
                    savingPath = Path.GetDirectoryName(Environment.ProcessPath);
                if (Path.GetExtension(mainMenu.currentlySelectedBook.Path) == ".cbz")
                    formatDropDown.SelectedIndex = 1;
                toolTip.SetToolTip(locationButton, Path.GetDirectoryName(mainMenu.currentlySelectedBook.Path));
                searchButton_Click(searchButton, EventArgs.Empty);
            }
        }

        private void controlStatus(bool enabled)
        {
            if (mainMenu.updateManga == false)
            {
                linkTextBox.Enabled = enabled;
                searchButton.Enabled = enabled;
                locationButton.Enabled = enabled;
                addToManagerCheckBox.Enabled = enabled;
                titleSelectionDropDown.Enabled = enabled;
                formatDropDown.Enabled = enabled;
            }
            qualityDropDown.Enabled = enabled;
            selectedChaptersList.Enabled = enabled;
            removeExtrasButton.Enabled = enabled;
            downloadButton.Enabled = enabled;
        }

        private void linkTextBox_TextChanged(object sender, EventArgs e)
        {
            linkChanged = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            linkChanged = false;
            try
            {
                if (linkTextBox.Text == String.Empty)
                    return;
                if (linkTextBox.Text.Split('/')[2] != "mangadex.org")
                    throw new Exception("domain");
                if (linkTextBox.Text.Split('/')[3] != "title")
                    throw new Exception("title");
                if (linkTextBox.Text.Split('/')[4] == null || Uri.TryCreate(linkTextBox.Text, UriKind.Absolute, out Uri uriResult) == false || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                if (ex.Message == "domain")
                    MessageBox.Show("The link is not from MangaDex!\nPlease only use MangaDex links.", "Bad domain", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (ex.Message == "title")
                    MessageBox.Show("The link is not a manga!\nPlease link to the manga.", "Bad type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("The link is incorrect or is not a link!", "Bad link", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            linkTextBox.Enabled = false;
            searchButton.Enabled = false;
            tempChapterIDs.Clear();
            tempChapterNumbers.Clear();
            checkedIndexes.Clear();
            duplicateIndexes.Clear();
            selectedChaptersList.BeginUpdate();
            selectedChaptersList.Items.Clear();
            selectedChaptersList.EndUpdate();
            if (mainMenu.updateManga == false)
            {
                titleSelectionDropDown.Items.Clear();
                titleSelectionDropDown.Items.Add("Select which title to use:");
                titleSelectionDropDown.SelectedIndex = 0;
            }
            mangaID = linkTextBox.Text.Split('/')[4];
            List<string> extraPages = new List<string>();
            JObject apiResult, mangaInfo;
            List<string> tempChapterGroups = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
                try
                {
                    Task<string> task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed".SetQueryParam("translatedLanguage[]", mainMenu.selectedLanguage).AppendQueryParam("limit", 100).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" }).AppendQueryParam("includes[]=scanlation_group")));
                    apiResult = JObject.Parse(task.Result);
                    calledAPI(false);
                    int apiTotal = apiResult.SelectToken("total").Value<int>();
                    if (apiTotal > 100)
                    {
                        int passes = apiTotal / 100;
                        while (passes > 0)
                        {
                            task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed".SetQueryParam("translatedLanguage[]", mainMenu.selectedLanguage).AppendQueryParam("limit", 100).AppendQueryParam("offset", Convert.ToString(passes * 100)).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" }).AppendQueryParam("includes[]=scanlation_group")));
                            extraPages.Add(task.Result);
                            calledAPI(false);
                            passes--;
                        }
                    }
                    foreach (JToken entry in apiResult.SelectToken("data"))
                    {
                        try
                        {
                            if (mainMenu.updateManga == true && Convert.ToDecimal(entry.SelectToken("attributes").SelectToken("chapter").Value<string>(), new CultureInfo("en-US")) <= mainMenu.currentlySelectedBook.LastChapter)
                                continue;
                            bool groupFound = false;
                            tempChapterNumbers.Add(Convert.ToDecimal(entry.SelectToken("attributes").SelectToken("chapter").Value<string>(), new CultureInfo("en-US")));
                            chapterNrPages.Add(entry.SelectToken("attributes").SelectToken("pages").Value<int>());
                            foreach (JToken group in entry.SelectToken("relationships"))
                                if (group.SelectToken("type").Value<string>() == "scanlation_group")
                                {
                                    groupFound = true;
                                    tempChapterGroups.Add(group.SelectToken("attributes").SelectToken("name").Value<string>());
                                    break;
                                }
                            if (groupFound == false)
                                tempChapterGroups.Add("Anonymous (No Group)");
                            tempChapterIDs.Add(entry.SelectToken("id").Value<string>());
                        }
                        catch { }
                    }
                    if (extraPages.Count > 0)
                        foreach (string page in extraPages)
                        {
                            apiResult = apiResult = JObject.Parse(page);
                            foreach (JToken entry in apiResult.SelectToken("data"))
                            {
                                try
                                {
                                    if (mainMenu.updateManga == true && Convert.ToDecimal(entry.SelectToken("attributes").SelectToken("chapter").Value<string>(), new CultureInfo("en-US")) <= mainMenu.currentlySelectedBook.LastChapter)
                                        continue;
                                    bool groupFound = false;
                                    tempChapterNumbers.Add(Convert.ToDecimal(entry.SelectToken("attributes").SelectToken("chapter").Value<string>(), new CultureInfo("en-US")));
                                    chapterNrPages.Add(entry.SelectToken("attributes").SelectToken("pages").Value<int>());
                                    foreach (JToken group in entry.SelectToken("relationships"))
                                        if (group.SelectToken("type").Value<string>() == "scanlation_group")
                                        {
                                            groupFound = true;
                                            tempChapterGroups.Add(group.SelectToken("attributes").SelectToken("name").Value<string>());
                                            break;
                                        }
                                    if (groupFound == false)
                                        tempChapterGroups.Add("Anonymous (No Group)");
                                    tempChapterIDs.Add(entry.SelectToken("id").Value<string>());
                                }
                                catch { }
                            }
                        }
                    if (mainMenu.updateManga == false)
                    {
                        task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "?includes[]=cover_art&includes[]=author&includes[]=artist"));
                        mangaInfo = JObject.Parse(task.Result);
                        calledAPI(false);
                        foreach (JToken title in mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("title"))
                            titleSelectionDropDown.Items.Add(((JProperty)title).Value);
                        bool gotSubtitle = false;
                        foreach (JToken subTitle in mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("altTitles"))
                            try
                            {
                                titleSelectionDropDown.Items.Add(subTitle.SelectToken(mainMenu.selectedLanguage).Value<string>());
                                gotSubtitle = true;
                            }
                            catch { }
                        if (gotSubtitle == false)
                            foreach (JToken subTitle in mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("altTitles"))
                                try
                                {
                                    titleSelectionDropDown.Items.Add(subTitle.SelectToken("en").Value<string>());
                                }
                                catch { }
                        foreach (JToken rel in mangaInfo.SelectToken("data").SelectToken("relationships"))
                        {
                            string type = rel.SelectToken("type").Value<string>();
                            if (type == "cover_art")
                                coverFileName = rel.SelectToken("attributes").SelectToken("fileName").Value<string>();
                            else if (type == "author")
                                bookAuthor = rel.SelectToken("attributes").SelectToken("name").Value<string>();
                            else if (type == "artist")
                                bookArtist = rel.SelectToken("attributes").SelectToken("name").Value<string>();
                        }
                        bookCreatedDate = mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("createdAt").Value<string>();

                        #region Creating temporary eBook with what I have so far (title will only be set by changing the dropdown, will force change if no selection is made)

                        becomingBook.Ongoing = mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("status").Value<string>() == "ongoing" || mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("status").Value<string>() == "hiatus";
                        becomingBook.Link = linkTextBox.Text;
                        string tempRating = mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("contentRating").Value<string>();
                        tempRating = tempRating.Substring(0, 1).ToUpper() + tempRating.Substring(1);
                        becomingBook.ContentRating = tempRating;
                        becomingBook.Tags = new List<string>();
                        foreach (JToken tag in mangaInfo.SelectToken("data").SelectToken("attributes").SelectToken("tags"))
                        {
                            string groupTemp = tag.SelectToken("attributes").SelectToken("group").Value<string>();
                            if (groupTemp == "genre" || groupTemp == "theme")
                                try
                                {
                                    becomingBook.Tags.Add(tag.SelectToken("attributes").SelectToken("name").SelectToken(mainMenu.selectedLanguage).Value<string>());
                                }
                                catch
                                {
                                    becomingBook.Tags.Add(tag.SelectToken("attributes").SelectToken("name").SelectToken("en").Value<string>());
                                }
                        }

                        #endregion
                    }
                }
                catch
                {
                    MessageBox.Show("Could not retrieve chapter list!", "API error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (mainMenu.updateManga == false)
                        linkTextBox.Enabled = true;
                    searchButton.Enabled = true;
                    return;
                }
            }
            bool doneGoneDidThisOne = true;
            for (int j = 1; (j <= tempChapterNumbers.Count - 1) && (doneGoneDidThisOne == true); j++)
            {
                doneGoneDidThisOne = false;
                for (int i = 0; i < tempChapterNumbers.Count - 1; i++)
                    if (tempChapterNumbers[i] > tempChapterNumbers[i + 1])
                    {
                        decimal temp = tempChapterNumbers[i + 1];
                        tempChapterNumbers[i + 1] = tempChapterNumbers[i];
                        tempChapterNumbers[i] = temp;
                        string temp2 = tempChapterGroups[i + 1];
                        tempChapterGroups[i + 1] = tempChapterGroups[i];
                        tempChapterGroups[i] = temp2;
                        temp2 = tempChapterIDs[i + 1];
                        tempChapterIDs[i + 1] = tempChapterIDs[i];
                        tempChapterIDs[i] = temp2;
                        int temp3 = chapterNrPages[i + 1];
                        chapterNrPages[i + 1] = chapterNrPages[i];
                        chapterNrPages[i] = temp3;
                        doneGoneDidThisOne = true;
                    }
            }
            Dictionary<string, int> groupAppearances = new Dictionary<string, int>();
            for (int i = 0; i < tempChapterNumbers.Count - 1; i++)
            {
                if (groupAppearances.ContainsKey(tempChapterGroups[i]) == true)
                    groupAppearances[tempChapterGroups[i]]++;
                else
                    groupAppearances[tempChapterGroups[i]] = 1;
                Dictionary<string, int> tempAuthors = new Dictionary<string, int>();
                if (tempChapterNumbers[i] == tempChapterNumbers[i + 1])
                {
                    while (tempChapterNumbers[i] == tempChapterNumbers[i + 1])
                    {
                        if (tempChapterGroups[i] != "Anonymous (No Group)")
                            tempAuthors[tempChapterGroups[i]] = i;
                        duplicateIndexes.Add(i);
                        i++;
                        if (groupAppearances.ContainsKey(tempChapterGroups[i]) == true)
                            groupAppearances[tempChapterGroups[i]]++;
                        else
                            groupAppearances[tempChapterGroups[i]] = 1;
                    }
                    if (tempChapterGroups[i] != "Anonymous (No Group)")
                        tempAuthors[tempChapterGroups[i]] = i;
                    duplicateIndexes.Add(i);
                    if (groupAppearances.ContainsKey(tempChapterGroups[i]) == true)
                        groupAppearances[tempChapterGroups[i]]++;
                    else
                        groupAppearances[tempChapterGroups[i]] = 1;
                    Dictionary<string, int> tempAuthorCount = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> author in tempAuthors)
                        tempAuthorCount[author.Key] = groupAppearances[author.Key];
                    tempAuthorCount = tempAuthorCount.OrderByDescending(key => key.Value).ToDictionary();
                    bool found = false;
                    foreach (KeyValuePair<string, int> author in tempAuthorCount)
                    {
                        if (tempAuthors.ContainsKey(author.Key) == false)
                            continue;
                        checkedIndexes.Add(tempAuthors[author.Key]);
                        found = true;
                        break;
                    }
                    if (found == false)
                        checkedIndexes.Add(i);
                    continue;
                }
                checkedIndexes.Add(i);
            }
            selectedChaptersList.BeginUpdate();
            for (int i = 0; i < tempChapterNumbers.Count; i++)
            {
                if (duplicateIndexes.Contains(i) == true)
                    selectedChaptersList.Items.Add("Chapter " + Convert.ToString(tempChapterNumbers[i], new CultureInfo("en-US")) + " by " + tempChapterGroups[i], checkedIndexes.Contains(i));
                else
                    selectedChaptersList.Items.Add("Chapter " + Convert.ToString(tempChapterNumbers[i], new CultureInfo("en-US")), true);
            }
            selectedChaptersList.EndUpdate();
            controlStatus(true);
        }

        private void removeExtrasButton_Click(object sender, EventArgs e)
        {
            if (excludedExtras == false)
            {
                if (MessageBox.Show("The program has no way of knowing whether a chapter is actually an extra or not. It just deselects all chapters with decimals (i.e. Chapter 5.5).\nAre you sure you want to exclude them?", "Remove extras", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
                excludedExtras = true;
                removeExtrasButton.Text = "Include Extras";
                removeExtrasButton.Font = new Font(removeExtrasButton.Font, FontStyle.Bold | FontStyle.Italic);
                selectedChaptersList.BeginUpdate();
                for (int i = 0; i < tempChapterNumbers.Count; i++)
                    if (tempChapterNumbers[i] - Convert.ToDecimal(Convert.ToInt32(tempChapterNumbers[i])) != 0m)
                        selectedChaptersList.SetItemChecked(i, false);
                selectedChaptersList.EndUpdate();
            }
            else
            {
                excludedExtras = false;
                removeExtrasButton.Text = "Exclude Extras";
                removeExtrasButton.Font = new Font(removeExtrasButton.Font, FontStyle.Bold);
                selectedChaptersList.BeginUpdate();
                for (int i = 0; i < tempChapterNumbers.Count; i++)
                    if (tempChapterNumbers[i] - Convert.ToDecimal(Convert.ToInt32(tempChapterNumbers[i])) != 0m)
                        selectedChaptersList.SetItemChecked(i, checkedIndexes.Contains(i));
                selectedChaptersList.EndUpdate();
            }
        }

        private void locationButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            savingPath = folderBrowserDialog.SelectedPath;
            if (mainMenu.updateManga == false)
                becomingBook.Path = savingPath;
            if (folderBrowserDialog.SelectedPath == Path.GetDirectoryName(Environment.ProcessPath))
                toolTip.SetToolTip(locationButton, "Defaults to wherever the executable is located");
            else
                toolTip.SetToolTip(locationButton, savingPath);

        }

        private void titleSelectionDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mainMenu.updateManga == false)
                becomingBook.Title = titleSelectionDropDown.Text;
        }

        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (linkChanged == true)
            {
                MessageBox.Show("The link has changed!\nPlease click the search button first.", "Link changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (downloading == false)
            {
                if (selectedChaptersList.Items.Count == 0)
                {
                    MessageBox.Show("No chapters found!\nPlease try a different language or double-check the link.", "No chapters available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                chapterDownloads.Clear();
                selectedChapterNumbers.Clear();
                chapterPages.Clear();
                chapterFolders.Clear();
                for (int i = 0; i < selectedChaptersList.Items.Count; i++)
                    if (selectedChaptersList.GetItemChecked(i) == true)
                    {
                        chapterDownloads.Add(tempChapterIDs[i]);
                        selectedChapterNumbers.Add(tempChapterNumbers[i]);
                        chapterPages.Add(chapterNrPages[i]);
                    }
                becomingBook.LastChapter = selectedChapterNumbers.Max();
                if (chapterDownloads.Count == 0)
                {
                    MessageBox.Show("No chapters selected!\nPlease select at least one chapter.", "No chapters selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (chapterDownloads.Count == 1)
                {
                    if (MessageBox.Show("Do you want to start downloading 1 chapter?", "Start download", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                else
                {
                    if (MessageBox.Show("Do you want to start downloading " + Convert.ToString(selectedChapterNumbers.Count) + " chapters?", "Start download", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                downloading = true;
                if (titleSelectionDropDown.SelectedIndex == 0)
                    titleSelectionDropDown.SelectedIndex = 1;
                string fileName = becomingBook.Title;
                foreach (char c in Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(Convert.ToString(c), String.Empty);
                becomingBook.Path = Path.Join(savingPath, fileName);
                dataSaving = qualityDropDown.SelectedIndex == 0;
                isCbz = formatDropDown.SelectedIndex == 1;
                cbzPrefix = String.Empty;
                totalPages = 0;
                foreach (int nr in chapterPages)
                    totalPages += nr;
                progressLabels.Clear();
                progressLoadingBars.Clear();
                progressFlowPanels.Clear();
                pageFileNames.Clear();
                SendMessage(this.Handle, 11, false, 0);
                controlStatus(false);
                downloadButton.Text = "Cancel Download";
                downloadButton.Font = new(downloadButton.Font, FontStyle.Bold | FontStyle.Italic);
                downloadButton.Enabled = true;
                for (int i = 0; i < selectedChapterNumbers.Count; i++)
                {
                    Label chapterLabel = new Label();
                    chapterLabel.AutoSize = true;
                    chapterLabel.Font = this.Font;
                    chapterLabel.BackColor = Color.DimGray;
                    chapterLabel.ForeColor = Color.White;
                    chapterLabel.TextAlign = ContentAlignment.MiddleLeft;
                    chapterLabel.Text = "Ch." + Convert.ToString(selectedChapterNumbers[i]);
                    chapterLabel.Dock = DockStyle.Fill;
                    progressLabels.Add(chapterLabel);
                    ProgressBar chapterBar = new ProgressBar();
                    chapterBar.Maximum = chapterPages[i];
                    chapterBar.Dock = DockStyle.Fill;
                    progressLoadingBars.Add(chapterBar);
                    TableLayoutPanel panel = new TableLayoutPanel();
                    panel.ColumnCount = 2;
                    panel.RowCount = 1;
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
                    panel.BackColor = Color.DimGray;
                    panel.AutoSize = true;
                    panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    panel.Dock = DockStyle.Top;
                    progressFlowPanels.Add(panel);
                    panel.Controls.Add(chapterLabel, 0, 0);
                    panel.Controls.Add(chapterBar, 1, 0);
                    flowLayout.Controls.Add(panel);
                    flowLayout.SetFlowBreak(panel, true);
                }
                SendMessage(this.Handle, 11, true, 0);
                this.Refresh();
                int j = 0;
                while (true)
                    if (Directory.Exists(savingPath + Path.DirectorySeparatorChar + "temp manga " + Convert.ToString(j)) == false)
                    {
                    Retry:
                        try
                        {
                            Directory.CreateDirectory(savingPath + Path.DirectorySeparatorChar + "temp manga " + Convert.ToString(j));
                            savingPath = savingPath + Path.DirectorySeparatorChar + "temp manga " + Convert.ToString(j);
                            break;
                        }
                        catch
                        {
                            if (mainMenu.updateManga == false)
                                if (MessageBox.Show("Cannot write to selected location!\nWould you like to pick a different location?", "Write error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                                {
                                    cancelDownload();
                                    return;
                                }
                                else
                                {
                                    locationButton_Click(sender, EventArgs.Empty);
                                    goto Retry;
                                }
                            else
                            {
                                MessageBox.Show("Cannot write to selected location!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                cancelDownload();
                                return;
                            }
                        }
                    }
                    else
                        j++;
                if (mainMenu.updateManga == false)
                {
                    if (isCbz == false)
                    {
                        for (int i = 0; i < chapterDownloads.Count; i++)
                        {
                            try
                            {
                                string current = savingPath + Path.DirectorySeparatorChar + Convert.ToString(i + 1);
                                Directory.CreateDirectory(current);
                                current += Path.DirectorySeparatorChar;
                                Directory.CreateDirectory(current + "css");
                                File.WriteAllText(current + "css" + Path.DirectorySeparatorChar + "style.css", "img {\r\n    max-height: 100%;\r\n    max-width: 100%;\r\n}");
                                Directory.CreateDirectory(current + "img");
                                Directory.CreateDirectory(current + "xhtml");
                            }
                            catch
                            {
                                MessageBox.Show("Cannot write to selected location!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                cancelDownload();
                                return;
                            }
                        }
                        downloaderThread.RunWorkerAsync(argument: 0);
                        Directory.CreateDirectory(savingPath + Path.DirectorySeparatorChar + "META-INF");
                        File.WriteAllText(Path.Join(savingPath, "META-INF", "container.xml"), "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">\r\n   <rootfiles>\r\n      <rootfile full-path=\"content.opf\" media-type=\"application/oebps-package+xml\"/>\r\n   </rootfiles>\r\n</container>\r\n");
                        File.WriteAllText(Path.Join(savingPath, "mimetype"), "application/epub+zip");
                        File.WriteAllText(Path.Join(savingPath, "cover.xhtml"), "\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\"><head><title>Cover</title><style type=\"text/css\" title=\"override_css\">\r\n@page {padding: 0pt; margin:0pt}\r\nbody { text-align: center; padding:0pt; margin: 0pt; }\r\ndiv { margin: 0pt; padding: 0pt; }\r\n</style></head><body><div>\r\n<img src=\"cover.jpg\" alt=\"cover\"/>\r\n</div></body></html>\r\n");

                        File.WriteAllText(Path.Join(savingPath, "content.opf"), "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<package xmlns=\"http://www.idpf.org/2007/opf\" version=\"2.0\">\r\n  <metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:opf=\"http://www.idpf.org/2007/opf\">\r\n    <dc:title></dc:title>\r\n    <dc:creator opf:role=\"aut\"></dc:creator>\r\n    <dc:rights>Copyrights as per source stories</dc:rights>\r\n    <dc:description></dc:description>\r\n    <dc:date></dc:date>\r\n    <dc:language></dc:language>\r\n    <dc:identifier id=\"book-id\"></dc:identifier>\r\n    <meta name=\"cover\" content=\"coverimageid\" />\r\n  </metadata>\r\n  <manifest>\r\n    <item id=\"cover\" href=\"cover.xhtml\" media-type=\"application/xhtml+xml\" />\r\n    <item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\" />\r\n    <item id=\"coverimageid\" href=\"cover.jpg\" media-type=\"image/jpeg\" />\r\n  </manifest>\r\n  <spine toc=\"ncx\">\r\n    <itemref idref=\"cover\" linear=\"yes\" />\r\n  </spine>\r\n  <guide>\r\n    <reference type=\"cover\" title=\"Cover\" href=\"cover.xhtml\" />\r\n  </guide>\r\n</package>");
                        File.WriteAllText(Path.Join(savingPath, "toc.ncx"), "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<ncx version=\"2005-1\" xmlns=\"http://www.daisy.org/z3986/2005/ncx/\">\r\n  <head>\r\n    <meta name=\"dtb:uid\" content=\"\" />\r\n    <meta name=\"dtb:depth\" content=\"2\" />\r\n    <meta name=\"dtb:totalPageCount\" content=\"0\" />\r\n    <meta name=\"dtb:maxPageNumber\" content=\"0\" />\r\n  </head>\r\n  <docTitle>\r\n    <text>\r\n    </text>\r\n  </docTitle>\r\n  <navMap>\r\n</navMap>\r\n</ncx>");
                    }
                    else
                        downloaderThread.RunWorkerAsync(argument: 0);
                }
                else
                {
                    using (ZipFile manga = ZipFile.Read(mainMenu.currentlySelectedBook.Path))
                    {
                        manga.ExtractAll(savingPath);
                    }
                    int offset = 1;
                    if (isCbz == false)
                    {
                        while (Directory.Exists(savingPath + Path.DirectorySeparatorChar + Convert.ToString(offset)) == true)
                            offset++;
                        startOffset = offset;
                        for (int i = 0; i < chapterDownloads.Count; i++)
                        {
                            try
                            {
                                string current = savingPath + Path.DirectorySeparatorChar + Convert.ToString(offset);
                                Directory.CreateDirectory(current);
                                current += Path.DirectorySeparatorChar;
                                Directory.CreateDirectory(current + "css");
                                File.WriteAllText(current + "css" + Path.DirectorySeparatorChar + "style.css", "img {\r\n    max-height: 100%;\r\n    max-width: 100%;\r\n}");
                                Directory.CreateDirectory(current + "img");
                                Directory.CreateDirectory(current + "xhtml");
                                offset++;
                            }
                            catch
                            {
                                MessageBox.Show("Cannot write to selected location!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                cancelDownload();
                                return;
                            }
                        }
                        downloaderThread.RunWorkerAsync(argument: startOffset - 1);
                    }
                    else
                    {
                        string[] entries = Directory.GetFiles(savingPath);
                        int i = 0;
                        while (Path.GetFileNameWithoutExtension(entries[0]).Substring(i, 1) == Path.GetFileNameWithoutExtension(entries.Last()).Substring(i, 1))
                        {
                            cbzPrefix += Path.GetFileNameWithoutExtension(entries[0]).Substring(i, 1);
                            i++;
                        }
                        downloaderThread.RunWorkerAsync(argument: entries.Count() - 1);
                    }
                }
            }
            else
            {
                if (MessageBox.Show("All progress will be lost.\nAre you sure you want to cancel the download?", "Cancel download", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
                if (downloaderThread.IsBusy == true)
                    downloaderThread.CancelAsync();
                if (epubMakerThread.IsBusy == true)
                    epubMakerThread.CancelAsync();
            }
        }

        private void cancelDownload()
        {
            downloading = false;
            SendMessage(this.Handle, 11, false, 0);
            downloadButton.Text = "Start Downloading";
            downloadButton.Font = new(downloadButton.Font, FontStyle.Bold);
            foreach (Panel panel in progressFlowPanels.ToArray())
                panel.Dispose();
            foreach (ProgressBar progressBar in progressLoadingBars.ToArray())
                progressBar.Dispose();
            foreach (TableLayoutPanel panel in progressFlowPanels.ToArray())
                panel.Dispose();
            controlStatus(true);
            SendMessage(this.Handle, 11, true, 0);
            this.Refresh();
        }

        public void calledAPI(bool isAtHome)
        {
            if (isAtHome == true)
                if (atHome.IsRunning == true)
                {
                    atHome.Stop();
                    atHomeCalls++;
                    apiSpan = atHome.Elapsed;
                    if (atHomeCalls >= 40)
                    {
                        if (apiSpan.Seconds < 60)
                            Thread.Sleep(60000 - apiSpan.Milliseconds);
                        atHomeCalls = 0;
                        atHome.Reset();
                    }
                    atHome.Start();
                }
                else
                    atHome.Start();
            else
            {
                if (generalApi.IsRunning == true)
                {
                    generalApi.Stop();
                    generalApiCalls++;
                    apiSpan = generalApi.Elapsed;
                    if (generalApiCalls >= 5)
                    {
                        if (apiSpan.Milliseconds < 1000)
                            Thread.Sleep(1000 - apiSpan.Milliseconds);
                        generalApiCalls = 0;
                        generalApi.Reset();
                    }
                    generalApi.Start();
                }
                else
                    generalApi.Start();
            }
        }

        private void downloaderThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            TimeSpan span;
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
            int offset = (int)e.Argument, cbzPageNumber = 1;
            for (int chapterIndex = offset; chapterIndex < chapterDownloads.Count + offset; chapterIndex++)
            {
                string currentPath = Path.Join(savingPath, Convert.ToString(chapterIndex + 1), "img", String.Empty);
            Retry:
                if (((BackgroundWorker)sender).CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                bool doReports = true;
                List<string> pageNames;
                string pageLink;
                JObject chapterLinksResponse;
                Task<string> task;
                try
                {
                    task = client.GetStringAsync(new Uri("https://api.mangadex.org/at-home/server/" + chapterDownloads[chapterIndex - offset]));
                    chapterLinksResponse = JObject.Parse(task.Result);
                    calledAPI(true);
                    if (chapterLinksResponse.SelectToken("baseUrl").Value<string>().Contains("mangadex.org"))
                        doReports = false;
                    if (dataSaving == false)
                    {
                        pageNames = chapterLinksResponse.SelectToken("chapter").SelectToken("data").ToObject<List<string>>();
                        pageLink = chapterLinksResponse.SelectToken("baseUrl").Value<string>() + "/data/" + chapterLinksResponse.SelectToken("chapter").SelectToken("hash").Value<string>() + "/";
                    }
                    else
                    {
                        pageNames = chapterLinksResponse.SelectToken("chapter").SelectToken("dataSaver").ToObject<List<string>>();
                        pageLink = chapterLinksResponse.SelectToken("baseUrl").Value<string>() + "/data-saver/" + chapterLinksResponse.SelectToken("chapter").SelectToken("hash").Value<string>() + "/";
                    }
                }
                catch
                {
                    MessageBox.Show("Could not contact the API.\nThe download will now cancel.", "API error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
                int pageNumber = 1;
                List<string> chapterPagesPaths = new List<string>();
                pageFileNames.Add(chapterPagesPaths);
                foreach (string pageName in pageNames)
                {
                    if (((BackgroundWorker)sender).CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }
                    Task<HttpResponseMessage> pageTask;
                    HttpResponseMessage pageResponse = new HttpResponseMessage(), reportResponse;
                    Image page;
                    try
                    {
                        sw.Start();
                        pageTask = client.GetAsync(pageLink + pageName);
                        pageResponse = pageTask.Result;
                        sw.Stop();
                        pageResponse.EnsureSuccessStatusCode();
                    }
                    catch
                    {
                        sw.Stop();
                        span = sw.Elapsed;
                        bool cached = false;
                        if (pageResponse.Headers.TryGetValues("X-Cache", out IEnumerable<string> headers) == true)
                            if (headers.First().StartsWith("HIT") == true)
                                cached = true;
                        HttpContent payload = report(pageLink + pageName, true, cached, pageResponse.Content.ReadAsStream().Length, span.Milliseconds);
                        try
                        {
                            if (doReports == true)
                            {
                                pageTask = client.PostAsync("https://api.mangadex.network/report", payload);
                                reportResponse = pageTask.Result;
                            }
                        }
                        catch { }
                        sw.Reset();
                        goto Retry;
                    }
                    try
                    {
                        span = sw.Elapsed;
                        bool cached = false;
                        if (pageResponse.Headers.TryGetValues("X-Cache", out IEnumerable<string> headers) == true)
                            if (headers.First().StartsWith("HIT") == true)
                                cached = true;
                        HttpContent payload = report(pageLink + pageName, true, cached, pageResponse.Content.ReadAsStream().Length, span.Milliseconds);
                        try
                        {
                            if (doReports == true)
                            {
                                pageTask = client.PostAsync("https://api.mangadex.network/report", payload);
                                reportResponse = pageTask.Result;
                            }
                        }
                        catch { }
                        sw.Reset();
                    }
                    catch { }
                    try
                    {
                        page = Image.FromStream(pageResponse.Content.ReadAsStream());
                        if (isCbz == false)
                        {
                            string fileName = pageNumber.ToString("D" + chapterPages[chapterIndex - offset].ToString().Length) + Path.GetExtension(pageName);
                            page.Save(Path.Join(currentPath, fileName));
                            File.WriteAllText(Path.Join(savingPath, Convert.ToString(chapterIndex + 1), "xhtml", pageNumber - 1 + ".xhtml"), $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <link href=\"../css/style.css\" rel=\"stylesheet\" type=\"text/css\"/>\r\n    <title>{fileName}</title>\r\n</head>\r\n<body>\r\n    <div>\r\n        <img alt=\"{fileName}\" src=\"../img/{fileName}\"/>\r\n    </div>\r\n</body>\r\n</html>\r\n");
                            pageNumber++;
                            chapterPagesPaths.Add(Path.Join(currentPath, fileName));
                        }
                        else
                        {
                            int currentPageNumber = offset + cbzPageNumber;
                            string fileName;
                            if (mainMenu.updateManga == false)
                                fileName = "pg-" + currentPageNumber.ToString("D" + totalPages.ToString().Length) + ".jpg";
                            else
                                fileName = cbzPrefix + currentPageNumber.ToString("D" + (totalPages + offset).ToString().Length) + ".jpg";
                            page.Save(Path.Join(savingPath, fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                            cbzPageNumber++;
                        }
                        page.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("Could not write the page file.\nThe download will now cancel.", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        return;
                    }
                    ((BackgroundWorker)sender).ReportProgress(chapterIndex - offset);
                }
            }
            try
            {
                Image cover = Image.FromStream(client.GetStreamAsync("https://uploads.mangadex.org/covers/" + mangaID + "/" + coverFileName).Result);
                if (isCbz == false)
                    cover.Save(Path.Join(savingPath, "cover.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                else
                {
                    string fileName;
                    if (mainMenu.updateManga == false)
                        fileName = "pg-" + 0.ToString("D" + totalPages.ToString().Length) + ".jpg";
                    else
                        fileName = cbzPrefix + 0.ToString("D" + (totalPages + offset).ToString().Length) + ".jpg";
                    cover.Save(Path.Join(savingPath, fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                cover.Dispose();
            }
            catch { }
        }

        public HttpContent report(string url, bool success, bool cached, long bytes, int duration)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values["url"] = url;
            values["success"] = success;
            values["cached"] = cached;
            values["bytes"] = bytes;
            values["duration"] = duration;
            return JsonContent.Create(values, mediaType: new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }

        private void downloaderThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressLoadingBars[e.ProgressPercentage].PerformStep();
        }

        private void downloaderThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                if (epubMakerThread.IsBusy == true)
                    epubMakerThread.CancelAsync();
                try
                {
                    Directory.Delete(savingPath, true);
                }
                catch
                {
                    MessageBox.Show("Could not delete the temporary folder!\nPlease delete it yourself.", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cancelDownload();
                return;
            }
            if (isCbz == false)
                epubMakerThread.RunWorkerAsync(argument: startOffset);
            else
            {
                downloadButton.Enabled = false;
                becomingBook.Path += ".cbz";
                string fileName = becomingBook.Title;
                foreach (char c in Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(Convert.ToString(c), String.Empty);
                try
                {
                    using ZipFile cbz = new ZipFile();
                    cbz.AddDirectory(savingPath);
                    cbz.Save(becomingBook.Path);
                    try
                    {
                        Directory.Delete(savingPath, true);
                    }
                    catch
                    {
                        MessageBox.Show("Could not delete the temporary folder!\nPlease delete it yourself.", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("The download was completed successfully, but the .cbz file could not be created.\nYou can create it yourself.", "Archiving failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (mainMenu.updateManga == false)
                {
                    if (addToManagerCheckBox.Checked == true)
                        this.DialogResult = DialogResult.OK;
                }
                else
                    this.DialogResult = DialogResult.Yes;
                mainMenu.currentlySelectedBook = becomingBook;
                downloading = false;
                MessageBox.Show("The download has finished successfully! Enjoy!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void epubMakerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            bool root = false;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(Path.Join(savingPath, "content.opf"));
                root = true;
            }
            catch
            {
                doc.Load(Path.Join(savingPath, "OEBPS", "content.opf"));
            }
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            foreach (decimal chapter in selectedChapterNumbers)
            {
                string nr = padDecimal(chapter, "D" + selectedChapterNumbers.Count.ToString().Length.ToString());
                doc.DocumentElement.SelectSingleNode("//dc:description", nsmgr).InnerText += "Ch." + nr + "<br>";
            }
            if (mainMenu.updateManga == false)
            {
                doc.DocumentElement.SelectSingleNode("//dc:title", nsmgr).InnerText = becomingBook.Title;
                doc.DocumentElement.SelectSingleNode("//dc:creator", nsmgr).InnerText = bookAuthor + ", " + bookArtist;
                doc.DocumentElement.SelectSingleNode("//dc:date", nsmgr).InnerText = bookCreatedDate;
                doc.DocumentElement.SelectSingleNode("//dc:language", nsmgr).InnerText = mainMenu.selectedLanguage;
                doc.DocumentElement.SelectSingleNode("//dc:identifier", nsmgr).InnerText = linkTextBox.Text;
            }
            XmlNode manifestNode = null, spineNode = null;
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                if (node.Name == "manifest")
                    manifestNode = node;
                else if (node.Name == "spine")
                    spineNode = node;
            for (int i = 0; i < selectedChapterNumbers.Count; i++)
            {
                XmlElement css = doc.CreateElement("item", manifestNode.NamespaceURI);
                css.SetAttribute("id", "css-" + selectedChapterNumbers[i]);
                css.SetAttribute("href", pageFileNames[i][0].Split(Path.DirectorySeparatorChar)[pageFileNames[i][0].Split(Path.DirectorySeparatorChar).Count() - 3] + "/css/style.css");
                css.SetAttribute("media-type", "text/css");
                manifestNode.AppendChild(css);
                for (int j = 0; j < chapterPages[i]; j++)
                {
                    XmlElement xhtml = doc.CreateElement("item", manifestNode.NamespaceURI);
                    xhtml.SetAttribute("id", "xhtml-" + selectedChapterNumbers[i] + "-" + Path.GetFileNameWithoutExtension(pageFileNames[i][j]));
                    xhtml.SetAttribute("href", pageFileNames[i][j].Split(Path.DirectorySeparatorChar)[pageFileNames[i][j].Split(Path.DirectorySeparatorChar).Count() - 3] + "/xhtml/" + Convert.ToString(Convert.ToInt32(Path.GetFileNameWithoutExtension(pageFileNames[i][j])) - 1) + ".xhtml");
                    xhtml.SetAttribute("media-type", "application/xhtml+xml");
                    XmlElement image = doc.CreateElement("item", manifestNode.NamespaceURI);
                    image.SetAttribute("id", "pic-" + selectedChapterNumbers[i] + "-" + Path.GetFileNameWithoutExtension(pageFileNames[i][j]));
                    image.SetAttribute("href", pageFileNames[i][j].Split(Path.DirectorySeparatorChar)[pageFileNames[i][j].Split(Path.DirectorySeparatorChar).Count() - 3] + "/img/" + Path.GetFileName(pageFileNames[i][j]));
                    image.SetAttribute("media-type", "image/png");
                    XmlElement spine = doc.CreateElement("itemref", manifestNode.NamespaceURI);
                    spine.SetAttribute("idref", "xhtml-" + selectedChapterNumbers[i] + "-" + Path.GetFileNameWithoutExtension(pageFileNames[i][j]));
                    spine.SetAttribute("linear", "yes");
                    manifestNode.AppendChild(xhtml);
                    manifestNode.AppendChild(image);
                    spineNode.AppendChild(spine);
                }
            }
            try
            {
                if (root == true)
                    doc.Save(Path.Join(savingPath, "content.opf"));
                else
                    doc.Save(Path.Join(savingPath, "OEBPS", "content.opf"));
            }
            catch
            {
                MessageBox.Show("Could not write the content.opf file!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
            XmlDocument toc = new XmlDocument();
            root = false;
            try
            {
                toc.Load(Path.Join(savingPath, "toc.ncx"));
                root = true;
            }
            catch
            {
                toc.Load(Path.Join(savingPath, "OEBPS", "toc.ncx"));
            }
            foreach (XmlNode node in toc.DocumentElement.ChildNodes)
                if (node.Name == "head")
                    foreach (XmlNode child in node.ChildNodes)
                        if (child.Attributes[0].Value == "dtb:uid")
                        {
                            child.Attributes[1].Value = doc.DocumentElement.SelectSingleNode("//dc:identifier", nsmgr).InnerText;
                            goto Found;
                        }
                    Found:
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("toc", toc.DocumentElement.GetAttribute("xmlns"));
            toc.DocumentElement.SelectSingleNode("toc:docTitle/toc:text", ns).InnerText = becomingBook.Title;
            int playOrder = 0;
            XmlNode navMap = toc.DocumentElement.SelectSingleNode("toc:navMap", ns);
            if (mainMenu.updateManga == true)
                foreach (XmlNode node in navMap.SelectNodes("toc:navPoint", ns))
                    playOrder = Convert.ToInt32(node.Attributes["playOrder"].Value);
            playOrder++;
            for (int i = 0; i < selectedChapterNumbers.Count; i++)
            {
                string chapterName = "Ch." + Convert.ToString(selectedChapterNumbers[i]);
                XmlElement navPoint = toc.CreateElement("navPoint", toc.DocumentElement.GetAttribute("xmlns"));
                navPoint.SetAttribute("id", "book" + Convert.ToString(playOrder));
                navPoint.SetAttribute("playOrder", Convert.ToString(playOrder));
                navMap.AppendChild(navPoint);
                XmlElement navLabel = toc.CreateElement("navLabel", toc.DocumentElement.GetAttribute("xmlns"));
                navPoint.AppendChild(navLabel);
                XmlElement labelText = toc.CreateElement("text", toc.DocumentElement.GetAttribute("xmlns"));
                labelText.InnerText = chapterName;
                navLabel.AppendChild(labelText);
                XmlElement content = toc.CreateElement("content", toc.DocumentElement.GetAttribute("xmlns"));
                content.SetAttribute("src", pageFileNames[i][0].Split(Path.DirectorySeparatorChar)[pageFileNames[i][0].Split(Path.DirectorySeparatorChar).Count() - 3] + "/xhtml/0.xhtml");
                navPoint.AppendChild(content);
                for (int j = 0; j < chapterPages[i]; j++)
                {
                    string path = pageFileNames[i][j].Split(Path.DirectorySeparatorChar)[pageFileNames[i][j].Split(Path.DirectorySeparatorChar).Count() - 3] + "/img/" + Path.GetFileName(pageFileNames[i][j]);
                    XmlElement pageNavPoint = toc.CreateElement("navPoint", toc.DocumentElement.GetAttribute("xmlns"));
                    pageNavPoint.SetAttribute("id", "book" + Convert.ToString(playOrder) + "-page" + Convert.ToString(j));
                    pageNavPoint.SetAttribute("playOrder", Convert.ToString(j + 1));
                    navPoint.AppendChild(pageNavPoint);
                    XmlElement pageNavLabel = toc.CreateElement("navLabel", toc.DocumentElement.GetAttribute("xmlns"));
                    pageNavPoint.AppendChild(pageNavLabel);
                    XmlElement pageLabelText = toc.CreateElement("text", toc.DocumentElement.GetAttribute("xmlns"));
                    pageLabelText.InnerText = "Page " + Convert.ToString(j + 1);
                    pageNavLabel.AppendChild(pageLabelText);
                    XmlElement pageContent = toc.CreateElement("content", toc.DocumentElement.GetAttribute("xmlns"));
                    pageContent.SetAttribute("src", pageFileNames[i][j].Split(Path.DirectorySeparatorChar)[pageFileNames[i][j].Split(Path.DirectorySeparatorChar).Count() - 3] + "/xhtml/" + Convert.ToString(j) + ".xhtml");
                    pageNavPoint.AppendChild(pageContent);
                }
                playOrder++;
            }
            try
            {
                if (root == true)
                    toc.Save(Path.Join(savingPath, "toc.ncx"));
                else
                    toc.Save(Path.Join(savingPath, "OEBPS", "toc.ncx"));
            }
            catch
            {
                MessageBox.Show("Could not write the toc.ncx file!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
        }

        private void epubMakerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
                return;
            downloadButton.Enabled = false;
            becomingBook.Path += ".epub";
            string fileName = becomingBook.Title;
            foreach (char c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(Convert.ToString(c), String.Empty);
            try
            {
                using ZipFile epub = new ZipFile();
                epub.AddDirectory(savingPath);
                epub.Save(becomingBook.Path);
                try
                {
                    Directory.Delete(savingPath, true);
                }
                catch
                {
                    MessageBox.Show("Could not delete the temporary folder!\nPlease delete it yourself.", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("The download was completed successfully, but the .epub file could not be created.\nYou can create it yourself.", "Archiving failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (mainMenu.updateManga == false)
            {
                if (addToManagerCheckBox.Checked == true)
                    this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Yes;
            mainMenu.currentlySelectedBook = becomingBook;
            downloading = false;
            MessageBox.Show("The download has finished successfully! Enjoy!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void mangaDownloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (downloading == false)
                return;
            if (downloaderThread.IsBusy == true || epubMakerThread.IsBusy == true)
                if (MessageBox.Show("All progress will be lost.\nAre you sure you want to cancel the download?", "Cancel download", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            if (downloaderThread.IsBusy == true)
                downloaderThread.CancelAsync();
            if (epubMakerThread.IsBusy == true)
                epubMakerThread.CancelAsync();
            try
            {
                Directory.Delete(savingPath, true);
            }
            catch
            {
                MessageBox.Show("Could not delete the temporary folder!\nPlease delete it yourself.", "Write error", MessageBoxButtons.OK);
            }
        }

        public string padDecimal(decimal value, string padding)
        {
            if (value - Convert.ToInt32(Math.Floor(value)) != 0)
                return Convert.ToInt32(Math.Floor(value)).ToString(padding) + (value - Convert.ToInt32(Math.Floor(value))).ToString().Substring(1);
            return Convert.ToInt32(value).ToString(padding);
        }
    }
}
