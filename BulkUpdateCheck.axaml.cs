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
using Avalonia.Threading;

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
        await Task.Run(() =>
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
                    apiError = false;
                    mangaIndexesNewChapters[i] = -1;
                    continue;
                }
                decimal onlineChapter = MDLGetData.GetLastChapter();
                mangaList[i].OnlineLastChapter = onlineChapter;
                mangaList[i].LastChecked = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                int newChapters = 0;
                for (int j = 0; j < MDLGetData.GetChapterNumbers().Count; j++)
                    if (MDLGetData.GetChapterNumbers()[j] > mangaList[i].FileLastChapter)
                    {
                        if (j > 0 && MDLGetData.GetChapterNumbers()[j] == MDLGetData.GetChapterNumbers()[j - 1])
                            continue;
                        newChapters++;
                    }
                mangaIndexesNewChapters[i] = newChapters;
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