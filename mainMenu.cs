using Newtonsoft.Json;
using System.Xml;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Flurl;

namespace Manga_Library_Manager
{
    public partial class mainMenu : Form
    {
        public class eBook
        {
            public string Title;
            public string Path;
            public decimal LastChapter;
            public bool Ongoing;
            public string Link;
            public string CotentRating;
            public List<string> Tags;

            public override string ToString()
            {
                return Title;
            }
        }

        private List<eBook> books = new List<eBook>();
        private AutoCompleteStringCollection searchTextBoxAutomcompleteStrings = new AutoCompleteStringCollection();
        private List<string> files = new List<string>(), ratingsList = new List<string>();
        private bool filterToggled = false, filterTags = false;
        private Regex chapterRegex = new Regex("Ch\\.[0-9.]+"), titleSanitationRegex = new Regex("[^a-zA-Z0-9 ]");
        private Dictionary<string, string> titleDictionary = new Dictionary<string, string>();
        private Dictionary<string, List<string>> tagsDictionary = new Dictionary<string, List<string>>();
        private List<bool> apiResponseOngoing = new List<bool>();
        private DateTime lastClickedTime = DateTime.MinValue;
        public static string jsonDump;
        public static List<string> uniqueTags = new List<string>(), includedTags = new List<string>(), excludedTags = new List<string>(), includedRatings = new List<string>() { "Safe", "Suggestive", "Erotica", "Pornographic" };
        public static Dictionary<string, int> tagsUsage = new Dictionary<string, int>();
        public static eBook currentlySelectedBook;
        public static bool inclusionMode = true, exclusionMode = false, resetTags = false; //true = and, false = or
        public static List<eBook> booksCopy = new List<eBook>();

        public mainMenu()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void mainMenu_Load(object sender, EventArgs e)
        {
            lastChapterNumber.Maximum = Decimal.MaxValue;
            if (File.Exists("Manga Library Manager.json") == false)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter("Manga Library Manager.json"))
                    {
                        writer.Write("[]");
                    }
                }
                catch
                {
                    MessageBox.Show("Could not write library json!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(2);
                }
            }
            try
            {
                using (StreamReader reader = new StreamReader("Manga Library Manager.json"))
                {
                    string line = reader.ReadToEnd();
                    books = JsonConvert.DeserializeObject<List<eBook>>(line);
                }
            }
            catch
            {
                MessageBox.Show("Could not read library json!", "Read error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(1);
            }
            mangaList.BeginUpdate();
            foreach (eBook book in books)
            {
                mangaList.Items.Add(book);
                searchTextBoxAutomcompleteStrings.Add(book.Title);
                foreach (string tag in book.Tags)
                {
                    if (tagsUsage.ContainsKey(tag))
                        tagsUsage[tag]++;
                    if (uniqueTags.Contains(tag) == false)
                    {
                        uniqueTags.Add(tag);
                        tagsUsage[tag] = 1;
                    }
                }
            }
            mangaList.EndUpdate();
            searchTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            searchTextBox.AutoCompleteCustomSource = searchTextBoxAutomcompleteStrings;
            mangaDescControls(false);
        }

        private void mangaDescControls(bool show)
        {
            label1.Visible = show;
            label2.Visible = show;
            ongoingCheckbox.Visible = show;
            lastChapterNumber.Visible = show;
            openInExplorerButton.Visible = show;
            checkOnlineButton.Visible = show;
            deleteEntryButton.Visible = show;
            linkTextBox.Visible = show;
            resetButton.Visible = show;
            mangaDescCover.Visible = show;
            editTagsButton.Visible = show;
            tagsTextBox.Visible = show;
            ratingLabel.Visible = show;
            if (show == false)
            {
                mangaDescTitle.Text = string.Empty;
                if (mangaDescCover.BackgroundImage != null)
                    mangaDescCover.BackgroundImage.Dispose();
            }
        }

        private void mangaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchTextBox.Text = String.Empty;
            lastChapterOnlineLabel.Visible = false;
            lastChapterOnlineLabel.Text = "Last chapter online: ...";
            ongoingOnlineLabel.Visible = false;
            ongoingOnlineLabel.Text = String.Empty;
            ratingLabel.Text = "Rating: Unknown";
            tagsTextBox.Text = String.Empty;
            linkTextBox.Items.Clear();
            if (mangaDescCover.BackgroundImage != null)
                mangaDescCover.BackgroundImage.Dispose();
            if (mangaList.SelectedIndex == -1)
            {
                mangaDescControls(false);
                return;
            }
            bool notFound = false;
            if (File.Exists(((eBook)mangaList.SelectedItem).Path) == false)
            {
                if (MessageBox.Show("Selected manga file no longer exists!\nWould you like to remove it from the list?", "Manga file not found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    deleteEntryButton_Click(sender, EventArgs.Empty);
                    return;
                }
                notFound = true;
            }
            if (((eBook)mangaList.SelectedItem).Link == String.Empty)
            {
                titleDictionary.Clear();
                tagsDictionary.Clear();
                ratingsList.Clear();
                apiResponseOngoing.Clear();
                if (thread1.IsBusy == false)
                    thread1.RunWorkerAsync(argument: ((eBook)mangaList.SelectedItem).Title);
            }
            mangaDescTitle.Text = ((eBook)mangaList.SelectedItem).Title;
            if (notFound == false)
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    using (ZipFile epub = ZipFile.Read(((eBook)mangaList.SelectedItem).Path))
                    {
                        foreach (ZipEntry entry in epub.Entries)
                            if (entry.FileName == "cover.jpg" || entry.FileName == "cover.jpeg" || entry.FileName == "cover.png" || entry.FileName == "cover.webp")
                            {
                                entry.Extract(stream);
                                stream.Position = 0;
                                break;
                            }
                    }
                    mangaDescCover.BackgroundImage = Image.FromStream(stream);
                }
                catch
                {
                    mangaDescCover.BackgroundImage = Properties.Resources.coverError;
                }
            }
            ongoingCheckbox.Checked = ((eBook)mangaList.SelectedItem).Ongoing;
            lastChapterNumber.Value = ((eBook)mangaList.SelectedItem).LastChapter;
            linkTextBox.Text = ((eBook)mangaList.SelectedItem).Link;
            if (((eBook)mangaList.SelectedItem).CotentRating != String.Empty)
                ratingLabel.Text = "Rating: " + ((eBook)mangaList.SelectedItem).CotentRating;
            if (((eBook)mangaList.SelectedItem).Tags.Count > 0)
                tagsTextBox.Text = "Tags: " + String.Join(", ", ((eBook)mangaList.SelectedItem).Tags);
            mangaDescControls(true);
        }

        private void thread1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string searchTitle = ((string)e.Argument).ToLower();
            searchTitle = titleSanitationRegex.Replace(searchTitle, "");
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
            try
            {
                string response = client.GetStringAsync("https://api.mangadex.org/manga".SetQueryParam("title", searchTitle).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" }).AppendQueryParam("order[relevance]", "desc")).Result;
                e.Result = (JObject.Parse(response), (string)e.Argument);
            }
            catch
            {
                return;
            }
        }

        private void thread1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ValueTuple<JObject, string> t = ((ValueTuple<JObject, string>)e.Result);
            JObject apiResult = t.Item1;
            List<string> autocompleteTemp = new List<string>(), tempTags = new List<string>();
            try
            {
                foreach (JToken entry in apiResult.SelectToken("data"))
                {
                    tempTags.Clear();
                    string title = entry.SelectToken("attributes").SelectToken("title").SelectToken("en").Value<string>();
                    titleDictionary[title] = "https://mangadex.org/title/" + entry.SelectToken("id").Value<string>();
                    apiResponseOngoing.Add(entry.SelectToken("attributes").SelectToken("status").Value<string>() == "ongoing" || entry.SelectToken("attributes").SelectToken("status").Value<string>() == "hiatus");
                    autocompleteTemp.Add(title);
                    foreach (JToken tag in entry.SelectToken("attributes").SelectToken("tags"))
                    {
                        string groupTemp = tag.SelectToken("attributes").SelectToken("group").Value<string>();
                        if (groupTemp == "genre" || groupTemp == "theme")
                            tempTags.Add(tag.SelectToken("attributes").SelectToken("name").SelectToken("en").Value<string>());
                    }
                    tagsDictionary[title] = tempTags.ToList();
                    string tempRating = entry.SelectToken("attributes").SelectToken("contentRating").Value<string>();
                    tempRating = tempRating.Substring(0, 1).ToUpper() + tempRating.Substring(1);
                    ratingsList.Add(tempRating);
                }
            }
            catch
            {
                return;
            }
            if (((eBook)mangaList.SelectedItem).Title != t.Item2)
            {
                foreach (eBook book in books)
                    if (book.Title == t.Item2)
                    {
                        book.Link = titleDictionary[autocompleteTemp[0]];
                        book.Ongoing = apiResponseOngoing[0];
                        book.CotentRating = ratingsList[0];
                        book.Tags = tagsDictionary[autocompleteTemp[0]];
                        foreach (string tag in tagsDictionary[autocompleteTemp[0]])
                            if (uniqueTags.Contains(tag) == false)
                            {
                                uniqueTags.Add(tag);
                                tagsUsage[tag] = 1;
                            }
                        break;
                    }
            }
            else
            {
                linkTextBox.Items.AddRange(autocompleteTemp.ToArray());
                if (linkTextBox.Items.Count > 0)
                    linkTextBox.SelectedIndex = 0;
            }
        }

        private void mangaList_MouseDown(object sender, MouseEventArgs e)
        {
            mangaList.SelectedIndex = mangaList.IndexFromPoint(e.Location);
        }

        private void linkTextBox_TextChanged(object sender, EventArgs e)
        {
            if (titleDictionary.ContainsKey(linkTextBox.Text) == true)
            {
                ongoingCheckbox.Checked = apiResponseOngoing[linkTextBox.SelectedIndex];
                ratingLabel.Text = "Rating: " + ratingsList[linkTextBox.SelectedIndex];
                ((eBook)mangaList.SelectedItem).CotentRating = ratingsList[linkTextBox.SelectedIndex];
                tagsTextBox.Text = "Tags: " + String.Join(", ", tagsDictionary[linkTextBox.Text]);
                ((eBook)mangaList.SelectedItem).Tags = tagsDictionary[linkTextBox.Text];
                BeginInvoke(new Action(() => linkTextBox.Text = titleDictionary[linkTextBox.Text]));
            }
            ((eBook)mangaList.SelectedItem).Link = linkTextBox.Text;
            toolTip.SetToolTip(linkTextBox, linkTextBox.Text);
        }

        private void linkTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                MessageBox.Show(linkTextBox.Text);
            if (e.Button == MouseButtons.Left)
                try
                {
                    if ((DateTime.Now - lastClickedTime).TotalMilliseconds > SystemInformation.DoubleClickTime)
                        return;
                    if (Uri.TryCreate(((eBook)mangaList.SelectedItem).Link, UriKind.Absolute, out Uri uriResult) == false || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                        return;
                    Process.Start(new ProcessStartInfo(linkTextBox.Text) { UseShellExecute = true });
                }
                finally
                {
                    lastClickedTime = DateTime.Now;
                }
        }

        private void ongoingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ((eBook)mangaList.SelectedItem).Ongoing = ongoingCheckbox.Checked;
        }

        private void lastChapterNumber_ValueChanged(object sender, EventArgs e)
        {
            ((eBook)mangaList.SelectedItem).LastChapter = lastChapterNumber.Value;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream stream = new MemoryStream();
            string title = string.Empty, desc = string.Empty;
            try
            {
                using (ZipFile epub = ZipFile.Read(((eBook)mangaList.SelectedItem).Path))
                {
                    epub["content.opf"].Extract(stream);
                }
                stream.Position = 0;
                doc.Load(stream);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                try
                {
                    title = doc.DocumentElement.SelectSingleNode("//dc:title", nsmgr).InnerText;
                    desc = doc.DocumentElement.SelectSingleNode("//dc:description", nsmgr).InnerText;
                }
                catch
                {
                    MessageBox.Show("Could not parse content file!", "Error parsing content.opf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Could not read the manga at:\n" + ((eBook)mangaList.SelectedItem).Path, "Error reading manga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                MatchCollection chapters = new Regex("Ch\\.[0-9.]+").Matches(desc);
                List<decimal> tempChapters = new List<decimal>();
                foreach (Match match in chapters)
                    tempChapters.Add(Convert.ToDecimal(match.Value.Substring(3), new CultureInfo("en-US")));
                ((eBook)mangaList.SelectedItem).LastChapter = tempChapters.Max();
            }
            catch
            {
                ((eBook)mangaList.SelectedItem).LastChapter = 0M;
            }
            lastChapterNumber.Value = ((eBook)mangaList.SelectedItem).LastChapter;
        }

        private void openInExplorerButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select," + ((eBook)mangaList.SelectedItem).Path);
            }
            catch
            {
                MessageBox.Show("Could not launch explorer.exe at the file path:\n" + ((eBook)mangaList.SelectedItem).Path, "Launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkOnlineButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (((eBook)mangaList.SelectedItem).Link == String.Empty)
                    throw new Exception("no link");
                if (((eBook)mangaList.SelectedItem).Link.Split('/')[2] != "mangadex.org")
                    throw new Exception("domain");
                if (((eBook)mangaList.SelectedItem).Link.Split('/')[3] != "title")
                    throw new Exception("title");
                if (((eBook)mangaList.SelectedItem).Link.Split('/')[4] == null || Uri.TryCreate(((eBook)mangaList.SelectedItem).Link, UriKind.Absolute, out Uri uriResult) == false || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                if (ex.Message == "no link")
                    MessageBox.Show("There is no link set!\nPlease set a link.", "No link set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (ex.Message == "domain")
                    MessageBox.Show("The link is not from MangaDex!\nPlease only use MangaDex links.", "Bad domain", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (ex.Message == "title")
                    MessageBox.Show("The link is not a manga!\nPlease link to the manga.", "Bad type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("The link is incorrect or is not a link!", "Bad link", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            checkOnlineButton.Enabled = false;
            lastChapterOnlineLabel.Visible = true;
            ongoingOnlineLabel.Visible = true;
            lastChapterOnlineLabel.Refresh();
            ongoingOnlineLabel.Refresh();
            checkOnlineTimer.Start();
            string mangaID = ((eBook)mangaList.SelectedItem).Link.Split('/')[4];
            List<string> extraPages = new List<string>();
            JObject apiResult;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
                try
                {
                    Task<string> task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed?translatedLanguage[]=en&limit=500".AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" })));
                    apiResult = JObject.Parse(task.Result);
                    int apiTotal = apiResult.SelectToken("total").Value<int>();
                    if (apiTotal > 500)
                    {
                        int passes = apiTotal / 500;
                        if (passes <= 4)
                            while (passes > 0)
                            {
                                task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed?translatedLanguage[]=en&limit=500".AppendQueryParam("offset", Convert.ToString(passes * 500)).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" })));
                                extraPages.Add(task.Result);
                                passes--;
                            }
                        else
                        {
                            for (int i = 1; i <= passes; i++)
                            {
                                if (i % 5 == 0)
                                    Thread.Sleep(1000);
                                task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed?translatedLanguage[]=en&limit=500".AppendQueryParam("offset", Convert.ToString(passes * 500)).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" })));
                                extraPages.Add(task.Result);
                            }
                        }
                    }
                    List<decimal> tempChapters = new List<decimal>();
                    foreach (JToken entry in apiResult.SelectToken("data"))
                    {
                        try
                        {
                            tempChapters.Add(Convert.ToDecimal(entry.SelectToken("attributes").SelectToken("chapter").Value<string>(), new CultureInfo("en-US")));
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
                                    tempChapters.Add(Convert.ToDecimal(entry.SelectToken("attributes").SelectToken("chapter").Value<string>(), new CultureInfo("en-US")));
                                }
                                catch { }
                            }
                        }
                    lastChapterOnlineLabel.Text = "Last chapter online: " + Convert.ToString(tempChapters.Max());
                    if (tempChapters.Max() - ((eBook)mangaList.SelectedItem).LastChapter == 0)
                        ongoingOnlineLabel.Text = "Up to date!";
                    else
                    {
                        if (tempChapters.Max() - ((eBook)mangaList.SelectedItem).LastChapter == 1)
                            ongoingOnlineLabel.Text = Convert.ToString(Convert.ToInt32(Math.Ceiling(tempChapters.Max() - ((eBook)mangaList.SelectedItem).LastChapter))) + " chapter ahead.";
                        else
                            ongoingOnlineLabel.Text = Convert.ToString(Convert.ToInt32(Math.Ceiling(tempChapters.Max() - ((eBook)mangaList.SelectedItem).LastChapter))) + " chapters ahead.";
                    }
                }
                catch
                {
                    MessageBox.Show("Could not retrieve chapter list!", "API error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lastChapterOnlineLabel.Visible = false;
                    ongoingOnlineLabel.Visible = false;
                    return;
                }
            }
        }

        private void deleteEntryButton_Click(object sender, EventArgs e)
        {
            if (sender == deleteEntryButton)
            {
                DialogResult result = MessageBox.Show("Would you like to also delete the file?", "Deletion confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(((eBook)mangaList.SelectedItem).Path);
                    }
                    catch
                    {
                        MessageBox.Show("Could not delete file!", "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            books.Remove(((eBook)mangaList.SelectedItem));
            searchTextBoxAutomcompleteStrings.Remove(((eBook)mangaList.SelectedItem).Title);
            mangaList.Items.RemoveAt(mangaList.SelectedIndex);
        }

        private void editTagsButton_Click(object sender, EventArgs e)
        {
            currentlySelectedBook = (eBook)mangaList.SelectedItem;
            uniqueTags.Clear();
            tagsUsage.Clear();
            foreach (eBook book in books)
                foreach (string tag in book.Tags)
                {
                    if (tagsUsage.ContainsKey(tag))
                        tagsUsage[tag]++;
                    if (uniqueTags.Contains(tag) == false)
                    {
                        uniqueTags.Add(tag);
                        tagsUsage[tag] = 1;
                    }
                }
            editTags tagsForm = new editTags();
            tagsForm.ShowDialog();
            tagsForm.Dispose();
            if (resetTags == true)
            {
                resetTags = false;
                resyncTags();
                return;
            }
            ratingLabel.Text = "Rating: " + ((eBook)mangaList.SelectedItem).CotentRating;
            if (((eBook)mangaList.SelectedItem).Tags.Count > 0)
                tagsTextBox.Text = "Tags: " + String.Join(", ", ((eBook)mangaList.SelectedItem).Tags);
            else
                tagsTextBox.Text = String.Empty;
        }

        private void resyncTags()
        {
            try
            {
                if (((eBook)mangaList.SelectedItem).Link == String.Empty)
                    throw new Exception("no link");
                if (((eBook)mangaList.SelectedItem).Link.Split('/')[2] != "mangadex.org")
                    throw new Exception("domain");
                if (((eBook)mangaList.SelectedItem).Link.Split('/')[3] != "title")
                    throw new Exception("title");
                if (((eBook)mangaList.SelectedItem).Link.Split('/')[4] == null || Uri.TryCreate(((eBook)mangaList.SelectedItem).Link, UriKind.Absolute, out Uri uriResult) == false || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                if (ex.Message == "no link")
                    MessageBox.Show("There is no link set!\nPlease set a link.", "No link set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (ex.Message == "domain")
                    MessageBox.Show("The link is not from MangaDex!\nPlease only use MangaDex links.", "Bad domain", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (ex.Message == "title")
                    MessageBox.Show("The link is not a manga!\nPlease link to the manga.", "Bad type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("The link is incorrect or is not a link!", "Bad link", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ratingLabel.Text = "Rating: Unknown";
            tagsTextBox.Text = String.Empty;
            List<string> tempTags = ((eBook)mangaList.SelectedItem).Tags.ToList();
            foreach (string tag in tempTags)
            {
                ((eBook)mangaList.SelectedItem).Tags.Remove(tag);
                tagsUsage[tag]--;
                if (tagsUsage[tag] == 0)
                {
                    uniqueTags.Remove(tag);
                    tagsUsage.Remove(tag);
                }
            }
            string mangaID = ((eBook)mangaList.SelectedItem).Link.Split('/')[4], tempRating;
            JObject apiResult;
            tempTags.Clear();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
                try
                {
                    Task<string> task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID.SetQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" })));
                    apiResult = JObject.Parse(task.Result);
                    foreach (JToken tag in apiResult.SelectToken("data").SelectToken("attributes").SelectToken("tags"))
                    {
                        string groupTemp = tag.SelectToken("attributes").SelectToken("group").Value<string>();
                        if (groupTemp == "genre" || groupTemp == "theme")
                            tempTags.Add(tag.SelectToken("attributes").SelectToken("name").SelectToken("en").Value<string>());
                    }
                    tempRating = apiResult.SelectToken("data").SelectToken("attributes").SelectToken("contentRating").Value<string>();
                    tempRating = tempRating.Substring(0, 1).ToUpper() + tempRating.Substring(1);
                    ((eBook)mangaList.SelectedItem).CotentRating = tempRating;
                    ongoingCheckbox.Checked = apiResult.SelectToken("data").SelectToken("attributes").SelectToken("status").Value<string>() == "ongoing" || apiResult.SelectToken("data").SelectToken("attributes").SelectToken("status").Value<string>() == "hiatus";
                }
                catch
                {
                    MessageBox.Show("Could not retrieve tag list!", "API error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            foreach (string tag in tempTags)
            {
                ((eBook)mangaList.SelectedItem).Tags.Add(tag);
                if (tagsUsage.ContainsKey(tag))
                    tagsUsage[tag]++;
                else
                {
                    tagsUsage[tag] = 1;
                    uniqueTags.Add(tag);
                }
            }
            ratingLabel.Text = "Rating: " + ((eBook)mangaList.SelectedItem).CotentRating;
            if (((eBook)mangaList.SelectedItem).Tags.Count > 0)
                tagsTextBox.Text = "Tags: " + String.Join(", ", ((eBook)mangaList.SelectedItem).Tags);
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            if (searchTextBox.Text == String.Empty)
                return;
            foreach (eBook ebook in mangaList.Items)
                if (ebook.Title.ToLower().Contains(searchTextBox.Text.ToLower()))
                {
                    mangaList.SelectedItem = ebook;
                    break;
                }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = null;
        }

        private void addMangaButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mangaList.BeginUpdate();
                foreach (string path in openFileDialog.FileNames)
                    addManga(path, true);
                mangaList.EndUpdate();
            }
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                files.Clear();
                FindMeFiles(folderBrowserDialog.SelectedPath);
                mangaList.BeginUpdate();
                foreach (string file in files)
                    addManga(file, false);
                mangaList.EndUpdate();
            }
        }

        private void FindMeFiles(string path)
        {
            try
            {
                string[] entries = Directory.GetFiles(path);
                foreach (string entry in entries)
                    if (Path.GetExtension(entry) == ".epub")
                        files.Add(entry);
                string[] subdirs = Directory.GetDirectories(path);
                foreach (string subdir in subdirs)
                    FindMeFiles(subdir);
            }
            catch { }
        }

        private void checkOnlineTimer_Tick(object sender, EventArgs e)
        {
            checkOnlineTimer.Stop();
            checkOnlineButton.Enabled = true;
        }

        private void addManga(string path, bool duplicateWarn)
        {
            eBook temp = new eBook();
            XmlDocument doc = new XmlDocument();
            MemoryStream stream = new MemoryStream();
            string title = string.Empty, desc = string.Empty;
            try
            {
                using (ZipFile epub = ZipFile.Read(path))
                {
                    try
                    {
                        epub["content.opf"].Extract(stream);
                    }
                    catch
                    {
                        epub["OEBPS/content.opf"].Extract(stream);
                    }
                }
                stream.Position = 0;
                doc.Load(stream);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                try
                {
                    title = doc.DocumentElement.SelectSingleNode("//dc:title", nsmgr).InnerText;
                    desc = doc.DocumentElement.SelectSingleNode("//dc:description", nsmgr).InnerText;
                }
                catch
                {
                    if (duplicateWarn == true)
                        MessageBox.Show("Could not parse content file!", "Error parsing content.opf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                if (duplicateWarn == true)
                    MessageBox.Show("Could not read the manga at:\n" + path, "Error reading manga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool dupe = false;
            foreach (eBook name in mangaList.Items)
            {
                if (name.Title == title)
                {
                    if (duplicateWarn == true)
                        MessageBox.Show(name + " is already on the list!", "Duplicate entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dupe = true;
                    break;
                }
            }
            if (dupe == false)
            {
                temp.Title = title;
                if (path.Contains(Path.GetDirectoryName(Environment.ProcessPath)))
                    temp.Path = path.Substring(Path.GetDirectoryName(Environment.ProcessPath).Length);
                else
                    temp.Path = path;
                if (temp.Path[0] == Path.DirectorySeparatorChar)
                    temp.Path = temp.Path.Substring(1);
                temp.Ongoing = false;
                temp.Link = String.Empty;
                temp.CotentRating = String.Empty;
                temp.Tags = new List<string>();
                try
                {
                    MatchCollection chapters = chapterRegex.Matches(desc);
                    List<decimal> tempChapters = new List<decimal>();
                    foreach (Match match in chapters)
                        tempChapters.Add(Convert.ToDecimal(match.Value.Substring(3), new CultureInfo("en-US")));
                    temp.LastChapter = tempChapters.Max();
                }
                catch
                {
                    temp.LastChapter = 0M;
                }
                books.Add(temp);
                if (filterToggled == false && filterTags == false)
                {
                    mangaList.Items.Add(temp);
                    searchTextBoxAutomcompleteStrings.Add(title);
                }
            }
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            mangaList.SelectedIndex = -1;
            filterToggled = !filterToggled;
            if (filterToggled == true)
            {
                filterButton.Font = new Font(filterButton.Font, FontStyle.Bold | FontStyle.Italic);
                filterButton.Text = "Showing Ongoing";
                filterByTags();
            }
            else
            {
                filterButton.Font = new Font(filterButton.Font, FontStyle.Bold);
                filterButton.Text = "Showing All";
                filterByTags();
            }
        }

        private void tagsButtons_Click(object sender, EventArgs e)
        {
            mangaList.SelectedIndex = -1;
            uniqueTags.Clear();
            tagsUsage.Clear();
            foreach (eBook book in books)
                foreach (string tag in book.Tags)
                {
                    if (tagsUsage.ContainsKey(tag))
                        tagsUsage[tag]++;
                    if (uniqueTags.Contains(tag) == false)
                    {
                        uniqueTags.Add(tag);
                        tagsUsage[tag] = 1;
                    }
                }
            booksCopy = books.ToList();
            tagsFilter filterForm = new tagsFilter();
            filterForm.ShowDialog();
            filterForm.Dispose();
            booksCopy.Clear();
            filterByTags();
        }

        private void filterByTags()
        {
            if (includedRatings.Count == 4 && includedTags.Count == 0 && excludedTags.Count == 0)
            {
                tagsButtons.Font = new Font(tagsButtons.Font, FontStyle.Bold);
                tagsButtons.Text = "Filter by Tags";
                mangaList.BeginUpdate();
                mangaList.Items.Clear();
                searchTextBoxAutomcompleteStrings.Clear();
                foreach (eBook book in books)
                {
                    if (filterToggled == true && book.Ongoing == false)
                        continue;
                    mangaList.Items.Add(book);
                    searchTextBoxAutomcompleteStrings.Add(book.Title);
                }
                mangaList.EndUpdate();
                filterTags = false;
                return;
            }
            filterTags = true;
            tagsButtons.Font = new Font(tagsButtons.Font, FontStyle.Bold | FontStyle.Italic);
            tagsButtons.Text = "Filtering by Tags";
            mangaList.BeginUpdate();
            mangaList.Items.Clear();
            searchTextBoxAutomcompleteStrings.Clear();
            foreach (eBook book in books)
            {
                if ((includedRatings.Contains(book.CotentRating) == false && book.CotentRating != String.Empty) || (filterToggled == true && book.Ongoing == false))
                    continue;
                if (inclusionMode == true)
                {
                    bool bad = false;
                    foreach (string tag in includedTags)
                        if (book.Tags.Contains(tag) == false)
                        {
                            bad = true;
                            break;
                        }
                    if (bad == true)
                        continue;
                    if (exclusionMode == true)
                    {
                        foreach (string tag in excludedTags)
                            if (book.Tags.Contains(tag) == false)
                            {
                                bad = true;
                                break;
                            }
                        if (bad == false && excludedTags.Count > 0)
                            continue;
                    }
                    else
                        if (book.Tags.Intersect(excludedTags).Any() == true)
                        continue;
                }
                else
                {
                    if (book.Tags.Intersect(includedTags).Any() == false)
                        continue;
                    if (exclusionMode == true)
                    {
                        bool bad = false;
                        foreach (string tag in excludedTags)
                            if (book.Tags.Contains(tag) == false)
                            {
                                bad = true;
                                break;
                            }
                        if (bad == false && excludedTags.Count > 0)
                            continue;
                    }
                    else
                        if (book.Tags.Intersect(excludedTags).Any() == true)
                        continue;
                }
                mangaList.Items.Add(book);
                searchTextBoxAutomcompleteStrings.Add(book.Title);
            }
            mangaList.EndUpdate();
        }

        private void mainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("Manga Library Manager.json"))
                {
                    writer.Write(JsonConvert.SerializeObject(books));
                }
            }
            catch
            {
                if (MessageBox.Show("Could not save library json!\nAre you sure you want to exit?", "Write error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    e.Cancel = true;
                if (MessageBox.Show("Would you like to get the library json yourself?", "Dump JSON", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    jsonDump = JsonConvert.SerializeObject(books);
                    dumpJSON form = new dumpJSON();
                    form.ShowDialog();
                    form.Dispose();
                }
            }
        }

        private void tagsTextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
