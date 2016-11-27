using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgodaApiExercise.Core.RateLimit
{
    public interface IBlackList
    {
        bool IsBlackListed(string key);
        void Add(string key);
    }
}
