using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace AgodaApiExercise.Core.RateLimit
{
    public class BlackList : IBlackList
    {
        private readonly int _duration;
        public const string _prefix = "blklst_";

        public BlackList(int duration)
        {
            _duration = duration;
        }

        public bool IsBlackListed(string key)
        {
            var value = MemoryCache.Default[_prefix + key];
            return (value != null);
        }

        public void Add(string key)
        {
            MemoryCache.Default.Set(_prefix + key, true, DateTime.Now.AddSeconds(_duration));
        }
    }
}