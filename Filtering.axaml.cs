using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Collections.Generic;
using static Manga_Manager.Globals;

namespace Manga_Manager;

public partial class Filtering : Window
{
    public Filtering()
    {
        InitializeComponent();

        TotalMangasTextBlock.Text = $"Mangas: {mangaList.Count}";
        int safeMangas = 0, suggestiveMangas = 0, eroticaMangas = 0, pornographicMangas = 0;
        foreach (Manga manga in mangaList)
            if(manga.ContentRating == "Safe")
                safeMangas++;
            else if (manga.ContentRating == "Suggestive")
                suggestiveMangas++;
            else if (manga.ContentRating == "Erotica")
                eroticaMangas++;
            else if (manga.ContentRating == "Pornographic")
                pornographicMangas++;
        MangaCountByTagsTextBlock.Text = $"\nSafe: {safeMangas}\nSuggestive: {suggestiveMangas}\nErotica: {eroticaMangas}\nPornographic: {pornographicMangas}\n\nNr. of Mangas by tag:";
        foreach (KeyValuePair<string, int> tag in tagsUsage)
            MangaCountByTagsTextBlock.Text += $"\n{tag.Key}: {tag.Value}";
        LoadControlStates();
    }

    private List<CheckBox> tagsCheckBoxes = new List<CheckBox>();
    private void LoadControlStates()
    {
        foreach (var child in ContentRatingCheckBoxesPanel.Children)
        {
            CheckBox checkBox = child as CheckBox;
            checkBox.IsChecked = Filters.IncludedContentRatings.Contains(checkBox.Content.ToString());
        }
        OnlyShowNewChaptersCheckBox.IsChecked = Filters.OnlyShowMangasWithUpdates;
        if (Filters.InclusionModeIsAnd == true)
            InclusionAndRadioButton.IsChecked = true;
        else
            InclusionOrRadioButton.IsChecked = true;
        if (Filters.ExclusionModeIsAnd == true)
            ExclusionAndRadioButton.IsChecked = true;
        else
            ExclusionOrRadioButton.IsChecked = true;
        tagsCheckBoxes.Clear();
        TagCheckBoxesWrapPanel.Children.Clear();
        foreach (KeyValuePair<string, int> tag in tagsUsage)
        {
            CheckBox checkBox = new CheckBox
            {
                Content = tag.Key,
                Margin = new Avalonia.Thickness(4, 2),
                Foreground = new SolidColorBrush(Colors.White),
                IsThreeState = true
            };
            if (Filters.IncludedTags.Contains(tag.Key))
                checkBox.IsChecked = true;
            else if (Filters.ExcludedTags.Contains(tag.Key))
                checkBox.IsChecked = null;
            TagCheckBoxesWrapPanel.Children.Add(checkBox);
            tagsCheckBoxes.Add(checkBox);
        }
    }

    private void ResetFiltersButton_Clicked(object sender, RoutedEventArgs args)
    {
        Filters.IncludedContentRatings = ["Safe", "Suggestive", "Erotica", "Pornographic"];
        Filters.OnlyShowMangasWithUpdates = false;
        Filters.InclusionModeIsAnd = true;
        Filters.ExclusionModeIsAnd = false;
        Filters.IncludedTags.Clear();
        Filters.ExcludedTags.Clear();
        LoadControlStates();
    }

    private void DemoCheckBoxes_Checked(object sender, RoutedEventArgs e)
    {
        // Avalonia weirdness here, if I refer to them directly by name it throws an object reference null exception. This is also called during initialization to set their default states, so the Window doesn't even open.
        CheckBox checkBox = sender as CheckBox;
        if (checkBox.Content.ToString() == "No preference")
            checkBox.IsChecked = false;
        else if (checkBox.Content.ToString() == "Included")
            checkBox.IsChecked = true;
        else if (checkBox.Content.ToString() == "Excluded")
            checkBox.IsChecked = null;
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        Filters.IncludedContentRatings.Clear();
        foreach (var child in ContentRatingCheckBoxesPanel.Children)
        {
            CheckBox checkBox = child as CheckBox;
            if (checkBox.IsChecked == true)
                Filters.IncludedContentRatings.Add(checkBox.Content.ToString());
        }
        Filters.OnlyShowMangasWithUpdates = (bool)OnlyShowNewChaptersCheckBox.IsChecked;
        Filters.InclusionModeIsAnd = InclusionAndRadioButton.IsChecked == true;
        Filters.ExclusionModeIsAnd = ExclusionAndRadioButton.IsChecked == true;
        Filters.IncludedTags.Clear();
        Filters.ExcludedTags.Clear();
        foreach (CheckBox checkBox in tagsCheckBoxes)
        {
            if (checkBox.IsChecked == true)
                Filters.IncludedTags.Add(checkBox.Content.ToString());
            else if (checkBox.IsChecked == null)
                Filters.ExcludedTags.Add(checkBox.Content.ToString());
        }

        base.OnClosing(e);
    }
}