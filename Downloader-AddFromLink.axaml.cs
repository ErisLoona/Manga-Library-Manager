using Avalonia.Controls;
using Avalonia.Interactivity;
using static Manga_Manager.Globals;

namespace Manga_Manager;

public partial class Downloader_AddFromLink : Window
{
    public Downloader_AddFromLink()
    {
        InitializeComponent();
    }

    private void LinkTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ValidateLink(LinkTextBox.Text) == true)
            AddButton.IsEnabled = true;
        else
            AddButton.IsEnabled = false;
    }

    private void AddButton_Clicked(object sender, RoutedEventArgs args)
    {
        Close(LinkTextBox.Text);
    }
}