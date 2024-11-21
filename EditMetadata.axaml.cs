using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using static Manga_Manager.Globals;
using MangaDex_Library;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Interactivity;
using System.IO;
using Avalonia.Threading;
using System.IO.Compression;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.Linq;

namespace Manga_Manager;

public partial class EditMetadata : Window
{
    private List<CheckBox> tagCheckBoxes = new List<CheckBox>();
    private Bitmap newCover;

    private static readonly string[] contentRatings = [ "Safe", "Suggestive", "Erotica", "Pornographic" ], ongoingStatuses = [ "Ongoing", "Completed", "Hiatus", "Cancelled" ];

    public EditMetadata()
    {
        InitializeComponent();

        if (mangaList[passIndex].ID != string.Empty)
        {
            MDLParameters.MangaID = mangaList[passIndex].ID;
            UpdateAllButton.IsEnabled = true;
            UpdateCoverButton.IsEnabled = true;
            UpdateDescriptionButton.IsEnabled = true;
            UpdateOngoingStatusButton.IsEnabled = true;
            UpdateContentRatingButton.IsEnabled = true;
            UpdateTagsButton.IsEnabled = true;
            OpenLinkButton.IsEnabled = true;

            LinkTextBox.TextChanged -= LinkTextBox_TextChanged;
            LinkTextBox.Text = "https://mangadex.org/title/" + mangaList[passIndex].ID;
            LinkTextBox.TextChanged += LinkTextBox_TextChanged;
        }

        if (File.Exists(mangaList[passIndex].Path) == true)
        {
            _ = Task.Run(() =>
            {
                using ZipArchive manga = ZipFile.OpenRead(mangaList[passIndex].Path);
                if (System.IO.Path.GetExtension(mangaList[passIndex].Path).ToLower() == ".epub")
                {
                    foreach (ZipArchiveEntry entry in manga.Entries)
                        if (entry.Name == "cover.jpg" || entry.Name == "cover.jpeg" || entry.Name == "cover.png" || entry.Name == "cover.webp")
                        {
                            MemoryStream stream = new MemoryStream();
                            entry.Open().CopyTo(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            Dispatcher.UIThread.Post(() => { CurrentCoverImage.Source = new Bitmap(stream); });
                            return;
                        }
                }
                else if (System.IO.Path.GetExtension(mangaList[passIndex].Path).ToLower() == ".cbz")
                {
                    MemoryStream stream = new MemoryStream();
                    manga.Entries.First().Open().CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Dispatcher.UIThread.Post(() => { CurrentCoverImage.Source = new Bitmap(stream); });
                    return;
                }
            });
        }
        DescriptionTextBox.Text = mangaList[passIndex].Description;
        LastChapterNumeric.Value = mangaList[passIndex].FileLastChapter;
        OngoingStatusComboBox.SelectedIndex = Array.IndexOf(ongoingStatuses, mangaList[passIndex].OngoingStatus);
        ContentRatingComboBox.SelectedIndex = Array.IndexOf(contentRatings, mangaList[passIndex].ContentRating);
        AddNewTagPanel();
        foreach (KeyValuePair<string, int> tag in tagsUsage)
        {
            CheckBox checkBox = new CheckBox()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Content = tag.Key
            };
            if (mangaList[passIndex].Tags.Contains(tag.Key))
                checkBox.IsChecked = true;
            tagCheckBoxes.Add(checkBox);
            TagsPanel.Children.Add(checkBox);
        }
    }

    private async void ConfirmButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (await MessageBoxManager.GetMessageBoxStandard("Confirmation", "The old cover cannot be recovered.\nThis process may take a while.\nAre you sure you want to proceed?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync() == MsBox.Avalonia.Enums.ButtonResult.No)
            return;
    }

    private void UpdateCoverButton_Clicked(object sender, RoutedEventArgs e)
    {
        _ = Task.Run(() => {
            MemoryStream stream = new MemoryStream();
            MDLGetData.GetCover().CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            if (apiError == true)
                return;
            newCover = new Bitmap(stream);
            Dispatcher.UIThread.Post(() => {
                NewCoverImage.Source = newCover;
                ConfirmButton.IsEnabled = true;
            });
        });
    }

    private void UpdateDescriptionButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        MDLGetData.GetDescription();
        if (apiError == true)
        {
            apiError = false;
            return;
        }
        DescriptionTextBox.Text = MDLGetData.GetDescription();
    }

    private void UpdateOngoingStatusButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        MDLGetData.GetStatus();
        if (apiError == true)
        {
            apiError = false;
            return;
        }
        OngoingStatusComboBox.SelectedIndex = Array.IndexOf(ongoingStatuses, MDLGetData.GetStatus().Substring(0, 1).ToUpper() + MDLGetData.GetStatus().Substring(1));
    }

    private void OpenLinkButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ValidateLink(LinkTextBox.Text) == true)
            OpenLink(LinkTextBox.Text);
    }

    private void UpdateContentRatingButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        MDLGetData.GetContentRating();
        if (apiError == true)
        {
            apiError = false;
            return;
        }
        ContentRatingComboBox.SelectedIndex = Array.IndexOf(contentRatings, MDLGetData.GetContentRating().Substring(0, 1).ToUpper() + MDLGetData.GetContentRating().Substring(1));
    }

    private void UpdateTagsButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        List<string> newTags = MDLGetData.GetTags();
        if (apiError == true)
        {
            apiError = false;
            return;
        }

        TagsPanel.Children.Clear();
        tagCheckBoxes.Clear();
        AddNewTagPanel();
        foreach (KeyValuePair<string, int> tag in tagsUsage)
        {
            CheckBox checkBox = new CheckBox()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Content = tag.Key
            };
            tagCheckBoxes.Add(checkBox);
            TagsPanel.Children.Add(checkBox);
        }

        foreach (string tag in newTags)
            if (tagsUsage.ContainsKey(tag))
            {
                foreach (CheckBox checkBox in tagCheckBoxes)
                    if (checkBox.Content.ToString() == tag)
                    {
                        checkBox.IsChecked = true;
                        break;
                    }
            }
            else
            {
                CheckBox checkBox = new CheckBox()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = tag,
                    IsChecked = true
                };
                tagCheckBoxes.Add(checkBox);
                TagsPanel.Children.Add(checkBox);
            }
    }

    private async void NewTagButton_Clicked(object sender, RoutedEventArgs e)
    {
        StackPanel stackPanel = TagsPanel.Children[0] as StackPanel;
        TextBox newTagTextBox = stackPanel.Children[0] as TextBox;
        foreach (CheckBox tagCheckBox in tagCheckBoxes)
            if (tagCheckBox.Content.ToString().ToLower() == newTagTextBox.Text.ToLower())
            {
                await MessageBoxManager.GetMessageBoxStandard("Tag already exists", "This tag already exists!").ShowAsync();
                return;
            }
        CheckBox checkBox = new CheckBox()
        {
            Foreground = new SolidColorBrush(Colors.White),
            Content = newTagTextBox.Text,
            IsChecked = true
        };
        tagCheckBoxes.Add(checkBox);
        TagsPanel.Children.Add(checkBox);
        newTagTextBox.Text = string.Empty;
    }

    private void LinkTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ValidateLink(LinkTextBox.Text) == true)
            MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        UpdateAllButton.IsEnabled = ValidateLink(LinkTextBox.Text);
        UpdateCoverButton.IsEnabled = ValidateLink(LinkTextBox.Text);
        UpdateDescriptionButton.IsEnabled = ValidateLink(LinkTextBox.Text);
        UpdateOngoingStatusButton.IsEnabled = ValidateLink(LinkTextBox.Text);
        UpdateContentRatingButton.IsEnabled = ValidateLink(LinkTextBox.Text);
        UpdateTagsButton.IsEnabled = ValidateLink(LinkTextBox.Text);
        OpenLinkButton.IsEnabled = ValidateLink(LinkTextBox.Text);
    }

    private void UpdateAllButton_Clicked(object sender, RoutedEventArgs e)
    {
        UpdateDescriptionButton_Clicked(this, e);
        UpdateCoverButton_Clicked(this, e);
        UpdateOngoingStatusButton_Clicked(this, e);
        UpdateContentRatingButton_Clicked(this, e);
        UpdateTagsButton_Clicked(this, e);
    }

    private void AddNewTagPanel()
    {
        TextBox newTagTextBox = new TextBox()
        {
            AcceptsReturn = false,
            TextWrapping = TextWrapping.NoWrap,
            Width = 160,
            Watermark = "Custom tag",
            FontSize = 15
        };
        Button addNewTagButton = new Button()
        {
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Content = "Add tag"
        };
        addNewTagButton.Click += NewTagButton_Clicked;
        StackPanel newTagPanel = new StackPanel()
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal
        };
        newTagPanel.Children.Add(newTagTextBox);
        newTagPanel.Children.Add(addNewTagButton);
        TagsPanel.Children.Add(newTagPanel);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        mangaList[passIndex].Description = DescriptionTextBox.Text;
        mangaList[passIndex].FileLastChapter = (decimal)LastChapterNumeric.Value;
        if (OngoingStatusComboBox.SelectedIndex != -1)
            mangaList[passIndex].OngoingStatus = ongoingStatuses[OngoingStatusComboBox.SelectedIndex];
        if (ValidateLink(LinkTextBox.Text) == true)
            mangaList[passIndex].ID = LinkTextBox.Text.Split('/')[4];
        else
            mangaList[passIndex].ID = string.Empty;
        if (ContentRatingComboBox.SelectedIndex != -1)
            mangaList[passIndex].ContentRating = contentRatings[ContentRatingComboBox.SelectedIndex];
        mangaList[passIndex].Tags.Clear();
        foreach (CheckBox checkBox in tagCheckBoxes)
            if (checkBox.IsChecked == true)
                mangaList[passIndex].Tags.Add(checkBox.Content.ToString());

        base.OnClosing(e);
    }
}