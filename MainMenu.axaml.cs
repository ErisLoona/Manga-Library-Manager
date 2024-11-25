using Avalonia.Controls;
using Avalonia.Controls.Documents;
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
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using Avalonia.Platform.Storage;

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
                    if (ValidateLink(link) == true)
                        mangaList[mangaList.Count - 1].ID = link.Split('/')[4];
                }
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard("Library JSON too old", "Your library JSON is too old.\nPlease download and launch an older version of the program first.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowAsync();
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
            SearchBox.ItemsSource = searchAutocomplete.ToArray();
            tagsUsage = tagsUsage.OrderByDescending(pair => pair.Value).ToDictionary();
        }

        private async void MainDisplayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetDescriptionPanel();
            if (MainDisplayList.SelectedIndex == -1)
                return;
            Manga currentManga = null;
            foreach (Manga manga in mangaList)
                if (manga.Title == displayTitles[MainDisplayList.SelectedIndex].Text)
                {
                    currentManga = manga;
                    break;
                }
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
                    if (Path.GetExtension(currentManga.Path).ToLower() == ".epub")
                    {
                        foreach (ZipArchiveEntry entry in manga.Entries)
                            if (entry.Name.ToLower() == "cover.jpg" || entry.Name.ToLower() == "cover.jpeg" || entry.Name.ToLower() == "cover.png" || entry.Name.ToLower() == "cover.webp")
                            {
                                MemoryStream stream = new MemoryStream();
                                entry.Open().CopyTo(stream);
                                stream.Seek(0, SeekOrigin.Begin);
                                Dispatcher.UIThread.Post(() => { CoverImage.Source = new Bitmap(stream); });
                                return;
                            }
                    }
                    else if (Path.GetExtension(currentManga.Path).ToLower() == ".cbz")
                    {
                        MemoryStream stream = new MemoryStream();
                        manga.Entries.First().Open().CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        Dispatcher.UIThread.Post(() => { CoverImage.Source = new Bitmap(stream); });
                        return;
                    }
                });
            }
            MangaDescPanel.IsVisible = true;
        }

        private async void DownloadMangaButton_Clicked(object sender, RoutedEventArgs args)
        {
            passIndex = MainDisplayList.SelectedIndex;
            MainDisplayList.SelectedIndex = -1;
            Downloader downloader = new Downloader();
            downloader.openedByDownloadUpdatesButton = false;
            await downloader.ShowDialog(this);
            Filter();
            if (passIndex != -1)
                for (int i = 0; i < displayTitles.Count; i++)
                    if (displayTitles[i].Text == mangaList[passIndex].Title)
                    {
                        MainDisplayList.SelectedIndex = i;
                        break;
                    }
        }

        private async void AddMangaFileButton_Clicked(object sender, RoutedEventArgs args)
        {
            List<string> mangaErrors = new List<string>();
            int count = 0;
            foreach (var file in await StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions()
            {
                Title = "Select all Manga files you want to add",
                AllowMultiple = true,
                FileTypeFilter = new[] { new FilePickerFileType("EPUB or CBZ Files") { Patterns = new[] { "*.epub", "*.cbz" } } }
            }))
            {
                string result = AddManga(file.TryGetLocalPath());
                if (result == null)
                    mangaErrors.Add(file.TryGetLocalPath());
                else if (result != string.Empty)
                    count++;
            }
            if (mangaErrors.Count > 0)
            {
                string message = $"Some files were not added:\nThere were {count} duplicates and {mangaErrors.Count} could not be read:\n";
                foreach (string error in mangaErrors)
                    message += error + '\n';
                await MessageBoxManager.GetMessageBoxStandard("Read error", message, ButtonEnum.Ok).ShowAsync();
            }
        }

        private static readonly Regex chapterRegex = new Regex("Ch\\.[0-9.]+");
        private string AddManga(string path)
        {
            Manga tempManga = new Manga();
            XmlDocument doc = new XmlDocument();
            MemoryStream stream = new MemoryStream();
            string title = string.Empty, desc = string.Empty, link = string.Empty;
            if (Path.GetExtension(path).ToLower() == ".epub")
            {
                try
                {
                    using (ZipArchive epub = ZipFile.OpenRead(path))
                    {
                        foreach (ZipArchiveEntry entry in epub.Entries)
                            if (entry.Name.ToLower() == "content.opf")
                            {
                                entry.Open().CopyTo(stream);
                                break;
                            }
                    }
                    stream.Position = 0;
                    doc.Load(stream);
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                    title = doc.DocumentElement.SelectSingleNode("//dc:title", nsmgr).InnerText;
                    desc = doc.DocumentElement.SelectSingleNode("//dc:description", nsmgr).InnerText;
                    link = doc.DocumentElement.SelectSingleNode("//dc:identifier", nsmgr).InnerText;
                }
                catch
                {
                    return null;
                }
            }
            else
                title = Path.GetFileNameWithoutExtension(path);
            foreach (Manga manga in mangaList)
                if (manga.Title == title)
                    return title;

            tempManga.Title = title;
            if (path.Contains(Path.GetDirectoryName(Environment.ProcessPath)))
                tempManga.Path = path.Substring(Path.GetDirectoryName(Environment.ProcessPath).Length);
            else
                tempManga.Path = path;
            if (tempManga.Path[0] == Path.DirectorySeparatorChar)
                tempManga.Path = tempManga.Path.Substring(1);
            try
            {
                MatchCollection chapters = chapterRegex.Matches(desc);
                List<decimal> tempChapters = new List<decimal>();
                foreach (Match match in chapters)
                    tempChapters.Add(Convert.ToDecimal(match.Value.Substring(3), new CultureInfo("en-US")));
                tempManga.FileLastChapter = tempChapters.Max();
            } catch { }
            if (ValidateLink(link) == true)
                tempManga.ID = link.Split('/')[4];

            mangaList.Add(tempManga);
            if (Filters.Active() == false)
            {
                DisplayAdd(title, tempManga.FileLastChapter, false);
                searchAutocomplete.Add(title);
                SearchBox.ItemsSource = searchAutocomplete.ToArray();
            }
            else
                Filter();
            return string.Empty;
        }

        private async void CheckUpdatesButton_Clicked(object sender, RoutedEventArgs args)
        {
            MainDisplayList.SelectedIndex = -1;
            BulkUpdateCheck bulkUpdateCheck = new BulkUpdateCheck();
            await bulkUpdateCheck.ShowDialog(this);
            Filter();
        }

        private async void FilterButton_Clicked(object sender, RoutedEventArgs args)
        {
            passIndex = MainDisplayList.SelectedIndex;
            MainDisplayList.SelectedIndex = -1;
            Filtering filtering = new Filtering();
            await filtering.ShowDialog(this);
            Filter();
            if (passIndex != -1)
                for (int i = 0; i < displayTitles.Count; i++)
                    if (displayTitles[i].Text == mangaList[passIndex].Title)
                    {
                        MainDisplayList.SelectedIndex = i;
                        break;
                    }
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
            for (int i = 0; i < displayTitles.Count; i++)
                if (displayTitles[i].Text.ToLower().Contains(SearchBox.Text.ToLower()))
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
            Manga currentManga = null;
            foreach (Manga manga in mangaList)
                if (manga.Title == displayTitles[MainDisplayList.SelectedIndex].Text)
                {
                    currentManga = manga;
                    break;
                }
            if (currentManga.ID != string.Empty)
                OpenLink("https://mangadex.org/title/" + currentManga.ID);
        }

        private void OpenInExplorerButton_Clicked(object sender, RoutedEventArgs args)
        {
            Manga currentManga = null;
            foreach (Manga manga in mangaList)
                if (manga.Title == displayTitles[MainDisplayList.SelectedIndex].Text)
                {
                    currentManga = manga;
                    break;
                }
            string path = currentManga.Path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start("explorer.exe", "/select," + path);
            else
            {
                if (Path.IsPathRooted(path) == true)
                    OpenLink(path);
                else
                    OpenLink(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), Path.GetDirectoryName(path)));
            }
        }

        private async void CheckOnlineButton_Clicked(object sender, RoutedEventArgs args)
        {
            Manga currentManga = null;
            foreach (Manga manga in mangaList)
                if (manga.Title == displayTitles[MainDisplayList.SelectedIndex].Text)
                {
                    currentManga = manga;
                    break;
                }

            MDLParameters.MangaID = currentManga.ID;
            MDLGetData.GetLastChapter();
            if (apiError == true)
            {
                await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
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
            }
            else
            {
                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Deletion confirmation", "Are you sure you want to delete the entry?", ButtonEnum.YesNo).ShowAsync();
                if (result == ButtonResult.No)
                    return;
            }
            searchAutocomplete.Remove(mangaList[index].Title);
            SearchBox.ItemsSource = searchAutocomplete.ToArray();
            displayChapters.RemoveAt(index);
            displayTitles.RemoveAt(index);
            displayPanels.RemoveAt(index);
            mangaList.RemoveAt(index);
            MainDisplayList.Items.RemoveAt(index);
        }

        private async void UpdateMangaButton_Clicked(object sender, RoutedEventArgs args)
        {
            passIndex = MainDisplayList.SelectedIndex;
            MainDisplayList.SelectedIndex = -1;
            Downloader downloader = new Downloader();
            downloader.openedByDownloadUpdatesButton = true;
            await downloader.ShowDialog(this);
            Filter();
            for (int i = 0; i < displayTitles.Count; i++)
                if (displayTitles[i].Text == mangaList[passIndex].Title)
                {
                    MainDisplayList.SelectedIndex = i;
                    break;
                }
        }

        private void CheckForUpdatesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Manga currentManga = null;
            foreach (Manga manga in mangaList)
                if (manga.Title == displayTitles[MainDisplayList.SelectedIndex].Text)
                {
                    currentManga = manga;
                    break;
                }
            currentManga.CheckInBulk = (bool)CheckForUpdatesCheckBox.IsChecked;
        }

        private async void EditMetadataButton_Clicked(object sender, RoutedEventArgs args)
        {
            for (int i = 0; i < mangaList.Count; i++)
                if (mangaList[i].Title == displayTitles[MainDisplayList.SelectedIndex].Text)
                {
                    passIndex = i;
                    break;
                }
            MainDisplayList.SelectedIndex = -1;
            EditMetadata editMetadata = new EditMetadata();
            await editMetadata.ShowDialog(this);
            tagsUsage.Clear();
            foreach (Manga manga in mangaList)
                foreach (string tag in manga.Tags)
                    if (tagsUsage.ContainsKey(tag))
                        tagsUsage[tag]++;
                    else
                        tagsUsage[tag] = 1;
            tagsUsage = tagsUsage.OrderByDescending(pair => pair.Value).ToDictionary();

            if (Filters.Active() == true)
                Filter();
            for (int i = 0; i < displayTitles.Count; i++)
                if (displayTitles[i].Text == mangaList[passIndex].Title)
                {
                    MainDisplayList.SelectedIndex = i;
                    break;
                }
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

        private void Filter()
        {
            displayTitles.Clear();
            displayChapters.Clear();
            displayPanels.Clear();
            MainDisplayList.Items.Clear();
            searchAutocomplete.Clear();
            if (Filters.Active() == false)
            {
                FilterButton.Content = "Filter";
                FilterButton.FontStyle = FontStyle.Normal;
                foreach (Manga manga in mangaList)
                {
                    searchAutocomplete.Add(manga.Title);
                    DisplayAdd(manga.Title, manga.FileLastChapter, manga.FileLastChapter < manga.OnlineLastChapter);
                }
                SearchBox.ItemsSource = searchAutocomplete;
                return;
            }

            FilterButton.Content = "Filtering";
            FilterButton.FontStyle = FontStyle.Italic;
            foreach (Manga manga in mangaList)
            {
                if ((Filters.IncludedContentRatings.Contains(manga.ContentRating) == false && manga.ContentRating != "Unknown") || (Filters.OnlyShowMangasWithUpdates == true && manga.FileLastChapter >= manga.OnlineLastChapter))
                    continue;

                if (Filters.InclusionModeIsAnd == true)
                {
                    bool bad = false;
                    foreach (string tag in Filters.IncludedTags)
                        if (manga.Tags.Contains(tag) == false)
                        {
                            bad = true;
                            break;
                        }
                    if (bad == true)
                        continue;
                    if (Filters.ExclusionModeIsAnd == true)
                    {
                        foreach (string tag in Filters.ExcludedTags)
                            if (manga.Tags.Contains(tag) == false)
                            {
                                bad = true;
                                break;
                            }
                        if (bad == false && Filters.ExcludedTags.Count > 0)
                            continue;
                    }
                    else if (manga.Tags.Intersect(Filters.ExcludedTags).Any() == true)
                        continue;
                }
                else
                {
                    if (manga.Tags.Intersect(Filters.IncludedTags).Any() == false && Filters.IncludedTags.Count != 0)
                        continue;
                    if (Filters.ExclusionModeIsAnd == true)
                    {
                        bool bad = false;
                        foreach (string tag in Filters.ExcludedTags)
                            if (manga.Tags.Contains(tag) == false)
                            {
                                bad = true;
                                break;
                            }
                        if (bad == false && Filters.ExcludedTags.Count > 0)
                            continue;
                    }
                    else if (manga.Tags.Intersect(Filters.ExcludedTags).Any() == true)
                        continue;
                }
                searchAutocomplete.Add(manga.Title);
                DisplayAdd(manga.Title, manga.FileLastChapter, manga.FileLastChapter < manga.OnlineLastChapter);
            }
            SearchBox.ItemsSource = searchAutocomplete.ToArray();
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
                if (await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not save the library file, your changes will not be saved!\nThe contents have been copied to your clipboard, you can paste them somewhere yourself.\nAre you sure you want to exit?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Error).ShowAsync() == ButtonResult.Yes)
                {
                    bypassSaving = true;
                    this.Close();
                }
            }

            base.OnClosing(e);
        }
    }
}