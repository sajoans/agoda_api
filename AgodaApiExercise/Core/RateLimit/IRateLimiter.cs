using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgodaApiExercise.Core.RateLimit
{
    public interface IRateLimiter
    {
        bool IsWitinLimit(string key);
    }
}
