using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Avalonia.Media.Imaging;
using System.IO.Compression;
using Avalonia.Threading;
using static Manga_Manager.Globals;
using MangaDex_Library;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Input;
using System.Linq;

namespace Manga_Manager
{
    public partial class MainWindow : Window
    {
        private List<string> searchAutocomplete = new List<string>();

        private void ResetDescriptionPanel()
        {
            MangaDescPanel.IsVisible = false;
            TitleLabel.Text = string.Empty;
            TitleLabel.Cursor = Avalonia.Input.Cursor.Default;
            ToolTip.SetTip(TitleLabel, null);
            CoverImage.Source = null;
            DescriptionLabel.Text = string.Empty;
            StatusLabel.Text = "Status: ";
            RatingLabel.Text = "Rating: ";
            LastChapterTextBlock.Text = "Last chapter on file: ";
            LastChapterOnlineLabel.IsVisible = false;
            LastChapterOnlineLabel.Text = "Last chapter online: ";
            LastCheckedDateLabel.IsVisible = false;
            LastCheckedDateLabel.Text = "Last checked: ";
            UpdateMangaButton.IsEnabled = false;
            OpenInExplorerButton.IsEnabled = true;
            CheckOnlineButton.IsEnabled = true;
            DeleteEntryButton.IsEnabled = true;
            CheckForUpdatesCheckBox.IsEnabled = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            Startup();
        }

        private async void Startup()
        {
            if (File.Exists("Manga Library Manager.json") == false && File.Exists(".Manga Library Manager.json") == false)
            {
                JObject tempSaveJson = new JObject();
                tempSaveJson["FormatVersion"] = 4;
                tempSaveJson["Language"] = "en";
                tempSaveJson["NoWarning"] = false;

                MessageBoxCustomParams updatePopup = new MessageBoxCustomParams
                {
                    ButtonDefinitions = new List<ButtonDefinition>
                    {
                        new ButtonDefinition{ Name = "Yes" },
                        new ButtonDefinition{ Name = "No" }
                    },
                    ContentTitle = "Enable checking for updates?",
                    ContentMessage = "Would you like the program to automatically check for updates?\nYou can always change this later in the settings.",
                    CanResize = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Topmost = true,
                    SystemDecorations = SystemDecorations.None
                };
                if (await MessageBoxManager.GetMessageBoxCustom(updatePopup).ShowAsync() == "Yes")
                    tempSaveJson["CheckUpdates"] = true;
                else
                    tempSaveJson["CheckUpdates"] = false;

                tempSaveJson["HideJsonFile"] = false;
                tempSaveJson["DownloaderLastUsedFormat"] = 0;
                tempSaveJson["Library"] = JToken.FromObject(mangaList);
                try
                {
                    File.WriteAllText("Manga Library Manager.json", tempSaveJson.ToString());
                }
                catch
                {
                    await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not write the library json!\nThe program will now exit.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                    System.Environment.Exit(1);
                }
            }

            JObject saveJson;
            try
            {
                saveJson = JObject.Parse(File.ReadAllText("Manga Library Manager.json"));
            }
            catch
            {
                saveJson = JObject.Parse(File.ReadAllText(".Manga Library Manager.json"));
            }
            selectedLanguage = saveJson.SelectToken("Language").Value<string>();
            noWarning = saveJson.SelectToken("NoWarning").Value<bool>();
            checkUpdates = saveJson.SelectToken("CheckUpdates").Value<bool>();
            if (saveJson.SelectToken("FormatVersion").Value<int>() == 4)
            {
                hideJsonFile = saveJson.SelectToken("HideJsonFile").Value<bool>();
                downloaderLastUsedFormat = saveJson.SelectToken("DownloaderLastUsedFormat").Value<int>();
                mangaList = saveJson.SelectToken("Library").ToObject<List<Manga>>();
            }
            #region Legacy JSON Formats
            else if (saveJson.SelectToken("FormatVersion").Value<int>() == 3)
            {
                hideJsonFile = false;
                downloaderLastUsedFormat = 0;
                foreach (JToken manga in saveJson.SelectToken("Library"))
                {
                    mangaList.Add(new Manga
                    {
                        Title = manga.SelectToken("Title").Value<string>(),
                        Description = string.Empty,
                        Path = manga.SelectToken("Path").Value<string>(),
                        FileLastChapter = manga.SelectToken("LastChapter").Value<decimal>(),
                        OnlineLastChapter = 0M,
                        LastChecked = new DateOnly(69, 1, 1),
                        OngoingStatus = "Unknown",
                        CheckInBulk = manga.SelectToken("Ongoing").Value<bool>(),
                        ID = manga.SelectToken("Link").Value<string>(),
                        ContentRating = manga.SelectToken("ContentRating").Value<string>(),
                        Tags = manga.SelectToken("Tags").ToObject<List<string>>()
                    });
                    string link = manga.SelectToken("Link").Value<string>();
                    bool bad = false;
                    try
                    {
                        if (link == String.Empty)
                            throw new Exception();
                        if (link.Split('/')[2] != "mangadex.org")
                            throw new Exception();
                        if (link.Split('/')[3] != "title")
                            throw new Exception();
                        if (link.Split('/')[4] == null || Uri.TryCreate(link, UriKind.Absolute, out Uri uriResult) == false || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                            throw new Exception();
                    }
                    catch { bad = true; }
                    if (bad == false)
                        mangaList[mangaList.Count - 1].ID = link.Split('/')[4];
                }
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard("Your library JSON is too old", "Please download and launch an older version of the program first.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowAsync();
                System.Environment.Exit(0);
            }
            #endregion

            //TODO check for updates
            if (checkUpdates == true)
            {

            }

            foreach (Manga manga in mangaList)
            {
                foreach (string tag in manga.Tags)
                    if (tagsUsage.ContainsKey(tag))
                        tagsUsage[tag]++;
                    else
                        tagsUsage[tag] = 1;
                searchAutocomplete.Add(manga.Title);
                DisplayAdd(manga.Title, manga.FileLastChapter, manga.FileLastChapter < manga.OnlineLastChapter);
            }
            SearchBox.ItemsSource = searchAutocomplete;
            tagsUsage = tagsUsage.OrderByDescending(pair => pair.Value).ToDictionary();
        }

        private async void MainDisplayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetDescriptionPanel();
            int index = MainDisplayList.SelectedIndex;
            if (index == -1)
                return;
            Manga currentManga = mangaList[index];
            bool notFound = false;
            if (File.Exists(currentManga.Path) == false)
            {
                notFound = true;
                if (noWarning == false)
                    if (await MessageBoxManager.GetMessageBoxStandard("Manga file not found", "Selected manga file no longer exists!\nWould you like to remove it from the list?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Error).ShowAsync() == ButtonResult.Yes)
                    {
                        DeleteEntryButton_Clicked(sender, null);
                        return;
                    }
                OpenInExplorerButton.IsEnabled = false;
                DeleteEntryButton.IsEnabled = false;
            }

            TitleLabel.Text = currentManga.Title;
            DescriptionLabel.Text = currentManga.Description;
            StatusLabel.Text += currentManga.OngoingStatus;
            RatingLabel.Text += currentManga.ContentRating;
            LastChapterTextBlock.Text += currentManga.FileLastChapter.ToString();
            LastChapterOnlineLabel.Text += currentManga.OnlineLastChapter.ToString();
            LastCheckedDateLabel.Text += currentManga.LastChecked.ToShortDateString();
            CheckForUpdatesCheckBox.IsChecked = currentManga.CheckInBulk;
            if (currentManga.LastChecked != new DateOnly(69, 1, 1))
            {
                LastChapterOnlineLabel.IsVisible = true;
                LastCheckedDateLabel.IsVisible = true;
                if (currentManga.OnlineLastChapter - currentManga.FileLastChapter > 0)
                    UpdateMangaButton.IsEnabled = true;
            }

            if (currentManga.ID != string.Empty)
            {
                TitleLabel.Cursor = Avalonia.Input.Cursor.Parse("Hand");
                ToolTip.SetTip(TitleLabel, "Click to open the manga link in your browser");
            }
            else
            {
                CheckOnlineButton.IsEnabled = false;
                CheckForUpdatesCheckBox.IsEnabled = false;
            }

            if (notFound == false)
            {
                _ = Task.Run(() =>
                {
                    using ZipArchive manga = ZipFile.OpenRead(currentManga.Path);
                    foreach (ZipArchiveEntry entry in manga.Entries)
                        if (entry.Name == "cover.jpg" || entry.Name == "cover.jpeg" || entry.Name == "cover.png" || entry.Name == "cover.webp")
                        {
                            Bitmap b = new Bitmap(entry.Open());
                            Dispatcher.UIThread.Post(() => { CoverImage.Source = b; });
                            return;
                        }
                });
            }
            MangaDescPanel.IsVisible = true;
        }

        private void DownloadMangaButton_Clicked(object sender, RoutedEventArgs args)
        {

        }

        private void AddMangaFileButton_Clicked(object sender, RoutedEventArgs args)
        {

        }

        private async void CheckUpdatesButton_Clicked(object sender, RoutedEventArgs args)
        {
            MainDisplayList.SelectedIndex = -1;
            BulkUpdateCheck bulkUpdateCheck = new BulkUpdateCheck();
            await bulkUpdateCheck.ShowDialog(this);
            for (int i = 0; i < mangaList.Count; i++)
                DisplaySetNewChaptersAvailable(i, mangaList[i].OnlineLastChapter - mangaList[i].FileLastChapter > 0);
        }

        private void SortAndFilterButton_Clicked(object sender, RoutedEventArgs args)
        {

        }

        private void SettingsButton_Clicked(object sender, RoutedEventArgs args)
        {
            Settings settings = new Settings();
            settings.ShowDialog(this);
        }

        #region SearchBox Logic
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == string.Empty)
                return;
            for (int i = 0; i < mangaList.Count; i++)
                if (mangaList[i].Title.ToLower().Contains(SearchBox.Text.ToLower()))
                {
                    MainDisplayList.SelectedIndex = i;
                    break;
                }
        }

        private void SearchBox_DropDownClosed(object sender, EventArgs e)
        {
            SearchBox_LostFocus(sender, null);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchBox_LostFocus(sender, null);
        }
        #endregion

        private void TitleLabel_Tapped(object sender, TappedEventArgs e)
        {
            if (mangaList[MainDisplayList.SelectedIndex].ID != string.Empty)
                OpenLink("https://mangadex.org/title/" + mangaList[MainDisplayList.SelectedIndex].ID);
        }

        private void OpenInExplorerButton_Clicked(object sender, RoutedEventArgs args)
        {
            string path = mangaList[MainDisplayList.SelectedIndex].Path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start("explorer.exe", "/select," + path);
            else
            {
                if (System.IO.Path.IsPathRooted(path) == true)
                    OpenLink(path);
                else
                    OpenLink(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.ProcessPath), System.IO.Path.GetDirectoryName(path)));
            }
        }

        private async void CheckOnlineButton_Clicked(object sender, RoutedEventArgs args)
        {
            Manga currentManga = mangaList[MainDisplayList.SelectedIndex];

            MDLParameters.MangaID = currentManga.ID;
            MDLGetData.GetLastChapter();
            if (apiError == true)
            {
                apiError = false;
                return;
            }
            decimal onlineChapter = MDLGetData.GetLastChapter();
            currentManga.OnlineLastChapter = onlineChapter;
            currentManga.LastChecked = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            LastChapterOnlineLabel.Text = $"Last chapter online: {onlineChapter.ToString()}";
            LastCheckedDateLabel.Text = $"Last checked: {currentManga.LastChecked.ToShortDateString()}";
            LastChapterOnlineLabel.IsVisible = true;
            LastCheckedDateLabel.IsVisible = true;
            if (onlineChapter > currentManga.FileLastChapter)
            {
                DisplaySetNewChaptersAvailable(MainDisplayList.SelectedIndex, true);
                UpdateMangaButton.IsEnabled = true;
            }
            else if (onlineChapter == currentManga.FileLastChapter)
                DisplaySetNewChaptersAvailable(MainDisplayList.SelectedIndex, false);
            else if (onlineChapter < currentManga.FileLastChapter)
            {
                DisplaySetNewChaptersAvailable(MainDisplayList.SelectedIndex, false);
                await MessageBoxManager.GetMessageBoxStandard("Invalid data", "Online chapter number is behind! Please double-check the manga ID and last chapter in the file.", ButtonEnum.Ok).ShowAsync();
            }
        }

        private async void DeleteEntryButton_Clicked(object sender, RoutedEventArgs args)
        {
            int index = MainDisplayList.SelectedIndex;
            if (sender == DeleteEntryButton && File.Exists(mangaList[index].Path))
            {
                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Deletion confirmation", "Would you like to also delete the file?\nThe file would be deleted, not moved to the Recycle Bin!", ButtonEnum.YesNoCancel).ShowAsync();
                if (result == ButtonResult.Cancel)
                    return;
                else if (result == ButtonResult.Yes)
                {
                    try
                    {
                        File.Delete(mangaList[index].Path);
                    }
                    catch
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the file!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                    }
                }
                searchAutocomplete.Remove(mangaList[index].Title);
                SearchBox.ItemsSource = searchAutocomplete;
                displayChapters.RemoveAt(index);
                displayTitles.RemoveAt(index);
                displayPanels.RemoveAt(index);
                mangaList.RemoveAt(index);
                MainDisplayList.Items.RemoveAt(index);
            }
        }

        private void UpdateMangaButton_Clicked(object sender, RoutedEventArgs args)
        {

        }

        private void CheckForUpdatesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mangaList[MainDisplayList.SelectedIndex].CheckInBulk = (bool)CheckForUpdatesCheckBox.IsChecked;
        }

        private void EditMetadataButton_Clicked(object sender, RoutedEventArgs args)
        {

        }

        private List<TextBlock> displayTitles = new List<TextBlock>(), displayChapters = new List<TextBlock>();
        private List<StackPanel> displayPanels = new List<StackPanel>();
        private void DisplayAdd(string title, decimal chapterOnFile, bool ahead)
        {
            displayTitles.Add(new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeight.Bold,
                FontSize = 12,
                Text = title,
                Margin = new Avalonia.Thickness(2)
            });

            displayChapters.Add(new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.LightGray),
                FontSize = 9,
                Margin = new Avalonia.Thickness(0),
                Text = string.Empty
            });
            if (chapterOnFile != 0)
                displayChapters[displayChapters.Count - 1].Text = "At chapter " + chapterOnFile.ToString();

            displayPanels.Add(new StackPanel
            {
                Children = { displayTitles[displayTitles.Count - 1], displayChapters[displayChapters.Count - 1] }
            });

            DisplaySetNewChaptersAvailable(displayPanels.Count - 1, ahead);

            MainDisplayList.Items.Add(displayPanels[displayPanels.Count - 1]);
        }

        private void DisplaySetNewChaptersAvailable(int index, bool available)
        {
            if (available == true)
            {
                if (displayPanels[index].Children.Count == 3)
                    return;
                displayPanels[index].Children.Add(new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.LightGray),
                    FontSize = 9,
                    Margin = new Avalonia.Thickness(0),
                    Text = "New chapters available!"
                });
            }
            else
                try
                {
                    displayPanels[index].Children.RemoveAt(2);
                }
                catch { }
        }

        private bool bypassSaving = false;
        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            JObject saveJson = new JObject();
            saveJson["FormatVersion"] = 4;
            saveJson["Language"] = selectedLanguage;
            saveJson["NoWarning"] = noWarning;
            saveJson["CheckUpdates"] = checkUpdates;
            saveJson["HideJsonFile"] = hideJsonFile;
            saveJson["DownloaderLastUsedFormat"] = downloaderLastUsedFormat;
            saveJson["Library"] = JToken.FromObject(mangaList);
            if (bypassSaving == true)
            {
                base.OnClosing(e);
                return;
            }
            try
            {
                if (hideJsonFile == true)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        File.SetAttributes(".Manga Library Manager.json", File.GetAttributes(".Manga Library Manager.json") & ~FileAttributes.Hidden);
                    File.WriteAllText(".Manga Library Manager.json", saveJson.ToString());
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        File.SetAttributes(".Manga Library Manager.json", File.GetAttributes(".Manga Library Manager.json") | FileAttributes.Hidden);
                }
                else
                    File.WriteAllText("Manga Library Manager.json", saveJson.ToString());
            }
            catch
            {
                e.Cancel = true;
                await Clipboard.SetTextAsync(saveJson.ToString());
                if (await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not save the library file, your changes will not be saved!\nThe contents have been copied to your clipboard, you can paste them somewhere yourself.\nAre you sure you want to exit?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.Yes)
                {
                    bypassSaving = true;
                    this.Close();
                }
            }

            base.OnClosing(e);
        }
    }
}