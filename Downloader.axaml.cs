using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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
using System.Linq;
using System.Xml;
using Avalonia.Platform.Storage;
using System.Threading;
using SkiaSharp;

namespace Manga_Manager;

public partial class Downloader : Window
{
    private class MimicCheckBox
    {
        public string Content = string.Empty;
        public bool Checked = false;
    }

    private class MimicProgressBar
    {
        public int Maximum = 0;
        public int Value = 0;
    }

    private class QueuedManga
    {
        public List<string> Titles = new List<string>();
        public int SelectedTitleIndex = -1;
        public int Format = 0;  // 0 = EPUB, 1 = CBZ
        public string SavePath = Path.GetDirectoryName(Environment.ProcessPath);
        public bool UpdateCover = true;
        public bool OriginalQuality = true;
        public List<MimicCheckBox> Chapters = new List<MimicCheckBox>();
        public List<MimicProgressBar> ProgressBars = new List<MimicProgressBar>();
        public List<string> ChapterIDs = new List<string>();
        public List<int> ChapterPages = new List<int>();
        public Manga TempManga = new Manga();
        public bool Updating = false;
    }

    internal static List<string> addedMangas = new List<string>(); // This is to not add these to "Add from library"
    private List<QueuedManga> mangaQueue = new List<QueuedManga>();
    private List<TextBlock> mangaQueueStatuses = new List<TextBlock>();

    public Downloader()
    {
        InitializeComponent();


        // More Avalonia weirdness, same issue as in Filtering, I think setting their states in the XAML triggers these methods before the QueueListBox "exists", so it's throwing the null reference error
        UpdateCoverCheckBox.IsCheckedChanged += UpdateCoverCheckBox_Checked;
        QualityComboBox.SelectionChanged += QualityComboBox_SelectionChanged;

        FormatComboBox.SelectedIndex = downloaderLastUsedFormat;
        if (MainWindow.openedByDownloadUpdatesButton == true)
        {
            MainWindow.openedByDownloadUpdatesButton = false;
            AddMangaToQueue(mangaList[passIndex].ID, true);
            QueueListBox.SelectedIndex = 0;
        }    
    }

    private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        downloaderLastUsedFormat = FormatComboBox.SelectedIndex;
        if (QueueListBox.SelectedIndex != -1)
            mangaQueue[QueueListBox.SelectedIndex].Format = FormatComboBox.SelectedIndex;
    }

    private void TitleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (QueueListBox.SelectedIndex != -1)
            mangaQueue[QueueListBox.SelectedIndex].SelectedTitleIndex = TitleComboBox.SelectedIndex;
    }

    private async void SavePathButton_Clicked(object sender, RoutedEventArgs args)
    {
        try
        {
            mangaQueue[QueueListBox.SelectedIndex].SavePath = (await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Select where to save the Manga"
            }))[0].TryGetLocalPath();
            ToolTip.SetTip(SavePathButton, mangaQueue[QueueListBox.SelectedIndex].SavePath);
        }
        catch { }
    }

    private void UpdateCoverCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        mangaQueue[QueueListBox.SelectedIndex].UpdateCover = (bool)UpdateCoverCheckBox.IsChecked;
    }

    private void QualityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (QueueListBox.SelectedIndex != -1)
            mangaQueue[QueueListBox.SelectedIndex].OriginalQuality = QualityComboBox.SelectedIndex == 0;
    }

    private async void AddFromLinkButton_Clicked(object sender, RoutedEventArgs args)
    {
        QueueListBox.SelectedIndex = -1;
        Downloader_AddFromLink downloader_AddFromLink = new Downloader_AddFromLink();
        string result = await downloader_AddFromLink.ShowDialog<string>(this);
        if (ValidateLink(result) == true)
        {
            StatusTextBox.Text = "Getting manga info...";
            if (addedMangas.Contains(result.Split('/')[4]) == true)
            {
                await MessageBoxManager.GetMessageBoxStandard("Manga already added", "This manga is already in queue!").ShowAsync();
                StatusTextBox.Text = "Add mangas to the queue!";
                return;
            }
            foreach (Manga manga in mangaList)
                if(manga.ID == result.Split('/')[4])
                {
                    if (await MessageBoxManager.GetMessageBoxStandard("Manga already exists", "This manga is already in the library!\nWould you like to update it instead?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
                    {
                        StatusTextBox.Text = "Add mangas to the queue!";
                        return;
                    }
                    passIndex = mangaList.IndexOf(manga);
                    AddMangaToQueue(manga.ID, true);
                    StatusTextBox.Text = "Add mangas to the queue!";
                    return;
                }
            AddMangaToQueue(result.Split('/')[4], false);
            StatusTextBox.Text = "Add mangas to the queue!";
            QueueListBox.SelectedIndex = QueueListBox.Items.Count - 1;
        }
    }

    private async void AddFromLibraryButton_Clicked(object sender, RoutedEventArgs args)
    {
        QueueListBox.SelectedIndex = -1;
        Downloader_AddFromLibrary downloader_AddFromLibrary = new Downloader_AddFromLibrary();
        List<int> result = await downloader_AddFromLibrary.ShowDialog<List<int>>(this);
        if (result == null)
            return;
        StatusTextBox.Text = "Getting manga info...";
        await Task.Run(() => Thread.Sleep(100));
        foreach (int indexToAdd in result)
        {
            passIndex = indexToAdd;
            AddMangaToQueue(mangaList[indexToAdd].ID, true);
        }
        StatusTextBox.Text = "Add mangas to the queue!";
        if (result.Count == 1)
            QueueListBox.SelectedIndex = QueueListBox.Items.Count - 1;
    }

    private async void RemoveFromQueueButton_Clicked(object sender, RoutedEventArgs args)
    {
        if (await MessageBoxManager.GetMessageBoxStandard("Confirmation", "Are you sure you want to remove this Manga from the queue?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
            return;
        int index = QueueListBox.SelectedIndex;
        QueueListBox.SelectedIndex = -1;
        QueueListBox.Items.RemoveAt(index);
        addedMangas.RemoveAt(index);
        mangaQueue.RemoveAt(index);
        if (mangaQueue.Count == 0)
            DownloadButton.IsEnabled = false;
    }

    private void QueueListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TitleComboBox.SelectionChanged -= TitleComboBox_SelectionChanged;
        FormatComboBox.SelectionChanged -= FormatComboBox_SelectionChanged;
        UpdateCoverCheckBox.IsCheckedChanged -= UpdateCoverCheckBox_Checked;
        QualityComboBox.SelectionChanged -= QualityComboBox_SelectionChanged;

        for (int i = ChaptersStackPanel.Children.Count - 1; i > 0; i--)
            ChaptersStackPanel.Children.RemoveAt(i);
        TitleComboBox.Items.Clear();
        SavePathButton.IsEnabled = false;
        UpdateCoverCheckBox.IsEnabled = false;
        if (QueueListBox.SelectedIndex == -1)
        {
            RemoveFromQueueButton.IsVisible = false;

            TitleComboBox.IsEnabled = false;

            FormatComboBox.IsEnabled = true;

            TitleComboBox.SelectionChanged += TitleComboBox_SelectionChanged;
            FormatComboBox.SelectionChanged += FormatComboBox_SelectionChanged;
            UpdateCoverCheckBox.IsCheckedChanged += UpdateCoverCheckBox_Checked;
            QualityComboBox.SelectionChanged += QualityComboBox_SelectionChanged;
            return;
        }
        RemoveFromQueueButton.IsVisible = true;
        for (int i = 0; i < mangaQueue[QueueListBox.SelectedIndex].Chapters.Count; i++)
        {
            CheckBox checkBox = new CheckBox()
            {
                Foreground = new SolidColorBrush(Colors.White),
                IsChecked = mangaQueue[QueueListBox.SelectedIndex].Chapters[i].Checked,
                Content = mangaQueue[QueueListBox.SelectedIndex].Chapters[i].Content,
                Margin = new Avalonia.Thickness(3, 0, 0, 0)
            };
            checkBox.IsCheckedChanged += ChapterCheckBox_Checked;
            ProgressBar progressBar = new ProgressBar()
            {
                Minimum = 0,
                Maximum = mangaQueue[QueueListBox.SelectedIndex].ProgressBars[i].Maximum,
                Value = mangaQueue[QueueListBox.SelectedIndex].ProgressBars[i].Value,
                IsVisible = false,
                Foreground = new SolidColorBrush(Color.FromRgb(248, 200, 220)),
                Background = new SolidColorBrush(Colors.White),
                Margin = new Avalonia.Thickness(3, 0)
            };
            ChaptersStackPanel.Children.Add(new StackPanel()
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = 4,
                Children = { checkBox, progressBar }
            });
        }

        foreach (string title in mangaQueue[QueueListBox.SelectedIndex].Titles)
            TitleComboBox.Items.Add(title);
        TitleComboBox.IsEnabled = true;
        TitleComboBox.SelectedIndex = mangaQueue[QueueListBox.SelectedIndex].SelectedTitleIndex;

        FormatComboBox.IsEnabled = !mangaQueue[QueueListBox.SelectedIndex].Updating;
        FormatComboBox.SelectedIndex = mangaQueue[QueueListBox.SelectedIndex].Format;

        SavePathButton.IsEnabled = !mangaQueue[QueueListBox.SelectedIndex].Updating;
        ToolTip.SetTip(SavePathButton, mangaQueue[QueueListBox.SelectedIndex].SavePath);

        UpdateCoverCheckBox.IsEnabled = mangaQueue[QueueListBox.SelectedIndex].Updating;
        UpdateCoverCheckBox.IsChecked = mangaQueue[QueueListBox.SelectedIndex].UpdateCover;

        if (mangaQueue[QueueListBox.SelectedIndex].OriginalQuality == true)
            QualityComboBox.SelectedIndex = 0;
        else
            QualityComboBox.SelectedIndex = 1;

        TitleComboBox.SelectionChanged += TitleComboBox_SelectionChanged;
        FormatComboBox.SelectionChanged += FormatComboBox_SelectionChanged;
        UpdateCoverCheckBox.IsCheckedChanged += UpdateCoverCheckBox_Checked;
        QualityComboBox.SelectionChanged += QualityComboBox_SelectionChanged;
    }

    private void ChapterCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        for (int i = 1; i < ChaptersStackPanel.Children.Count; i++)
        {
            StackPanel parent = ChaptersStackPanel.Children[i] as StackPanel;
            CheckBox checkBox = parent.Children[0] as CheckBox;
            if (checkBox == (sender as CheckBox))
            {
                mangaQueue[QueueListBox.SelectedIndex].Chapters[i - 1].Checked = (bool)checkBox.IsChecked;
                break;
            }
        }
    }

    private async void AddMangaToQueue(string mangaID, bool isUpdate)
    {
        MDLParameters.MangaID = mangaID;
        QueuedManga newManga = new QueuedManga();
        MDLGetData.GetTitles();
        if (apiError == true)
        {
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
            apiError = false;
            return;
        }
        MDLGetData.GetChapterIDs();
        if (apiError == true)
        {
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
            apiError = false;
            return;
        }
        newManga.Titles = MDLGetData.GetTitles().ToList<string>();
        newManga.SelectedTitleIndex = 0;
        if (isUpdate == true)
        {
            newManga.Titles.Insert(0, mangaList[passIndex].Title);
            if (Path.GetExtension(mangaList[passIndex].Path).ToLower() == ".epub")
                newManga.Format = 0;
            else
                newManga.Format = 1;
            newManga.SavePath = Path.GetDirectoryName(mangaList[passIndex].Path);
        }
        else
            newManga.Format = FormatComboBox.SelectedIndex;
        newManga.UpdateCover = (bool)UpdateCoverCheckBox.IsChecked;
        newManga.OriginalQuality = QualityComboBox.SelectedIndex == 0;
        List<decimal> chapterNumbers = MDLGetData.GetChapterNumbers();
        Dictionary<decimal, int> chapterNumberAppearances = new Dictionary<decimal, int>();
        foreach (decimal number in chapterNumbers)
            if (chapterNumberAppearances.Count > 0 && chapterNumberAppearances.ElementAt(chapterNumberAppearances.Count - 1).Key == number)
                chapterNumberAppearances[number]++;
            else
                chapterNumberAppearances[number] = 1;
        for (int i = 0; i < chapterNumbers.Count; i++)
        {
            if (isUpdate == true && chapterNumbers[i] <= mangaList[passIndex].FileLastChapter)
                continue;

            newManga.Chapters.Add(new MimicCheckBox()
            {
                Checked = MDLGetData.GetAltCuratedChapterIndexes().Contains(i)
            });
            if (chapterNumberAppearances[chapterNumbers[i]] > 1) // If the current chapter number appears more than once, I also add the scanlator name to it
                newManga.Chapters[newManga.Chapters.Count - 1].Content = $"Ch.{chapterNumbers[i]} by {MDLGetData.GetChapterGroups()[i]}";
            else
                newManga.Chapters[newManga.Chapters.Count - 1].Content = $"Ch.{chapterNumbers[i]}";

            newManga.ProgressBars.Add(new MimicProgressBar()
            {
                Maximum = MDLGetData.GetChapterNrPages()[i] - 1,
                Value = 0
            });
        }
        newManga.ChapterIDs = MDLGetData.GetChapterIDs().ToList<string>();
        newManga.ChapterPages = MDLGetData.GetChapterNrPages().ToList<int>();
        while (newManga.Chapters.Count < newManga.ChapterIDs.Count) // Accounting for skipped chapters if it's an update
        {
            newManga.ChapterIDs.RemoveAt(0);
            newManga.ChapterPages.RemoveAt(0);
        }
        newManga.TempManga = new Manga()
        {
            Title = newManga.Titles[0],
            Description = MDLGetData.GetDescription().ToString(),
            Path = newManga.SavePath,
            OnlineLastChapter = MDLGetData.GetLastChapter(),
            LastChecked = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            OngoingStatus = MDLGetData.GetStatus().Substring(0, 1).ToUpper() + MDLGetData.GetStatus().Substring(1),
            CheckInBulk = (MDLGetData.GetStatus().Substring(0, 1).ToUpper() + MDLGetData.GetStatus().Substring(1)) == "Ongoing" || (MDLGetData.GetStatus().Substring(0, 1).ToUpper() + MDLGetData.GetStatus().Substring(1)) == "Hiatus",
            ID = mangaID,
            ContentRating = MDLGetData.GetContentRating().Substring(0, 1).ToUpper() + MDLGetData.GetContentRating().Substring(1),
            Tags = MDLGetData.GetTags().ToList<string>()
        };
        newManga.Updating = isUpdate;

        TextBlock title = new TextBlock()
        {
            Foreground = new SolidColorBrush(Colors.White),
            FontWeight = FontWeight.Bold,
            FontSize = 12,
            Text = newManga.Titles[0],
            Margin = new Avalonia.Thickness(2)
        }, status = new TextBlock()
        {
            Foreground = new SolidColorBrush(Colors.LightGray),
            FontSize = 9,
            Margin = new Avalonia.Thickness(0),
            Text = "In queue"
        };
        QueueListBox.Items.Add(new StackPanel()
        {
            Children = { title, status }
        });
        mangaQueue.Add(newManga);
        addedMangas.Add(mangaID); // This is to not add these to "Add from library"
        mangaQueueStatuses.Add(status);
        DownloadButton.IsEnabled = true;
    }

    #region Downloading

    private void AltQueueListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TitleComboBox.SelectionChanged -= TitleComboBox_SelectionChanged;
        FormatComboBox.SelectionChanged -= FormatComboBox_SelectionChanged;
        UpdateCoverCheckBox.IsCheckedChanged -= UpdateCoverCheckBox_Checked;
        QualityComboBox.SelectionChanged -= QualityComboBox_SelectionChanged;

        for (int i = ChaptersStackPanel.Children.Count - 1; i > 0; i--)
            ChaptersStackPanel.Children.RemoveAt(i);
        TitleComboBox.Items.Clear();

        if (QueueListBox.SelectedIndex == -1)
        {
            TitleComboBox.SelectionChanged += TitleComboBox_SelectionChanged;
            FormatComboBox.SelectionChanged += FormatComboBox_SelectionChanged;
            UpdateCoverCheckBox.IsCheckedChanged += UpdateCoverCheckBox_Checked;
            QualityComboBox.SelectionChanged += QualityComboBox_SelectionChanged;

            return;
        }

        TitleComboBox.Items.Add(mangaQueue[QueueListBox.SelectedIndex].Titles[mangaQueue[QueueListBox.SelectedIndex].SelectedTitleIndex]);
        TitleComboBox.SelectedIndex = 0;

        FormatComboBox.SelectedIndex = mangaQueue[QueueListBox.SelectedIndex].Format;

        UpdateCoverCheckBox.IsChecked = mangaQueue[QueueListBox.SelectedIndex].UpdateCover;

        if (mangaQueue[QueueListBox.SelectedIndex].OriginalQuality == true)
            QualityComboBox.SelectedIndex = 0;
        else
            QualityComboBox.SelectedIndex = 1;

        for (int i = 0; i < mangaQueue[QueueListBox.SelectedIndex].Chapters.Count; i++)
        {
            CheckBox checkBox = new CheckBox()
            {
                Foreground = new SolidColorBrush(Colors.White),
                IsChecked = mangaQueue[QueueListBox.SelectedIndex].Chapters[i].Checked,
                Content = mangaQueue[QueueListBox.SelectedIndex].Chapters[i].Content,
                Margin = new Avalonia.Thickness(3, 0, 0, 0),
                IsEnabled = false
            };
            ProgressBar progressBar = new ProgressBar()
            {
                Minimum = 0,
                Maximum = mangaQueue[QueueListBox.SelectedIndex].ProgressBars[i].Maximum,
                Value = mangaQueue[QueueListBox.SelectedIndex].ProgressBars[i].Value,
                IsVisible = (bool)checkBox.IsChecked,
                Foreground = new SolidColorBrush(Color.FromRgb(248, 200, 220)),
                Background = new SolidColorBrush(Colors.White),
                Margin = new Avalonia.Thickness(3, 0)
            };
            ChaptersStackPanel.Children.Add(new StackPanel()
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = 4,
                Children = { checkBox, progressBar }
            });
        }

        TitleComboBox.SelectionChanged += TitleComboBox_SelectionChanged;
        FormatComboBox.SelectionChanged += FormatComboBox_SelectionChanged;
        UpdateCoverCheckBox.IsCheckedChanged += UpdateCoverCheckBox_Checked;
        QualityComboBox.SelectionChanged += QualityComboBox_SelectionChanged;
    }

    private int queueIndex = 0;
    private bool downloading = false;

    CancellationTokenSource tokenSource = new CancellationTokenSource();
    private CancellationToken cancelCheck;
    private bool skip = false;
    private async void DownloadButton_Clicked(object sender, RoutedEventArgs args)
    {
        if (downloading == true)
        {
            string result = await MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition { Name = "Skip Manga" },
                    new ButtonDefinition { Name = "Stop all" },
                    new ButtonDefinition { Name = "Cancel" }
                },
                ContentTitle = "Skip or cancel",
                ContentMessage = "Would you like to just skip this Manga, or stop all the downloads?",
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                Topmost = true,
                SystemDecorations = SystemDecorations.None
            }).ShowAsync();
            if (result == "Cancel")
                return;
            skip = result == "Skip Manga";
            if (skip == false)
                tokenSource.Cancel();
            return;
        }

        if (await MessageBoxManager.GetMessageBoxStandard("Confirmation", "Are you sure you want to start downloading?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
            return;

        downloading = true;

        QueueListBox.SelectionChanged -= QueueListBox_SelectionChanged;
        QueueListBox.SelectionChanged += AltQueueListBox_SelectionChanged;

        QueueListBox.SelectedIndex = -1;

        foreach (TextBlock textBox in mangaQueueStatuses)
        {
            textBox.Text = "In queue";
            textBox.Foreground = new SolidColorBrush(Colors.LightGray);
        }    

        TitleComboBox.IsEnabled = false;
        FormatComboBox.IsEnabled = false;
        SavePathButton.IsEnabled = false;
        UpdateCoverCheckBox.IsEnabled = false;
        QualityComboBox.IsEnabled = false;

        AddFromLinkButton.IsEnabled = false;
        AddFromLibraryButton.IsEnabled = false;
        RemoveFromQueueButton.IsEnabled = false;

        DownloadButton.Content = "Skip or Stop download";

        cancelCheck = tokenSource.Token;

        _ = Task.Run(() =>
        {
            for (queueIndex = 0; queueIndex < mangaQueue.Count; queueIndex++)
            {
                skip = false;
                int indexHere = queueIndex;
                Dispatcher.UIThread.Post(() =>
                {
                    StatusTextBox.Text = "Preparing files...";
                    StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
                    mangaQueueStatuses[indexHere].Text = "In progress";
                    mangaQueueStatuses[indexHere].Foreground = new SolidColorBrush(Colors.Yellow);
                    QueueListBox.SelectedIndex = indexHere;
                });

                QueuedManga currentManga = mangaQueue[queueIndex];

                currentManga.TempManga.Title = currentManga.Titles[currentManga.SelectedTitleIndex];

                Dictionary<string, int> chaptersToDownload = new Dictionary<string, int>(); // Chapter ID to Nr. of pages
                List<decimal> selectedChapterNumbers = new List<decimal>();
                for (int i = 0; i < currentManga.Chapters.Count; i++)
                    if (currentManga.Chapters[i].Checked == true)
                    {
                        chaptersToDownload[currentManga.ChapterIDs[i]] = currentManga.ChapterPages[i];
                        try
                        {
                            currentManga.TempManga.FileLastChapter = Convert.ToDecimal(currentManga.Chapters[i].Content.Split(' ')[0].Split("Ch.")[1]);
                            selectedChapterNumbers.Add(Convert.ToDecimal(currentManga.Chapters[i].Content.Split(' ')[0].Split("Ch.")[1]));
                        }
                        catch
                        {
                            currentManga.TempManga.FileLastChapter = Convert.ToDecimal(currentManga.Chapters[i].Content.Split("Ch.")[1]);
                            selectedChapterNumbers.Add(Convert.ToDecimal(currentManga.Chapters[i].Content.Split("Ch.")[1]));
                        }
                    }

                if (chaptersToDownload.Count == 0 && currentManga.Updating == true)
                {
                    foreach (Manga manga in mangaList)
                        if (manga.ID == currentManga.TempManga.ID)
                        {
                            currentManga.TempManga.FileLastChapter = manga.FileLastChapter;
                            break;
                        }
                }

                if (currentManga.Updating == false && chaptersToDownload.Count == 0)
                {
                    int indexHere2 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[indexHere2].Text = "Dropped: no chapters selected";
                        mangaQueueStatuses[indexHere2].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                }

                int directoryName = 0;
                while (Directory.Exists(Path.Combine(currentManga.SavePath, $"Temp Manga folder {directoryName}")))
                    directoryName++;

                string tempFolderPath = Path.Combine(currentManga.SavePath, $"Temp Manga folder {directoryName}");

                try
                {
                    Directory.CreateDirectory(Path.Combine(currentManga.SavePath, $"Temp Manga folder {directoryName}"));
                }
                catch
                {
                    int indexHere3 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[indexHere3].Text = "Error: could not create folder";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                }

                if (FileSetup(tempFolderPath, chaptersToDownload.Count) == false) // Creates empty folder structure for new mangas or extracts the archives if it's an update
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        int indexHere3 = queueIndex;
                        mangaQueueStatuses[indexHere3].Text = "Dropped: could not create files";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                }
                string fileName = string.Empty;
                if (currentManga.Updating == false)
                    fileName = currentManga.Titles[currentManga.SelectedTitleIndex];
                else
                {
                    foreach (Manga manga in mangaList)
                        if (manga.ID == currentManga.TempManga.ID)
                        {
                            fileName = Path.GetFileNameWithoutExtension(manga.Path);
                            break;
                        }
                }
                foreach (char c in Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(c.ToString(), string.Empty);
                if (currentManga.Updating == false)
                {
                    currentManga.SavePath = Path.Combine(currentManga.SavePath, fileName);
                    if (currentManga.SavePath.Contains(Path.GetDirectoryName(Environment.ProcessPath)))
                        currentManga.TempManga.Path = currentManga.SavePath.Substring(Path.GetDirectoryName(Environment.ProcessPath).Length);
                    else
                        currentManga.TempManga.Path = currentManga.SavePath;
                    if (currentManga.TempManga.Path[0] == Path.DirectorySeparatorChar)
                        currentManga.TempManga.Path = currentManga.TempManga.Path.Substring(1);
                }

                string cbzPrefix = string.Empty;
                if (currentManga.Format == 1)
                {
                    if (currentManga.Updating == true)
                    {
                        string[] files = Directory.GetFiles(tempFolderPath);
                        int i = 0;
                        while (Path.GetFileNameWithoutExtension(files[0]).Substring(i, 1) == Path.GetFileNameWithoutExtension(files.Last()).Substring(i, 1))
                        {
                            cbzPrefix += Path.GetFileNameWithoutExtension(files[0]).Substring(i, 1);
                            i++;
                        }
                    }
                    else
                        cbzPrefix = "pg-";
                }

                int offset = 0;
                if (currentManga.Updating == true)
                {
                    if (currentManga.Format == 0)
                    {
                        offset = 1; // The folders in the existing manga folder start with 1
                        while (Directory.Exists(Path.Combine(tempFolderPath, offset.ToString())) == true)
                            offset++;
                        offset--; // I need the last folder that was used
                        offset -= chaptersToDownload.Count; // The folders for the new chapters were already created, so gotta subtract those to get to the correct offset
                    } // No, I will not be fixing this atrocious.. thing. It works and it shows my thought process, leave me alone.
                    else
                    {
                        try
                        {
                            offset = Directory.GetFiles(tempFolderPath).Count() - 1;
                        }
                        catch
                        {
                            int indexHere3 = queueIndex;
                            Dispatcher.UIThread.Post(() =>
                            {
                                mangaQueueStatuses[indexHere3].Text = "Dropped: could not read files";
                                mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                            });
                            continue;
                        }
                    }
                }

                int totalPages = 0;
                foreach (KeyValuePair<string, int> chapter in chaptersToDownload)
                    totalPages += chapter.Value;

                if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                {
                    try
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                    catch
                    {
                        Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
                    }
                    if (skip == false)
                    {
                        Dispatcher.UIThread.Post(() => AbortAll());
                        return;
                    }
                    int indexHere3 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[indexHere3].Text = "Dropped: user skipped manga";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                } // Cancel check

                MDLParameters.MangaID = currentManga.TempManga.ID;
                MDLParameters.DataSaving = !currentManga.OriginalQuality;

                List<List<string>> pageFileNames = new List<List<string>>();

                #region Downloading
                int cbzPageNumber = 1, pageFailures = 0;
                bool skipManga = false;
                for (int chapterIndex = offset; chapterIndex < chaptersToDownload.Count + offset; chapterIndex++)
                {
                    string currentPath = Path.Combine(tempFolderPath, Convert.ToString(chapterIndex + 1), "img", string.Empty);
                Retry:
                    if (pageFailures >= 5)
                    {
                        skipManga = true;
                        int indexHere3 = queueIndex;
                        Dispatcher.UIThread.Post(() =>
                        {
                            mangaQueueStatuses[indexHere3].Text = "Dropped: could not complete download";
                            mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                        });
                        break;
                    }
                    if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                    {
                        if (skip == false)
                        {
                            Dispatcher.UIThread.Post(() => AbortAll());
                            return;
                        }
                        break;
                    } // Cancel check
                    List<string> pageLinks = MDLGetData.GetPageLinks(chaptersToDownload.ElementAt(chapterIndex - offset).Key);
                    if (apiError == true)
                    {
                        apiError = false;
                        skipManga = true;
                        int indexHere3 = queueIndex;
                        Dispatcher.UIThread.Post(() =>
                        {
                            _ = MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.\n This Manga will be skipped.", ButtonEnum.Ok).ShowAsync();
                            mangaQueueStatuses[indexHere3].Text = "Dropped: API error";
                            mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                        });
                        break;
                    }

                    List<string> chapterPagePaths = new List<string>();
                    pageFileNames.Add(chapterPagePaths);

                    for (int pageNumber = 1; pageNumber <= pageLinks.Count; pageNumber++)
                    {
                        if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                        {
                            if (skip == false)
                            {
                                Dispatcher.UIThread.Post(() => AbortAll());
                                return;
                            }
                            break;
                        } // Cancel check
                        using MemoryStream pageStream = new MemoryStream();
                        MDLGetData.GetPageImage(pageLinks[pageNumber - 1]).CopyTo(pageStream);
                        if (apiError == true || pageStream == null)
                        {
                            pageFailures++;
                            apiError = false;
                            goto Retry;
                        }
                        pageStream.Seek(0, SeekOrigin.Begin);
                        using SKBitmap page = SKBitmap.Decode(pageStream);
                        // Saving the page
                        try
                        {
                            if (currentManga.Format == 0)
                            {
                                string pageFileName = pageNumber.ToString("D" + chaptersToDownload.ElementAt(chapterIndex - offset).Value.ToString().Length) + ".jpg";
                                using FileStream fileStream = new FileStream(Path.Combine(currentPath, pageFileName), FileMode.Create);
                                SKImage.FromBitmap(page).Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(fileStream);
                                File.WriteAllText(Path.Combine(tempFolderPath, Convert.ToString(chapterIndex + 1), "xhtml", pageNumber - 1 + ".xhtml"), $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <link href=\"../css/style.css\" rel=\"stylesheet\" type=\"text/css\"/>\r\n    <title>{pageFileName}</title>\r\n</head>\r\n<body>\r\n    <div>\r\n        <img alt=\"{pageFileName}\" src=\"../img/{pageFileName}\"/>\r\n    </div>\r\n</body>\r\n</html>\r\n");
                                chapterPagePaths.Add(Path.Combine(currentPath, pageFileName));
                            }
                            else if (currentManga.Format == 1)
                            {
                                int currentPageNumber = offset + cbzPageNumber;
                                string pageFileName;
                                if (currentManga.Updating == false)
                                    pageFileName = $"{cbzPrefix}{currentPageNumber.ToString("D" + totalPages.ToString().Length)}.jpg";
                                else
                                    pageFileName = $"{cbzPrefix}{currentPageNumber.ToString("D" + (totalPages + offset).ToString().Length)}.jpg";
                                using FileStream fileStream = new FileStream(Path.Combine(tempFolderPath, pageFileName), FileMode.Create);
                                SKImage.FromBitmap(page).Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(fileStream);
                                cbzPageNumber++;
                            }
                        }
                        catch
                        {
                            int indexHere3 = queueIndex;
                            Dispatcher.UIThread.Post(() =>
                            {
                                mangaQueueStatuses[indexHere3].Text = "Dropped: could not create files";
                                mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                            });
                            skipManga = true;
                            break;
                        }

                        // Reporting progress
                        currentManga.ProgressBars[chapterIndex - offset].Value++;
                        int indexHere2 = queueIndex, indexHere4 = chapterIndex;
                        Dispatcher.UIThread.Post(() =>
                        {
                            if (QueueListBox.SelectedIndex == indexHere2)
                                ((ProgressBar)((StackPanel)ChaptersStackPanel.Children[mangaQueue[indexHere2].ChapterIDs.IndexOf(chaptersToDownload.ElementAt(indexHere4 - offset).Key) + 1]).Children[1]).Value = mangaQueue[indexHere2].ProgressBars[indexHere4 - offset].Value;
                        });
                    }
                    if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                    {
                        if (skip == false)
                        {
                            Dispatcher.UIThread.Post(() => AbortAll());
                            return;
                        }
                        break;
                    } // Cancel check
                }
                if (skipManga == true)
                {
                    skipManga = false;
                    try
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                    catch
                    {
                        Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
                    }
                    continue;
                }
                if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                {
                    try
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                    catch
                    {
                        Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
                    }
                    if (skip == false)
                    {
                        Dispatcher.UIThread.Post(() => AbortAll());
                        return;
                    }
                    int indexHere3 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[indexHere3].Text = "Dropped: user skipped manga";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                } // Cancel check

                // Get the cover image
                if (currentManga.Updating == false || (currentManga.Updating == true && currentManga.UpdateCover == true))
                {
                    MemoryStream coverStream = new MemoryStream();
                    MDLGetData.GetCover().CopyTo(coverStream);
                    if (apiError == true)
                    {
                        try
                        {
                            Directory.Delete(tempFolderPath, true);
                        }
                        catch
                        {
                            Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
                        }
                        int indexHere3 = queueIndex;
                        Dispatcher.UIThread.Post(() =>
                        {
                            _ = MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.\n This Manga will be skipped.", ButtonEnum.Ok).ShowAsync();
                            mangaQueueStatuses[indexHere3].Text = "Dropped: API error";
                            mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                        });
                        continue;
                    }
                    coverStream.Seek(0, SeekOrigin.Begin);
                    using SKBitmap cover = SKBitmap.Decode(coverStream);
                    try
                    {
                        File.Delete(Path.Combine(tempFolderPath, "cover.jpg"));
                    }
                    catch { }
                    if (currentManga.Format == 0)
                    {
                        using FileStream fileStream = new FileStream(Path.Combine(tempFolderPath, "cover.jpg"), FileMode.Create);
                        SKImage.FromBitmap(cover).Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(fileStream);
                    }
                    else if (currentManga.Format == 1)
                    {
                        string coverFileName;
                        if (currentManga.Updating == false)
                            coverFileName = $"{cbzPrefix}{0.ToString("D" + totalPages.ToString().Length)}.jpg";
                        else
                            coverFileName = $"{cbzPrefix}{0.ToString("D" + (totalPages + offset).ToString().Length)}.jpg";
                        try
                        {
                            File.Delete(Path.Combine(tempFolderPath, coverFileName));
                        }
                        catch { }
                        using FileStream fileStream = new FileStream(Path.Combine(tempFolderPath, coverFileName), FileMode.Create);
                        SKImage.FromBitmap(cover).Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(fileStream);
                    }
                    cover.Dispose();
                }
                #endregion

                if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                {
                    try
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                    catch
                    {
                        Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
                    }
                    if (skip == false)
                    {
                        Dispatcher.UIThread.Post(() => AbortAll());
                        return;
                    }
                    int indexHere3 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[indexHere3].Text = "Dropped: user skipped manga";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                } // Cancel check

                if (currentManga.Format == 1)
                {
                    int hereIndex2 = queueIndex;
                    if (currentManga.Updating == true)
                        PackManga(tempFolderPath, Path.Combine(currentManga.TempManga.Path, fileName));
                    else
                        PackManga(tempFolderPath, currentManga.TempManga.Path);
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[hereIndex2].Text = "Completed";
                        mangaQueueStatuses[hereIndex2].Foreground = new SolidColorBrush(Colors.Lime);
                    });
                    continue;
                }

                #region Creating EPUB files
                bool root = false;
                XmlDocument doc = new XmlDocument();
                try
                {
                    try
                    {
                        doc.Load(Path.Join(tempFolderPath, "content.opf"));
                        root = true;
                    }
                    catch
                    {
                        doc.Load(Path.Join(tempFolderPath, "OEBPS", "content.opf"));
                    }
                }
                catch
                {
                    int indexHere3 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        _ = MessageBoxManager.GetMessageBoxStandard("Missing toc.ncx", "Could not find the content.opf!\nThe download has completed but the EPUB assembly has failed.").ShowAsync();
                        mangaQueueStatuses[indexHere3].Text = "Dropped: could not assemble EPUB";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                }
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

                if (selectedChapterNumbers.Count > 0)
                {
                    string nr = padDecimal(selectedChapterNumbers.Max(), "D" + selectedChapterNumbers.Count.ToString().Length.ToString());
                    doc.DocumentElement.SelectSingleNode("//dc:description", nsmgr).InnerText += "<br>" + "Ch." + nr;
                }

                doc.DocumentElement.SelectSingleNode("//dc:title", nsmgr).InnerText = currentManga.TempManga.Title;
                doc.DocumentElement.SelectSingleNode("//dc:creator", nsmgr).InnerText = MDLGetData.GetAuthors()[0] + ", " + MDLGetData.GetArtists()[0];
                doc.DocumentElement.SelectSingleNode("//dc:date", nsmgr).InnerText = MDLGetData.GetCreationDate();
                doc.DocumentElement.SelectSingleNode("//dc:language", nsmgr).InnerText = selectedLanguage;
                doc.DocumentElement.SelectSingleNode("//dc:identifier", nsmgr).InnerText = "https://mangadex.org/title/" + currentManga.TempManga.ID;
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
                    for (int j = 0; j < chaptersToDownload.ElementAt(i).Value; j++)
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
                        doc.Save(Path.Join(tempFolderPath, "content.opf"));
                    else
                        doc.Save(Path.Join(tempFolderPath, "OEBPS", "content.opf"));
                }
                catch
                {
                    Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not write the content.opf file!").ShowAsync());
                    continue;
                }
                XmlDocument toc = new XmlDocument();
                root = false;
                try
                {
                    try
                    {
                        toc.Load(Path.Join(tempFolderPath, "toc.ncx"));
                        root = true;
                    }
                    catch
                    {
                        toc.Load(Path.Join(tempFolderPath, "OEBPS", "toc.ncx"));
                    }
                }
                catch
                {
                    Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Missing toc.ncx", "Could not find the content.opf!\nThe download has completed but the EPUB assembly has failed.").ShowAsync());
                    continue;
                }
                bool foundUID = false, foundDepth = false;
                foreach (XmlNode node in toc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "head")
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Attributes["name"].Value == "dtb:uid")
                            {
                                child.Attributes["content"].Value = doc.DocumentElement.SelectSingleNode("//dc:identifier", nsmgr).InnerText;
                                foundUID = true;
                            }
                            if (child.Attributes["name"].Value == "dtb:depth" && Convert.ToInt32(child.Attributes["content"].Value) < 2)
                            {
                                child.Attributes["content"].Value = "2";
                                foundDepth = true;
                            }
                            if (foundUID == true && foundDepth == true)
                                break;
                        }
                    if (foundUID == true && foundDepth == true)
                        break;

                }
                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("toc", toc.DocumentElement.GetAttribute("xmlns"));
                toc.DocumentElement.SelectSingleNode("toc:docTitle/toc:text", ns).InnerText = currentManga.TempManga.Title;
                int playOrder = 0;
                XmlNode navMap = toc.DocumentElement.SelectSingleNode("toc:navMap", ns);
                if (currentManga.Updating == true)
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
                    for (int j = 0; j < chaptersToDownload.ElementAt(i).Value; j++)
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
                        toc.Save(Path.Join(tempFolderPath, "toc.ncx"));
                    else
                        toc.Save(Path.Join(tempFolderPath, "OEBPS", "toc.ncx"));
                }
                catch
                {
                    Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not write the content.opf file!").ShowAsync());
                    continue;
                }
                #endregion

                if (cancelCheck.IsCancellationRequested || skip == true) // Cancel check
                {
                    try
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                    catch
                    {
                        Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
                    }
                    if (skip == false)
                    {
                        Dispatcher.UIThread.Post(() => AbortAll());
                        return;
                    }
                    int indexHere3 = queueIndex;
                    Dispatcher.UIThread.Post(() =>
                    {
                        mangaQueueStatuses[indexHere3].Text = "Dropped: user skipped manga";
                        mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                    });
                    continue;
                } // Cancel check

                if (currentManga.Updating == true)
                    PackManga(tempFolderPath, Path.Combine(currentManga.TempManga.Path, fileName));
                else
                    PackManga(tempFolderPath, currentManga.TempManga.Path);

                int hereIndex = queueIndex;
                Dispatcher.UIThread.Post(() =>
                {
                    mangaQueueStatuses[hereIndex].Text = "Completed";
                    mangaQueueStatuses[hereIndex].Foreground = new SolidColorBrush(Colors.Lime);
                });
            }

            downloading = false;
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBox.Text = "All done!";
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Lime);
                DownloadButton.IsEnabled = false;
            });
        }, cancelCheck);
    }

    private bool FileSetup(string tempFolderPath, int chapterCount)
    {
        QueuedManga currentManga = mangaQueue[queueIndex];
        if (currentManga.Updating == true && currentManga.Format == 0)
        {
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBox.Text = "Extracting file...";
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
            });

            string filePath = string.Empty;
            foreach (Manga manga in mangaList)
                if (manga.ID == currentManga.TempManga.ID)
                {
                    filePath = manga.Path;
                    break;
                }

            try
            {
                ZipFile.ExtractToDirectory(filePath, tempFolderPath);
                int offset = 1;
                while (Directory.Exists(Path.Combine(tempFolderPath, offset.ToString())) == true)
                    offset++;
                for (int i = offset; i < chapterCount + offset; i++)
                {
                    string currentFolder = Path.Combine(tempFolderPath, Convert.ToString(i));
                    Directory.CreateDirectory(currentFolder);
                    Directory.CreateDirectory(Path.Combine(currentFolder, "css"));
                    File.WriteAllText(Path.Combine(currentFolder, "css", "style.css"), "img {\r\n    max-height: 100%;\r\n    max-width: 100%;\r\n}");
                    Directory.CreateDirectory(Path.Combine(currentFolder, "img"));
                    Directory.CreateDirectory(Path.Combine(currentFolder, "xhtml"));
                }
            }
            catch 
            {
                return false;
            }

            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBox.Text = "Downloading...";
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
            });
            return true;
        }
        else if (currentManga.Updating == true && currentManga.Format == 1)
        {
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBox.Text = "Extracting file...";
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
            });

            string filePath = string.Empty;
            foreach (Manga manga in mangaList)
                if (manga.ID == currentManga.TempManga.ID)
                {
                    filePath = manga.Path;
                    break;
                }

            try
            {
                ZipFile.ExtractToDirectory(filePath, tempFolderPath);
            }
            catch
            {
                return false;
            }

            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBox.Text = "Downloading...";
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
            });
            return true;
        }
        else if (currentManga.Format == 0)
        {
            try
            {
                for (int i = 0; i < chapterCount; i++)
                {
                    string currentFolder = Path.Combine(tempFolderPath, Convert.ToString(i + 1));
                    Directory.CreateDirectory(currentFolder);
                    Directory.CreateDirectory(Path.Combine(currentFolder, "css"));
                    File.WriteAllText(Path.Combine(currentFolder, "css", "style.css"), "img {\r\n    max-height: 100%;\r\n    max-width: 100%;\r\n}");
                    Directory.CreateDirectory(Path.Combine(currentFolder, "img"));
                    Directory.CreateDirectory(Path.Combine(currentFolder, "xhtml"));
                }
                Directory.CreateDirectory(Path.Combine(tempFolderPath, "META-INF"));
                File.WriteAllText(Path.Combine(tempFolderPath, "META-INF", "container.xml"), "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">\r\n   <rootfiles>\r\n      <rootfile full-path=\"content.opf\" media-type=\"application/oebps-package+xml\"/>\r\n   </rootfiles>\r\n</container>\r\n");
                File.WriteAllText(Path.Combine(tempFolderPath, "mimetype"), "application/epub+zip");
                File.WriteAllText(Path.Combine(tempFolderPath, "cover.xhtml"), "\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\"><head><title>Cover</title><style type=\"text/css\" title=\"override_css\">\r\n@page {padding: 0pt; margin:0pt}\r\nbody { text-align: center; padding:0pt; margin: 0pt; }\r\ndiv { margin: 0pt; padding: 0pt; }\r\n</style></head><body><div>\r\n<img src=\"cover.jpg\" alt=\"cover\"/>\r\n</div></body></html>\r\n");

                File.WriteAllText(Path.Combine(tempFolderPath, "content.opf"), "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<package xmlns=\"http://www.idpf.org/2007/opf\" version=\"2.0\">\r\n  <metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:opf=\"http://www.idpf.org/2007/opf\">\r\n    <dc:title></dc:title>\r\n    <dc:creator opf:role=\"aut\"></dc:creator>\r\n    <dc:rights>Copyrights as per source stories</dc:rights>\r\n    <dc:description></dc:description>\r\n    <dc:date></dc:date>\r\n    <dc:language></dc:language>\r\n    <dc:identifier id=\"book-id\"></dc:identifier>\r\n    <meta name=\"cover\" content=\"coverimageid\" />\r\n  </metadata>\r\n  <manifest>\r\n    <item id=\"cover\" href=\"cover.xhtml\" media-type=\"application/xhtml+xml\" />\r\n    <item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\" />\r\n    <item id=\"coverimageid\" href=\"cover.jpg\" media-type=\"image/jpeg\" />\r\n  </manifest>\r\n  <spine toc=\"ncx\">\r\n    <itemref idref=\"cover\" linear=\"yes\" />\r\n  </spine>\r\n  <guide>\r\n    <reference type=\"cover\" title=\"Cover\" href=\"cover.xhtml\" />\r\n  </guide>\r\n</package>");
                File.WriteAllText(Path.Combine(tempFolderPath, "toc.ncx"), "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<ncx version=\"2005-1\" xmlns=\"http://www.daisy.org/z3986/2005/ncx/\">\r\n  <head>\r\n    <meta name=\"dtb:uid\" content=\"\" />\r\n    <meta name=\"dtb:depth\" content=\"2\" />\r\n    <meta name=\"dtb:totalPageCount\" content=\"0\" />\r\n    <meta name=\"dtb:maxPageNumber\" content=\"0\" />\r\n  </head>\r\n  <docTitle>\r\n    <text>\r\n    </text>\r\n  </docTitle>\r\n  <navMap>\r\n</navMap>\r\n</ncx>");

                Dispatcher.UIThread.Post(() =>
                {
                    StatusTextBox.Text = "Downloading...";
                    StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        else if (currentManga.Format == 1)
        {
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBox.Text = "Downloading...";
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
            });
            return true;
        }
        return false;
    }

    private void PackManga(string tempFolderPath, string fileName)
    {
        Dispatcher.UIThread.Post(() =>
        {
            StatusTextBox.Text = "Archiving files...";
            StatusTextBox.Foreground = new SolidColorBrush(Colors.Yellow);
        });
        if (mangaQueue[queueIndex].Format == 0)
            fileName += ".epub";
        else if (mangaQueue[queueIndex].Format == 1)
            fileName += ".cbz";
        try
        {
            File.Delete(fileName);
        }
        catch { }
        try
        {
            ZipFile.CreateFromDirectory(tempFolderPath, fileName);
        }
        catch
        {
            int indexHere3 = queueIndex;
            Dispatcher.UIThread.Post(() =>
            {
                mangaQueueStatuses[indexHere3].Text = "Dropped: could not write archive";
                mangaQueueStatuses[indexHere3].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
            });
            return;
        }
        try
        {
            Directory.Delete(tempFolderPath, true);
        }
        catch
        {
            Dispatcher.UIThread.Post(() => _ = MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the temporary folder, please delete it yourself.").ShowAsync());
        }
        mangaQueue[queueIndex].TempManga.Path = fileName;
        if (mangaQueue[queueIndex].Updating == false)
            mangaList.Add(mangaQueue[queueIndex].TempManga);
        else
            for (int i = 0; i < mangaList.Count; i++)
                if (mangaList[i].ID == mangaQueue[queueIndex].TempManga.ID)
                {
                    mangaList[i] = mangaQueue[queueIndex].TempManga;
                    break;
                }
    }

    public string padDecimal(decimal value, string padding)
    {
        if (value - Convert.ToInt32(Math.Floor(value)) != 0)
            return Convert.ToInt32(Math.Floor(value)).ToString(padding) + (value - Convert.ToInt32(Math.Floor(value))).ToString().Substring(1);
        return Convert.ToInt32(value).ToString(padding);
    }

    private void AbortAll()
    {
        downloading = false;

        QueueListBox.SelectedIndex = -1;

        QueueListBox.SelectionChanged -= AltQueueListBox_SelectionChanged;
        QueueListBox.SelectionChanged += QueueListBox_SelectionChanged;
        TitleComboBox.IsEnabled = true;
        QualityComboBox.IsEnabled = true;

        AddFromLinkButton.IsEnabled = true;
        AddFromLibraryButton.IsEnabled = true;
        RemoveFromQueueButton.IsEnabled = true;

        StatusTextBox.Text = "Add mangas to the queue!";
        StatusTextBox.Foreground = new SolidColorBrush(Colors.White);
        DownloadButton.Content = "Start downloading";

        for (int i = queueIndex; i < mangaQueue.Count; i++)
        {
            mangaQueueStatuses[i].Text = "Dropped: user cancelled all downloads";
            mangaQueueStatuses[i].Foreground = new SolidColorBrush(Color.FromRgb(130, 0, 0));
        }
    }

    #endregion

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (downloading == true)
        {
            e.Cancel = true;
            return;
        }
        addedMangas.Clear();

        base.OnClosing(e);
    }
}