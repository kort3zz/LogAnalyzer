using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzer
{
    public class LogParser
    {
        private static readonly string[] ValidLevels =
        {
            "TRACE",
            "DEBUG",
            "INFO",
            "WARN",
            "ERROR"
        };

        public bool TryParse(string line, out LogEntry? entry, out string error)
        {
            entry = null;
            error = string.Empty;

            string[] parts = line.Split('|');

            if (parts.Length != 5)
            {
                error = "Неверное количествно полей";
                return false;
            }

            string timestampText = parts[0].Trim();
            string level = parts[1].Trim();
            string source = parts[2].Trim();
            string eventCode = parts[3].Trim();
            string message = parts[4].Trim();
            
            if (string.IsNullOrWhiteSpace(timestampText) || string.IsNullOrWhiteSpace(level) || string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(eventCode) || string.IsNullOrWhiteSpace(message))
            {
                error = "Одно или несколько полей пустые";
                return false;
            }

            if (!DateTime.TryParse(timestampText, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime timestamp))
            {
                error = "Неверный timestamp";
                return false;
            }

            if (!ValidLevels.Contains(level))
            {
                error = $"Недопустимый уровень {level}";
                return false;
            }
            entry = new LogEntry
            {
                Timestamp = timestamp,
                Level = level,
                Source = source,
                EventCode = eventCode,
                Message = message
            };
            return true;
        }
    }
}
