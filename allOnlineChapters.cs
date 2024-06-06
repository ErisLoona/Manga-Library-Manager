using Flurl;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Manga_Library_Manager
{
    public partial class allOnlineChapters : Form
    {
        private Dictionary<string, string> books = new Dictionary<string, string>();
        public static Stopwatch generalApi = new Stopwatch();
        public static TimeSpan apiSpan;
        public static int generalApiCalls = 0;

        public allOnlineChapters()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void nameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusList.SelectedIndex = nameList.SelectedIndex;
        }

        private void statusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            nameList.SelectedIndex = statusList.SelectedIndex;
        }

        private void displayList_MouseDown(object sender, MouseEventArgs e)
        {
            ((ListBox)sender).SelectedIndex = ((ListBox)sender).IndexFromPoint(e.Location);
        }

        private void allOnlineChapters_Load(object sender, EventArgs e)
        {
            foreach (mainMenu.eBook book in mainMenu.booksCopy)
                books.Add(book.Title, book.Link);
            loadingBar.Maximum = books.Count;
            thread.RunWorkerAsync(argument: books);
        }

        private void thread_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, string> booksCopy = e.Argument as Dictionary<string, string>;
            Dictionary<string, int> chapters = new Dictionary<string, int>();
            Stopwatch sw = new Stopwatch();
            TimeSpan span;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
                int counter = 0;
                sw.Start();
                foreach (KeyValuePair<string, string> book in booksCopy)
                {
                    if (counter == 5)
                    {
                        sw.Stop();
                        span = sw.Elapsed;
                        if (span.Milliseconds < 1000)
                            Thread.Sleep(1000 - span.Milliseconds);
                        counter = 0;
                        sw.Reset();
                        sw.Start();
                    }
                    if (((BackgroundWorker)sender).CancellationPending == true)
                    {
                        e.Cancel = true;
                        sw.Reset();
                        break;
                    }
                    List<string> extraPages = new List<string>();
                    JObject apiResult;
                    try
                    {
                        string mangaID = book.Value.Split('/')[4];
                        Task<string> task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed".SetQueryParam("translatedLanguage[]", mainMenu.selectedLanguage).AppendQueryParam("limit", 100).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" })));
                        apiResult = JObject.Parse(task.Result);
                        calledAPI();
                        int apiTotal = apiResult.SelectToken("total").Value<int>();
                        if (apiTotal > 100)
                        {
                            int passes = apiTotal / 100;
                            while (passes > 0)
                            {
                                task = client.GetStringAsync(new Uri("https://api.mangadex.org/manga/" + mangaID + "/feed".SetQueryParam("translatedLanguage[]", mainMenu.selectedLanguage).AppendQueryParam("limit", 100).AppendQueryParam("offset", Convert.ToString(passes * 100)).AppendQueryParam("contentRating[]", new[] { "safe", "suggestive", "erotica", "pornographic" })));
                                extraPages.Add(task.Result);
                                calledAPI();
                                passes--;
                                counter++;
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
                        chapters[book.Key] = Convert.ToInt32(Math.Ceiling(tempChapters.Max() - mainMenu.booksCopy[books.Keys.ToList().IndexOf(book.Key)].LastChapter));
                    }
                    catch
                    {
                        chapters[book.Key] = Int32.MinValue;
                    }
                    ((BackgroundWorker)sender).ReportProgress(1);
                    counter++;
                }
            }
            e.Result = chapters;
        }

        private void thread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            loadingBar.PerformStep();
        }

        private void thread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            doneButton.Text = "Close";
            //loadingBar.Visible = false;
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                result = e.Result as Dictionary<string, int>;
                if (result == null || result.Count == 0)
                    throw new Exception();
            }
            catch
            {
                return;
            }
            nameList.BeginUpdate();
            statusList.BeginUpdate();
            foreach (KeyValuePair<string, Int32> chapter in result.OrderByDescending(key => key.Value))
            {
                nameList.Items.Add(chapter.Key);
                if (chapter.Value == 0)
                    statusList.Items.Add("-> Up to date!");
                else if (chapter.Value == 1)
                    statusList.Items.Add("-> 1 chapter ahead.");
                else if (chapter.Value == Int32.MinValue)
                    statusList.Items.Add("-> Error!");
                else if (chapter.Value < 0)
                    statusList.Items.Add("-> Please check book link!");
                else
                    statusList.Items.Add("-> " + Convert.ToString(chapter.Value) + " chapters ahead.");
            }
            nameList.EndUpdate();
            statusList.EndUpdate();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void allOnlineChapters_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread.IsBusy == true)
                if (MessageBox.Show("This will cancel the process.\nAre you sure you wanna stop it?", "Process still running", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                    thread.CancelAsync();
        }

        public void calledAPI()
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
}