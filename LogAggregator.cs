using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LogAnalyzer
{
    public class LogAggregator
    {
        private readonly Dictionary<string, int> _levels = new();
        private readonly Dictionary<string, int> _sources = new();
        private readonly Dictionary<string, int> _errorCodes = new();
        public IReadOnlyDictionary<string, int> Levels => _levels;
        public IReadOnlyDictionary<string, int> Sources => _sources;

        public int TotalEvents { get; private set; }

        public void Add(LogEntry entry)
        {
            TotalEvents++;

            Increment(_levels, entry.Level);

            Increment(_sources, entry.Source);

            if (entry.Level == "ERROR")
            {
                Increment(_errorCodes, entry.EventCode);
            }
        }
        public void Increment(Dictionary<string, int> counts, string key)
        {
            if (counts.ContainsKey(key))
            {
                counts[key]++;
            }
            else
            {
                counts[key] = 1;
            }

        }
        public IEnumerable<KeyValuePair<string, int>> GetTopErrorCodes(int count)
        {
            return _errorCodes.OrderByDescending(item => item.Value).ThenBy(item => item.Key).Take(count);

        }
    }
}
