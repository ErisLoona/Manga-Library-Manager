using System;
using System.Threading;
using System.Diagnostics;

namespace MangaDex_Library
{
    internal static class RateLimiter
    {
        private static Stopwatch api = new Stopwatch(), home = new Stopwatch();
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
            apiCalls++;
            if (apiCalls > 5)
            {
                if (api.ElapsedMilliseconds < 1000)
                    Thread.Sleep(1000 - Convert.ToInt32(api.ElapsedMilliseconds));
                apiCalls = 1;
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
            homeCalls++;
            if (homeCalls > 40)
            {
                if (home.ElapsedMilliseconds < 60000)
                    Thread.Sleep(60000 - Convert.ToInt32(home.ElapsedMilliseconds));
                homeCalls = 1;
                home.Reset();
            }
            home.Start();
        }
    }
}
