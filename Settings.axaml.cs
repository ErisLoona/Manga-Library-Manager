using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using static Manga_Manager.Globals;

namespace Manga_Manager;

public partial class Settings : Window
{
    public Settings()
    {
        InitializeComponent();

        LanguageComboBox.ItemsSource = languageDictionary.Keys;
        LanguageComboBox.SelectedItem = languageDictionary.FirstOrDefault(language => language.Value == selectedLanguage).Key;
        UpdatesCheckBox.IsChecked = checkUpdates;
        HideJsonCheckBox.IsChecked = hideJsonFile;
        WarningCheckBox.IsChecked = !noWarning;
    }

    public void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedLanguage = languageDictionary[LanguageComboBox.SelectedItem.ToString()];
    }

    private void UpdatesCheckBox_Checked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        checkUpdates = (bool)UpdatesCheckBox.IsChecked;
    }

    private void HideJsonCheckBox_Checked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        hideJsonFile = (bool)HideJsonCheckBox.IsChecked;
    }

    private void WarningCheckBox_Checked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        noWarning = !(bool)WarningCheckBox.IsChecked;
    }

    public void IssueButton_Clicked(object sender, RoutedEventArgs args)
    {
        OpenLink("https://github.com/ErisLoona/Manga-Library-Manager/issues");
    }

    public void DonateButton_Clicked(object sender, RoutedEventArgs args)
    {
        OpenLink("https://ko-fi.com/erisloona");
    }
}