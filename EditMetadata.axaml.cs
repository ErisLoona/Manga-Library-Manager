using System;
using Avalonia.Controls;
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
using MsBox.Avalonia.Enums;

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
                if (Path.GetExtension(mangaList[passIndex].Path).ToLower() == ".epub")
                {
                    foreach (ZipArchiveEntry entry in manga.Entries)
                        if (entry.Name.ToLower() == "cover.jpg" || entry.Name.ToLower() == "cover.jpeg" || entry.Name.ToLower() == "cover.png" || entry.Name.ToLower() == "cover.webp")
                        {
                            MemoryStream stream = new MemoryStream();
                            entry.Open().CopyTo(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            Dispatcher.UIThread.Post(() =>
                            {
                                CurrentCoverImage.Source = new Bitmap(stream);
                                UpdateCoverButton.IsEnabled = true;
                            });
                            return;
                        }
                }
                else if (Path.GetExtension(mangaList[passIndex].Path).ToLower() == ".cbz")
                {
                    MemoryStream stream = new MemoryStream();
                    manga.Entries.First().Open().CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Dispatcher.UIThread.Post(() =>
                    {
                        CurrentCoverImage.Source = new Bitmap(stream);
                        UpdateCoverButton.IsEnabled = true;
                    });
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

    private string coverPath = string.Empty;
    private bool updatingCover = false;
    private async void ConfirmButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (await MessageBoxManager.GetMessageBoxStandard("Confirmation", "The old cover cannot be recovered.\nThis process may take a while.\nAre you sure you want to proceed?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync() == MsBox.Avalonia.Enums.ButtonResult.No)
            return;

        updatingCover = true;
        string tempPath, mangaPath;
        if (Path.IsPathRooted(mangaList[passIndex].Path) == true)
            mangaPath = mangaList[passIndex].Path;
        else
            mangaPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), mangaList[passIndex].Path);
        int j = 1;
        try
        {
            while (Directory.Exists(Path.Combine(Path.GetDirectoryName(mangaPath), $"Manga Extraction {j}")) == true)
                j++;
            Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(mangaPath), $"Manga Extraction {j}"));
        }
        catch
        {
            updatingCover = false;
            await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not create a file!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            return;
        }
        tempPath = Path.Combine(Path.GetDirectoryName(mangaPath), $"Manga Extraction {j}");
        try
        {
            CoverLabel.Text = "Extracting Manga...";
            await Task.Run(() => { ZipFile.ExtractToDirectory(mangaPath, tempPath); });
        }
        catch
        {
            updatingCover = false;
            CoverLabel.Text = "Cover";
            try
            {
                Directory.Delete(tempPath, true);
            } catch { }
            await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not Extract Manga!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            return;
        }

        if (Path.GetExtension(mangaPath).ToLower() == ".epub")
        {
            bool found = false;
            foreach (string file in Directory.GetFiles(tempPath))
                if (Path.GetFileName(file).ToLower() == "cover.jpg" || Path.GetFileName(file).ToLower() == "cover.jpeg" || Path.GetFileName(file).ToLower() == "cover.png" || Path.GetFileName(file).ToLower() == "cover.webp")
                {
                    coverPath = file;
                    found = true;
                    break;
                }
            if (found == false)
                foreach (string file in Directory.GetFiles(Path.Combine(tempPath, "OEBPS", "OEBPS")))
                    if (Path.GetFileName(file).ToLower() == "cover.jpg" || Path.GetFileName(file).ToLower() == "cover.jpeg" || Path.GetFileName(file).ToLower() == "cover.png" || Path.GetFileName(file).ToLower() == "cover.webp")
                    {
                        coverPath = file;
                        found = true;
                        break;
                    }
            if (found == false)
                FindMeFiles(tempPath);
            try
            {
                File.Delete(coverPath);
            }
            catch
            {
                updatingCover = false;
                CoverLabel.Text = "Cover";
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch { }
                await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not delete the existing cover file!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                return;
            }
            try
            {
                newCover.Save(coverPath);
            }
            catch
            {
                updatingCover = false;
                CoverLabel.Text = "Cover";
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch { }
                await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not save the new cover file!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                return;
            }
        }
        else if (Path.GetExtension(mangaPath).ToLower() == ".cbz")
        {
            string cbzPrefix = string.Empty;
            string[] files = Directory.GetFiles(tempPath);
            j = 0;
            while (Path.GetFileNameWithoutExtension(files[0]).Substring(j, 1) == Path.GetFileNameWithoutExtension(files.Last()).Substring(j, 1))
            {
                cbzPrefix += Path.GetFileNameWithoutExtension(files[0]).Substring(j, 1);
                j++;
            }
            try
            {
                newCover.Save(Path.Combine(tempPath, cbzPrefix + 0.ToString("D" + (Path.GetFileNameWithoutExtension(files[0]).Length - cbzPrefix.Length).ToString())) + Path.GetExtension(files[0]));
            }
            catch
            {
                updatingCover = false;
                CoverLabel.Text = "Cover";
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch { }
                await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not save the new cover file!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                return;
            }
        }
        try
        {
            CoverLabel.Text = "Archiving Manga...";
            File.Delete(mangaPath);
            await Task.Run(() => { ZipFile.CreateFromDirectory(tempPath, mangaPath); });
            Directory.Delete(tempPath, true);
        }
        catch
        {
            updatingCover = false;
            CoverLabel.Text = "Cover";
            try
            {
                Directory.Delete(tempPath, true);
            }
            catch { }
            await MessageBoxManager.GetMessageBoxStandard("Write error", "Could not archive the manga!", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            return;
        }

        updatingCover = false;
        CoverLabel.Text = "Cover";
        ConfirmButton.IsEnabled = false;
        NewCoverImage.Source = null;
        CurrentCoverImage.Source = newCover;
    }

    private void FindMeFiles(string path)
    {
        try
        {
            string[] entries = Directory.GetFiles(path);
            foreach (string file in entries)
                if (Path.GetFileName(file).ToLower() == "cover.jpg" || Path.GetFileName(file).ToLower() == "cover.jpeg" || Path.GetFileName(file).ToLower() == "cover.png" || Path.GetFileName(file).ToLower() == "cover.webp")
                {
                    coverPath = file;
                    return;
                }
            string[] subdirs = Directory.GetDirectories(path);
            foreach (string subdir in subdirs)
                FindMeFiles(subdir);
        } catch { }
    }

    private void UpdateCoverButton_Clicked(object sender, RoutedEventArgs e)
    {
        _ = Task.Run(async () => {
            MemoryStream stream = new MemoryStream();
            MDLGetData.GetCover().CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            if (apiError == true)
            {
                await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
                apiError = false;
                return;
            }
            newCover = new Bitmap(stream);
            Dispatcher.UIThread.Post(() => {
                NewCoverImage.Source = newCover;
                ConfirmButton.IsEnabled = true;
            });
        });
    }

    private async void UpdateDescriptionButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        MDLGetData.GetDescription();
        if (apiError == true)
        {
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
            apiError = false;
            return;
        }
        DescriptionTextBox.Text = MDLGetData.GetDescription();
    }

    private async void UpdateOngoingStatusButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        MDLGetData.GetStatus();
        if (apiError == true)
        {
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
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

    private async void UpdateContentRatingButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        MDLGetData.GetContentRating();
        if (apiError == true)
        {
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
            apiError = false;
            return;
        }
        ContentRatingComboBox.SelectedIndex = Array.IndexOf(contentRatings, MDLGetData.GetContentRating().Substring(0, 1).ToUpper() + MDLGetData.GetContentRating().Substring(1));
    }

    private async void UpdateTagsButton_Clicked(object sender, RoutedEventArgs e)
    {
        MDLParameters.MangaID = LinkTextBox.Text.Split('/')[4];
        List<string> newTags = MDLGetData.GetTags();
        if (apiError == true)
        {
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.\nPlease double-check the Manga link and try again later.", ButtonEnum.Ok).ShowAsync();
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
        if (CurrentCoverImage != null)
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
        if (UpdateCoverButton.IsEnabled == true)
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
        if (updatingCover == true)
        {
            e.Cancel = true;
            return;
        }

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