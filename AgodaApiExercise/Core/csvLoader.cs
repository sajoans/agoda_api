using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgodaApiExercise.Core
{
    public class CsvLoader:ICsvLoader
    {
        public IEnumerable<string> ReadLines(string filePath)
        {
            var hotelsLines = System.IO.File.ReadAllLines(filePath).ToList();
            hotelsLines.RemoveAt(0); // remove header
            return hotelsLines;

        }
    }
}