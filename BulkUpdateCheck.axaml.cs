using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Manga_Manager.Globals;
using MangaDex_Library;
using System.Linq;
using Avalonia.Media;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace Manga_Manager;

public partial class BulkUpdateCheck : Window
{
    private bool checking = true, cancelled = false, destroyInput1 = false, destroyInput2 = false;
    private List<int> mangaIndexes = new List<int>();
    private Dictionary<int, int> mangaIndexesNewChapters = new Dictionary<int, int>();
    private CancellationTokenSource tokenSource = new CancellationTokenSource();
    private CancellationToken cancelCheck;

    public BulkUpdateCheck()
    {
        InitializeComponent();
        StartChecking();
    }

    private async void StartChecking()
    {
        for (int i = 0; i < mangaList.Count; i++)
            if (mangaList[i].CheckInBulk == true)
                mangaIndexes.Add(i);
        ProgressBar.Maximum = mangaIndexes.Count;
        cancelCheck = tokenSource.Token;
        IProgress<int> progress = new Progress<int>(v => { ProgressBar.Value++; });
        await Task.Run(async () =>
        {
            foreach (int i in mangaIndexes)
            {
                if (cancelCheck.IsCancellationRequested)
                {
                    cancelled = true;
                    return;
                }
                MDLParameters.MangaID = mangaList[i].ID;
                MDLGetData.GetLastChapter();
                if (apiError == true)
                {
                    await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
                    apiError = false;
                    return;
                }
                decimal onlineChapter = MDLGetData.GetLastChapter();
                mangaList[i].OnlineLastChapter = onlineChapter;
                mangaList[i].LastChecked = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                mangaIndexesNewChapters[i] = Convert.ToInt32(Math.Ceiling(onlineChapter - mangaList[i].FileLastChapter));
                progress.Report(0);
            }
            mangaIndexesNewChapters = mangaIndexesNewChapters.OrderByDescending(pair => pair.Value).ToDictionary();
        }, cancelCheck);
        checking = false;
        CancelButton.Content = "Close";
        if (cancelled == true)
            return;
        foreach (KeyValuePair<int, int> result in mangaIndexesNewChapters)
        {
            MangaNameList.Items.Add(new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 12,
                Text = mangaList[result.Key].Title
            });
            if (result.Value == 0)
                ChapterStatusList.Items.Add(new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 12,
                    Text = "-> Up to date!"
                });
            else if (result.Value == 1)
                ChapterStatusList.Items.Add(new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 12,
                    Text = "-> 1 chapter ahead."
                });
            else if (result.Value < 0)
                ChapterStatusList.Items.Add(new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 12,
                    Text = "-> Please check book link!"
                });
            else
                ChapterStatusList.Items.Add(new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 12,
                    Text = "-> " + Convert.ToString(result.Value) + " chapters ahead."
                });
        }
    }

    private void MangaNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (destroyInput1 == true)
        {
            destroyInput1 = false;
            return;
        }
        destroyInput2 = true;
        ChapterStatusList.SelectedIndex = MangaNameList.SelectedIndex;
    }

    private void ChapterStatusList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (destroyInput2 == true)
        {
            destroyInput2 = false;
            return;
        }
        destroyInput1 = true;
        MangaNameList.SelectedIndex = ChapterStatusList.SelectedIndex;
    }

    private void CancelButton_Clicked(object sender, RoutedEventArgs args)
    {
        if (checking == true)
            tokenSource.Cancel();
        else
            this.Close();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (checking == true)
            tokenSource.Cancel();

        base.OnClosing(e);
    }
}