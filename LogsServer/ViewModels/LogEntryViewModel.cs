using Gestion.Common;
using Gestion.Services;
using System;
using System.Globalization;

namespace LogsServer.ViewModels
{
    public class LogEntryViewModel
    {
        public string id { get; set; }
        public string eventType { get; set; }
        public string description { get; set; }
        public string timestamp { get; set; }

        public static LogEntryViewModel FromEntity(LogEntry log)
        {
            return new LogEntryViewModel
            {
                id = log.Id,
                eventType = Enum.GetName(typeof(EventType), log.EventType),
                description = log.Description,
                timestamp = log.Timestamp.ToString("s", CultureInfo.InvariantCulture)
            };
        }
    }
}