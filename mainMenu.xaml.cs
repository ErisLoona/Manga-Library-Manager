using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Manga_Library_Manager
{
    public partial class MainPage : ContentPage
    {
        public static HttpClient client = new HttpClient();
        public static Stopwatch generalApi = new Stopwatch(), atHome = new Stopwatch();
        public static int atHomeCalls = 0, generalApiCalls = 0;
        public static TimeSpan apiSpan;

        public class eBook
        {
            public string Title;
            public string Path;
            public decimal LastChapter;
            public bool Ongoing;
            public string Link;
            public string ContentRating;
            public List<string> Tags;

            public override string ToString()
            {
                return Title;
            }
        }

        #region Support code to make MAUI work for me

        public MainPage()
        {
            InitializeComponent();
        }

        public void ButtonPressed(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
                ((Button)sender).Background = Color.FromRgb(188, 152, 167);
            else if (sender.GetType() == typeof(ImageButton))
                ((ImageButton)sender).Background = Color.FromRgb(188, 152, 167);
        }

        public void ButtonReleased(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
                ((Button)sender).Background = Color.FromRgb(248, 200, 220);
            else if (sender.GetType() == typeof(ImageButton))
                ((ImageButton)sender).Background = Color.FromRgb(248, 200, 220);
        }

        #endregion

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

        public bool CallAPI(string mangaID, bool feed)
        {
            return false;
        }

        private void mainMenu_Loaded(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Manga Library Manager for Windows by (github) ErisLoona");
        }

        private void editTagsButton_Click(object sender, EventArgs e)
        {

        }

        private void updateMangaButton_Click(object sender, EventArgs e)
        {

        }

        private void deleteEntryButton_Click(object sender, EventArgs e)
        {

        }

        private void checkOnlineButton_Click(object sender, EventArgs e)
        {

        }

        private void openInExplorerButton_Click(object sender, EventArgs e)
        {

        }

        private void linkTextBox_TextChanged(object sender, EventArgs e)
        { 

        }

        private void resetButton_Click(object sender, EventArgs e)
        {

        }

        private void ongoingCheckbox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void mangaList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void settingsButton_Click(object sender, EventArgs e)
        {

        }

        private void tagsButtons_Click(object sender, EventArgs e)
        {

        }

        private void filterButton_Click(object sender, EventArgs e)
        {

        }

        private void checkAllOnlineButton_Click(object sender, EventArgs e)
        {

        }

        private void downloadMangaButton_Click(object sender, EventArgs e)
        {

        }

        private void scanButton_Click(object sender, EventArgs e)
        {

        }

        private void addMangaButton_Click(object sender, EventArgs e)
        {

        }

        private void mainMenu_Closing(object sender, EventArgs e)
        {

        }

        private void lastChapterNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
