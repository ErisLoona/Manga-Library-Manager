using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

namespace MangaDex_Library
{
    public static class MDLGetData
    {
        public static event EventHandler ApiRequestFailed, DownloadError;
        private const string apiLink = "https://api.mangadex.org/manga/";
        private static HttpClient client = MDLParameters.client;
        private static string oldIDManga = null, oldIDFeed = null, oldNameSearch = null;
        private static JObject manga = null, search = null;
        private static List<JObject> feed = new List<JObject>();

        //Manga API Data
        private static List<string> titles = new List<string>(), tags = new List<string>(), authors = new List<string>(), artists = new List<string>();
        private static string description, originalLanguage, publicationDemographic, status, contentRating, creationDate, coverFileName;

        //Feed API Data
        private static List<string> chapterIDs = new List<string>(), chapterTitles = new List<string>(), chapterGroups = new List<string>();
        private static List<int> chapterPages = new List<int>();
        private static List<decimal> chapterNumbers = new List<decimal>();
        private static decimal lastChapter;

        //Downloads
        public static Stream pageImage;
        public static bool doReports = true;

        public static void ForceReset()
        {
            manga = null;
            oldIDManga = null;
            feed = null;
            oldIDFeed = null;
            search = null;
            oldNameSearch = null;
        }

        #region Manga API Data

        private static void UpdateManga()
        {
            if (MDLParameters.MangaID == null)
                throw new Exception("No Manga ID set.");
            if (MDLParameters.AgentSet == false)
                throw new Exception("No User Agent set for the HttpClient.");
            RateLimiter.ApiCall();
            try
            {
                manga = JObject.Parse(client.GetStringAsync($"{apiLink}{MDLParameters.MangaID}?includes[]=cover_art&includes[]=author&includes[]=artist").Result);
            }
            catch
            {
                ApiRequestFailed?.Invoke(null, EventArgs.Empty);
                return;
            }
            oldIDManga = MDLParameters.MangaID;
            description = originalLanguage = publicationDemographic = status = contentRating = creationDate = coverFileName = null;
            titles.Clear();
            tags.Clear();
            authors.Clear();
            artists.Clear();
            JToken data = manga.SelectToken("data");
            bool found = false;
            titles.Add(data.SelectToken("attributes.title.en").Value<string>());
            foreach (JToken subTitle in data.SelectToken("attributes.altTitles"))
            {
                try
                {
                    titles.Add(subTitle.SelectToken(MDLParameters.Language).Value<string>());
                    found = true;
                }
                catch { }
            }
            if (found == false)
                foreach (JToken subTitle in data.SelectToken("attributes.altTitles"))
                    try
                    {
                        titles.Add(subTitle.SelectToken("en").Value<string>());
                    }
                    catch { }
            try
            {
                description = data.SelectToken("attributes.description").SelectToken(MDLParameters.Language).Value<string>();
            }
            catch
            {
                description = data.SelectToken("attributes.description.en").Value<string>();
            }
            originalLanguage = data.SelectToken("attributes.originalLanguage").Value<string>();
            publicationDemographic = data.SelectToken("attributes.publicationDemographic").Value<string>();
            status = data.SelectToken("attributes.status").Value<string>();
            contentRating = data.SelectToken("attributes.contentRating").Value<string>();
            foreach (JToken tag in data.SelectToken("attributes.tags"))
            {
                string groupTemp = tag.SelectToken("attributes.group").Value<string>();
                if (groupTemp == "genre" || groupTemp == "theme")
                    try
                    {
                        tags.Add(tag.SelectToken("attributes.name").SelectToken(MDLParameters.Language).Value<string>());
                    }
                    catch
                    {
                        tags.Add(tag.SelectToken("attributes.name.en").Value<string>());
                    }
            }
            creationDate = data.SelectToken("attributes.createdAt").Value<string>();
            foreach (JToken rel in data.SelectToken("relationships"))
            {
                string type = rel.SelectToken("type").Value<string>();
                if (type == "cover_art")
                    coverFileName = rel.SelectToken("attributes").SelectToken("fileName").Value<string>();
                else if (type == "author")
                    authors.Add(rel.SelectToken("attributes").SelectToken("name").Value<string>());
                else if (type == "artist")
                    artists.Add(rel.SelectToken("attributes").SelectToken("name").Value<string>());
            }
        }

        public static List<string> GetTitles()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return titles;
        }

        public static string GetDescription()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return description;
        }

        public static string GetOriginalLanguage()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return originalLanguage;
        }

        public static string GetPublicationDemographic()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return publicationDemographic;
        }

        public static string GetStatus()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return status;
        }

        public static string GetContentRating()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return contentRating;
        }

        public static List<string> GetTags()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return tags;
        }

        public static string GetCreationDate()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return creationDate;
        }

        public static List<string> GetAuthors()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return authors;
        }

        public static List<string> GetArtists()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            return artists;
        }

        public static Stream GetCover()
        {
            if (oldIDManga != MDLParameters.MangaID || manga == null)
                UpdateManga();
            try
            {
                RateLimiter.ApiCall();
                return MDLParameters.client.GetStreamAsync($"https://uploads.mangadex.org/covers/{MDLParameters.MangaID}/{coverFileName}").Result;
            }
            catch
            {
                ApiRequestFailed?.Invoke(null, EventArgs.Empty);
                return null;
            }
        }

        #endregion

        #region Feed API Data

        private static void UpdateFeed()
        {
            if (MDLParameters.MangaID == null)
                throw new Exception("No Manga ID set.");
            if (MDLParameters.AgentSet == false)
                throw new Exception("No User Agent set for the HttpClient.");
            List<JObject> pages = new List<JObject>();
            RateLimiter.ApiCall();
            try
            {
                pages.Add(JObject.Parse(client.GetStringAsync($"{apiLink}{MDLParameters.MangaID}/feed?translatedLanguage[]={MDLParameters.Language}&limit=100&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&contentRating[]=pornographic&includes[]=scanlation_group").Result));
                int nrPages = pages[0].SelectToken("total").Value<int>();
                if (nrPages > 100)
                {
                    int passes = nrPages / 100;
                    while (passes > 0)
                    {
                        RateLimiter.ApiCall();
                        pages.Add(JObject.Parse(client.GetStringAsync($"{apiLink}{MDLParameters.MangaID}/feed?translatedLanguage[]={MDLParameters.Language}&limit=100&offset={passes * 100}&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&contentRating[]=pornographic&includes[]=scanlation_group").Result));
                        passes--;
                    }
                }
                feed = pages;
            }
            catch
            {
                ApiRequestFailed?.Invoke(null, EventArgs.Empty);
                return;
            }
            oldIDFeed = MDLParameters.MangaID;
            lastChapter = 0M;
            chapterIDs.Clear();
            chapterTitles.Clear();
            chapterPages.Clear();
            chapterNumbers.Clear();
            chapterGroups.Clear();
            foreach (JObject page in feed)
                foreach (JToken chapter in page.SelectToken("data"))
                {
                    chapterIDs.Add(chapter.SelectToken("id").Value<string>());
                    chapterNumbers.Add(Convert.ToDecimal(chapter.SelectToken("attributes.chapter").Value<string>(), new CultureInfo("en-US")));
                    chapterTitles.Add(chapter.SelectToken("attributes.title").Value<string>());
                    chapterPages.Add(chapter.SelectToken("attributes.pages").Value<int>());
                    bool groupFound = false;
                    foreach (JToken group in chapter.SelectToken("relationships"))
                        if (group.SelectToken("type").Value<string>() == "scanlation_group")
                        {
                            groupFound = true;
                            chapterGroups.Add(group.SelectToken("attributes.name").Value<string>());
                            break;
                        }
                    if (groupFound == false)
                        chapterGroups.Add("Anonymous (No Group)");
                }
            bool doneGoneDidThisOne = true;
            for (int j = 1; (j <= chapterNumbers.Count - 1) && (doneGoneDidThisOne == true); j++)
            {
                doneGoneDidThisOne = false;
                for (int i = 0; i < chapterNumbers.Count - 1; i++)
                    if (chapterNumbers[i] > chapterNumbers[i + 1])
                    {
                        doneGoneDidThisOne = true;
                        string str;
                        decimal dec;
                        int inte;

                        str = chapterIDs[i + 1];
                        chapterIDs[i + 1] = chapterIDs[i];
                        chapterIDs[i] = str;

                        str = chapterTitles[i + 1];
                        chapterTitles[i + 1] = chapterTitles[i];
                        chapterTitles[i] = str;

                        inte = chapterPages[i + 1];
                        chapterPages[i + 1] = chapterPages[i];
                        chapterPages[i] = inte;

                        dec = chapterNumbers[i + 1];
                        chapterNumbers[i + 1] = chapterNumbers[i];
                        chapterNumbers[i] = dec;

                        str = chapterGroups[i + 1];
                        chapterGroups[i + 1] = chapterGroups[i];
                        chapterGroups[i] = str;
                    }
            }
            lastChapter = chapterNumbers[chapterNumbers.Count - 1];
        }

        public static List<string> GetChapterIDs()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            return chapterIDs;
        }

        public static List<decimal> GetChapterNumbers()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            return chapterNumbers;
        }

        public static List<string> GetChapterTitles()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            return chapterTitles;
        }

        public static List<int> GetChapterNrPages()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            return chapterPages;
        }

        public static List<string> GetChapterGroups()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            return chapterGroups;
        }

        public static decimal GetLastChapter()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            return lastChapter;
        }

        public static List<int> GetCuratedChapterIndexes()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            List<int> curatedIndexes = new List<int>();
            Dictionary<string, int> groupAppearances = new Dictionary<string, int>(), groupIndexes = new Dictionary<string, int>();
            for (int i = 0; i < chapterNumbers.Count; i++)
            {
                if (groupAppearances.ContainsKey(chapterGroups[i]))
                    groupAppearances[chapterGroups[i]]++;
                else
                    groupAppearances[chapterGroups[i]] = 1;
                groupIndexes[chapterGroups[i]] = i;
                if (i != chapterNumbers.Count - 1 && chapterNumbers[i] == chapterNumbers[i + 1])
                    continue;
                if (groupIndexes.Count == 1)
                {
                    curatedIndexes.Add(i);
                    groupIndexes.Clear();
                    continue;
                }
                groupAppearances = groupAppearances.OrderByDescending(keyValuePair => keyValuePair.Value).ToDictionary();
                bool found = false;
                foreach (KeyValuePair<string, int> group in groupAppearances)
                    if (groupIndexes.ContainsKey(group.Key) && group.Key != "Anonymous (No Group)")
                    {
                        found = true;
                        curatedIndexes.Add(groupIndexes[group.Key]);
                        break;
                    }
                if (found == false)
                    if (groupIndexes.ContainsKey("Anonymous (No Group)"))
                        curatedIndexes.Add(groupIndexes["Anonymous (No Group)"]);
                    else
                        curatedIndexes.Add(groupIndexes.First().Value);
                groupIndexes.Clear();
            }
            return curatedIndexes;
        }

        public static List<int> GetAltCuratedChapterIndexes()
        {
            if (oldIDFeed != MDLParameters.MangaID || feed == null)
                UpdateFeed();
            List<int> curatedIndexes = new List<int>();
            Dictionary<string, int> groupAppearances = new Dictionary<string, int>(), groupIndexes = new Dictionary<string, int>();
            foreach (string group in chapterGroups)
                if (groupAppearances.ContainsKey(group))
                    groupAppearances[group]++;
                else
                    groupAppearances[group] = 1;
            groupAppearances = groupAppearances.OrderByDescending(keyValuePair => keyValuePair.Value).ToDictionary();
            for (int i = 0; i < chapterNumbers.Count; i++)
            {
                groupIndexes[chapterGroups[i]] = i;
                if (i != chapterNumbers.Count - 1 && chapterNumbers[i] == chapterNumbers[i + 1])
                    continue;
                if (groupIndexes.Count == 1)
                {
                    curatedIndexes.Add(i);
                    groupIndexes.Clear();
                    continue;
                }
                bool found = false;
                foreach (KeyValuePair<string, int> group in groupAppearances)
                    if (groupIndexes.ContainsKey(group.Key) && group.Key != "Anonymous (No Group)")
                    {
                        found = true;
                        curatedIndexes.Add(groupIndexes[group.Key]);
                        break;
                    }
                if (found == false)
                    if (groupIndexes.ContainsKey("Anonymous (No Group)"))
                        curatedIndexes.Add(groupIndexes["Anonymous (No Group)"]);
                    else
                        curatedIndexes.Add(groupIndexes.First().Value);
                groupIndexes.Clear();
            }
            return curatedIndexes;
        }

        #endregion

        public static Dictionary<string, string> GetSearchResults(string mangaTitle)
        {
            if (MDLParameters.AgentSet == false)
                throw new Exception("No User Agent set for the HttpClient.");
            Dictionary<string, string> results = new Dictionary<string, string>();
            if (mangaTitle != oldNameSearch)
            {
                if (MDLParameters.AgentSet == false)
                    throw new Exception("No User Agent set for the HttpClient.");
                mangaTitle = mangaTitle.ToLower();
                mangaTitle = MDLParameters.titleSanitationRegex.Replace(mangaTitle, "");
                RateLimiter.ApiCall();
                try
                {
                    search = JObject.Parse(client.GetStringAsync($"{apiLink}?title={mangaTitle}&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&contentRating[]=pornographic&order[relevance]=desc").Result);
                }
                catch
                {
                    ApiRequestFailed?.Invoke(null, EventArgs.Empty);
                    return null;
                }
                oldNameSearch = mangaTitle;
            }
            foreach (JToken result in search.SelectToken("data"))
                results[result.SelectToken("attributes.title.en").Value<string>()] = result.SelectToken("attributes.id").Value<string>();
            return results;
        }

        #region Downloads

        public static List<string> GetPageLinks(string chapterID)
        {
            if (MDLParameters.AgentSet == false)
                throw new Exception("No User Agent set for the HttpClient.");
            List<string> links = new List<string>();
            JObject getLinksResponse = new JObject();
            RateLimiter.HomeCall();
            try
            {
                getLinksResponse = JObject.Parse(MDLParameters.client.GetStringAsync($"https://api.mangadex.org/at-home/server/{chapterID}").Result);
            }
            catch
            {
                ApiRequestFailed?.Invoke(null, EventArgs.Empty);
                return null;
            }
            string url;
            doReports = true;
            if (getLinksResponse.SelectToken("baseUrl").Value<string>().Contains("mangadex.org"))
                doReports = false;
            if (MDLParameters.DataSaving == false)
            {
                url = $"{getLinksResponse.SelectToken("baseUrl").Value<string>()}/data/{getLinksResponse.SelectToken("chapter.hash").Value<string>()}/";
                foreach (JToken pageFileName in getLinksResponse.SelectToken("chapter.data"))
                    links.Add($"{url}{pageFileName.Value<string>()}");
            }
            else
            {
                url = $"{getLinksResponse.SelectToken("baseUrl").Value<string>()}/data-saver/{getLinksResponse.SelectToken("chapter.hash").Value<string>()}/";
                foreach (JToken pageFileName in getLinksResponse.SelectToken("chapter.dataSaver"))
                    links.Add($"{url}{pageFileName.Value<string>()}");
            }
            return links;
        }

        public static Stream GetPageImage(string link)
        {
            if (MDLParameters.AgentSet == false)
                throw new Exception("No User Agent set for the HttpClient.");
            Stopwatch dlTime = new Stopwatch();
            TimeSpan span = new TimeSpan();
            HttpResponseMessage response = new HttpResponseMessage();
            bool success = true, cached = false;
            try
            {
                dlTime.Start();
                response = MDLParameters.client.GetAsync(link).Result;
                dlTime.Stop();
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                dlTime.Stop();
                success = false;
                DownloadError?.Invoke(null, EventArgs.Empty);
            }
            if (doReports == true)
            {
                span = dlTime.Elapsed;
                if (response.Headers.TryGetValues("X-Cache", out IEnumerable<string> headers) == true)
                    if (headers.First().StartsWith("HIT") == true)
                        cached = true;
                Report(link, success, cached, response.Content.ReadAsStream().Length, span.Milliseconds, !success);
            }
            dlTime.Reset();
            return response.Content.ReadAsStream();
        }

        private static async void Report(string link, bool success, bool cached, long bytes, long duration, bool wait)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values["url"] = link;
            values["success"] = success;
            values["cached"] = cached;
            values["bytes"] = bytes;
            values["duration"] = duration;
            HttpContent payload = JsonContent.Create(values, mediaType: new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            try
            {
                if (wait == false)
                    _ = MDLParameters.client.PostAsync("https://api.mangadex.network/report", payload);
                else
                    await MDLParameters.client.PostAsync("https://api.mangadex.network/report", payload);
            }
            catch { }
        }

        #endregion
    }
}
