using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using static Manga_Manager.Globals;
using System.Collections.Generic;
using System.IO;

namespace Manga_Manager;

public partial class Downloader_AddFromLibrary : Window
{
    Dictionary<string, int> addedMangaIndexes = new Dictionary<string, int>();

    public Downloader_AddFromLibrary()
    {
        InitializeComponent();

        for (int i = 0; i < mangaList.Count; i++)
        {
            if (mangaList[i].ID != string.Empty && Downloader.addedMangas.Contains(mangaList[i].ID) == false && File.Exists(mangaList[i].Path))
            {
                MangaStackPanel.Children.Add(new CheckBox()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(3, 0, 0, 0),
                    Content = mangaList[i].Title
                });
                addedMangaIndexes[mangaList[i].Title] = i;
            }
        }
    }

    private void SelectAllButton_Clicked(object sender, RoutedEventArgs args)
    {
        foreach (Control child in MangaStackPanel.Children)
        {
            CheckBox checkBox = child as CheckBox;
            checkBox.IsChecked = true;
        }
    }

    private void ConfirmButton_Clicked(object sender, RoutedEventArgs args)
    {
        List<int> selectedIndexes = new List<int>();
        foreach (Control child in MangaStackPanel.Children)
        {
            CheckBox checkBox = child as CheckBox;
            if (checkBox.IsChecked == true)
                selectedIndexes.Add(addedMangaIndexes[checkBox.Content.ToString()]);
        }
        Close(selectedIndexes);
    }
}