using System.Diagnostics;

namespace MangaDex_Library
{
    internal static class RateLimiter
    {
        private static Stopwatch api = new Stopwatch(), home = new Stopwatch();
        private static TimeSpan span;
        private static int apiCalls = 0, homeCalls = 0;

        internal static void ApiCall()
        {
            if (api.IsRunning == false)
            {
                apiCalls++;
                api.Start();
                return;
            }
            api.Stop();
            span = api.Elapsed;
            apiCalls++;
            if (apiCalls > 5)
            {
                if (span.Milliseconds < 1000)
                    Thread.Sleep(1000 - span.Milliseconds);
                apiCalls = 0;
                api.Reset();
            }
            api.Start();
        }

        internal static void HomeCall()
        {
            if (home.IsRunning == false)
            {
                homeCalls++;
                home.Start();
                return;
            }
            home.Stop();
            span = home.Elapsed;
            homeCalls++;
            if (homeCalls > 40)
            {
                if (span.Milliseconds < 60000)
                    Thread.Sleep(60000 - span.Milliseconds);
                homeCalls = 0;
                home.Reset();
            }
            home.Start();
        }
    }
}
