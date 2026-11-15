using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzer
{
    public class LogFilter
    {
        public bool Matches(LogEntry entry, DateTime? from = null, DateTime? to = null, string? level = null, string? source = null)
        {
            if (from.HasValue && entry.Timestamp < from.Value)
            {
                return false;
            }   

            if (to.HasValue && entry.Timestamp >= to.Value)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(level) && !entry.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(source) && !entry.Source.Contains(source, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

        return true;
        }
    }
}
