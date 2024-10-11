using System;
using System.Collections.Generic;
using System.Globalization;
using MangaDex_Library;
using System.Runtime.InteropServices;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Diagnostics;

namespace Manga_Manager
{
    internal class Manga
    {
        public string Title = string.Empty;
        public string Description = string.Empty;
        public string Path = string.Empty;
        public decimal FileLastChapter = 0M;
        public decimal OnlineLastChapter = 0M;
        public DateOnly LastChecked = new DateOnly(69, 1, 1);
        public string OngoingStatus = string.Empty;
        public bool CheckInBulk = false;
        public string ID = string.Empty;
        public string ContentRating = string.Empty;
        public List<string> Tags = new List<string>();
    }

    internal static class Globals
    {
        static Globals()
        {
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                if (languageDictionary.ContainsValue(culture.TwoLetterISOLanguageName) == false)
                    languageDictionary[culture.EnglishName] = culture.TwoLetterISOLanguageName;
            languageDictionary["Simplified Chinese"] = "zh";
            languageDictionary["Traditional Chinese"] = "zh-hk";
            languageDictionary["Brazilian Portugese"] = "pt-br";
            languageDictionary["Castilian Spanish"] = "es";
            languageDictionary["Latin American Spanish"] = "es-la";
            languageDictionary["Romanized Japanese"] = "ja-ro";
            languageDictionary["Romanized Korean"] = "ko-ro";
            languageDictionary["Romanized Chinese"] = "zh-ro";

            MDLGetData.ApiRequestFailed += MDLGetData_ApiRequestFailed;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                MDLParameters.SetUserAgent("Manga Library Manager for Windows by (github) ErisLoona");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                MDLParameters.SetUserAgent("Manga Library Manager for OSX by (github) ErisLoona");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                MDLParameters.SetUserAgent("Manga Library Manager for Linux by (github) ErisLoona");
            else
                MDLParameters.SetUserAgent("Manga Library Manager by (github) ErisLoona");
        }

        internal async static void MDLGetData_ApiRequestFailed(object sender, EventArgs e)
        {
            apiError = true;
            await MessageBoxManager.GetMessageBoxStandard("API error", "An error occurred while trying to contact the MangaDex API.", ButtonEnum.Ok).ShowAsync();
        }

        internal static async void OpenLink(string link)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start(new ProcessStartInfo("open", new string[] { link }));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start(new ProcessStartInfo("xdg-open", new string[] { link }));
            else
                await MessageBoxManager.GetMessageBoxStandard("Unknown OS", $"Couldn't identify your OS, what are you running? Open the link yourself lol:\n{link}", ButtonEnum.Ok).ShowAsync();
        }

        #region Variables

        internal static List<Manga> mangaList = new List<Manga>();

        #region Settings Globals
        internal static string selectedLanguage = "en";
        internal static bool noWarning = false, checkUpdates = false, hideJsonFile = false;
        internal static int downloaderLastUsedFormat = 0;
        #endregion

        internal static Dictionary<string, string> languageDictionary = new Dictionary<string, string>();
        internal static Dictionary<string, int> tagsUsage = new Dictionary<string, int>();

        internal static bool apiError = false;

        #endregion
    }
}
