using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace AgodaApiExercise.Core.RateLimit
{
    public sealed class RateLimiter:IRateLimiter
    {
        private static object syncRoot = new Object();
        private int _period;
        private int _requestThreshold;

        public RateLimiter(int period, int requestThreshold)
        {
            _period = period;
            _requestThreshold = requestThreshold;
        }

        public bool IsWitinLimit(string key)
        {
            lock (syncRoot)
            {
                return (AccessCountForCurrentPeriod(key) <= _requestThreshold);
            }          
        }

        private List<DateTime> AccessTimeStamps(string key)
        {
            if (MemoryCache.Default[key] == null)
            {
                return new List<DateTime>();
            }
            return (List<DateTime>)MemoryCache.Default[key];
        }

        private void UpdateAccessTimeStamps(string key, List<DateTime> timestamps)
        {
            MemoryCache.Default.Set(key, timestamps, DateTime.Now.AddSeconds(_period));
        }

        private int AccessCountForCurrentPeriod(string key)
        {
            var currentTime = DateTime.Now;
            var timestamps = AccessTimeStamps(key);
            // Remove expired timestamps
            timestamps.RemoveAll(x => x < currentTime.AddSeconds(-_period));
            // Update cache with current period timestamps
            timestamps.Add(currentTime);
            UpdateAccessTimeStamps(key, timestamps);
            return timestamps.Count;
        }
    }
}