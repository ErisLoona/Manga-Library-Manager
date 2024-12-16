using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using static Manga_Manager.Globals;
using System.Collections.Generic;
using System.IO;
using Avalonia.Input;
using System;
using System.Linq;

namespace Manga_Manager;

public partial class Downloader_AddFromLibrary : Window
{
    private Dictionary<string, int> addedMangaIndexes = new Dictionary<string, int>();
    private List<CheckBox> checkBoxes = new List<CheckBox>();

    public Downloader_AddFromLibrary()
    {
        InitializeComponent();

        for (int i = 0; i < mangaList.Count; i++)
        {
            if (mangaList[i].ID != string.Empty && Downloader.addedMangas.Contains(mangaList[i].ID) == false && File.Exists(mangaList[i].Path))
            {
                checkBoxes.Add(new CheckBox()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(3, 0, 0, 0),
                    Content = mangaList[i].Title
                });
                MangaStackPanel.Children.Add(checkBoxes[checkBoxes.Count - 1]);
                addedMangaIndexes[mangaList[i].Title] = i;
            }
        }

        SearchBox.ItemsSource = addedMangaIndexes.Keys.ToArray();
    }

    private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (SearchBox.Text == string.Empty)
            return;
        for (int i = 0; i < addedMangaIndexes.Count; i++)
            if (addedMangaIndexes.ElementAt(i).Key.ToLower().Contains(SearchBox.Text.ToLower()))
            {
                checkBoxes[i].IsChecked = true;
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

    private void SelectAllButton_Clicked(object sender, RoutedEventArgs args)
    {
        foreach (CheckBox checkBox in checkBoxes)
            checkBox.IsChecked = true;
    }

    private void ConfirmButton_Clicked(object sender, RoutedEventArgs args)
    {
        List<int> selectedIndexes = new List<int>();
        foreach (CheckBox checkBox in checkBoxes)
        {
            if (checkBox.IsChecked == true)
                selectedIndexes.Add(addedMangaIndexes[checkBox.Content.ToString()]);
        }
        Close(selectedIndexes);
    }
}