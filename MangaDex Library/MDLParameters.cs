using System.Text.RegularExpressions;

namespace MangaDex_Library
{
    public static class MDLParameters
    {
        public static string MangaID = null, Language = "en";
        public static bool DataSaving = false;

        internal static HttpClient client = new HttpClient();
        private static bool setHeaders = false;
        internal static bool AgentSet = false;
        internal static Regex titleSanitationRegex = new Regex("[^a-zA-Z0-9 ]");

        public static void SetUserAgent(string userAgent)
        {
            if (setHeaders == false)
            {
                setHeaders = true;
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            AgentSet = true;
        }
    }
}
