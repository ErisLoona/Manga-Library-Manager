using Avalonia.Controls;
using Avalonia.Interactivity;
using MangaDex_Library;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static Manga_Manager.Globals;

namespace Manga_Manager;

public partial class Settings : Window
{
    private bool isFirstTrigger;

    public Settings()
    {
        InitializeComponent();

        LanguageComboBox.ItemsSource = languageDictionary.Keys;
        LanguageComboBox.SelectedItem = languageDictionary.FirstOrDefault(language => language.Value == selectedLanguage).Key;
        UpdatesCheckBox.IsChecked = checkUpdates;
        isFirstTrigger = hideJsonFile;
        HideJsonCheckBox.IsChecked = hideJsonFile;
        WarningCheckBox.IsChecked = !noWarning;
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedLanguage = languageDictionary[LanguageComboBox.SelectedItem.ToString()];
        MDLParameters.Language = selectedLanguage;
    }

    private void UpdatesCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        checkUpdates = (bool)UpdatesCheckBox.IsChecked;
    }

    private async void HideJsonCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        hideJsonFile = (bool)HideJsonCheckBox.IsChecked;
        if (isFirstTrigger == true)
        {
            isFirstTrigger = false;
            return;
        }
        try
        {
            if (hideJsonFile == true)
            {
                File.Move("Manga Library Manager.json", ".Manga Library Manager.json");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    File.SetAttributes(".Manga Library Manager.json", File.GetAttributes(".Manga Library Manager.json") | FileAttributes.Hidden);
            }
            else
            {
                File.Move(".Manga Library Manager.json", "Manga Library Manager.json");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    File.SetAttributes("Manga Library Manager.json", File.GetAttributes("Manga Library Manager.json") & ~FileAttributes.Hidden);
            }
        }
        catch
        {
            await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not complete the operation!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            isFirstTrigger = true;
            HideJsonCheckBox.IsChecked = !HideJsonCheckBox.IsChecked;
        }
    }

    private void WarningCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        noWarning = !(bool)WarningCheckBox.IsChecked;
    }

    private void IssueButton_Clicked(object sender, RoutedEventArgs args)
    {
        OpenLink("https://github.com/ErisLoona/Manga-Library-Manager/issues");
    }

    private void DonateButton_Clicked(object sender, RoutedEventArgs args)
    {
        OpenLink("https://ko-fi.com/erisloona");
    }
}