using Gestion.Services;
using System;

namespace Gestion.Common
{
    public class LogEntry
    {
        public string Id { get; set; }
        public EventType EventType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        private LogEntry()
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
        }

        public static LogEntry WithDescription(EventType type, string description)
        {
            return new LogEntry
            {
                EventType = type,
                Description = description
            };
        }
    }
}
