using System.Collections.Generic;

namespace AgodaApiExercise.Core
{
    public interface ICsvLoader
    {
        IEnumerable<string> ReadLines(string filePath);
    }
}