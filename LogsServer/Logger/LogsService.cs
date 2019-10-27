using Gestion.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;

namespace LogsServer.Logger
{
    public class LogsService
    {
        private static LogsService Instance { get; set; }
        private string QueuePath { get; }

        public static LogsService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new LogsService();
            }

            return Instance;
        }

        private LogsService() {
            QueuePath = Config.GetValue<string>("msmq");
        }

        public LogQuery QueryLogs()
        {
            return new LogQuery(GetAll());
        }

        public IEnumerable<LogEntry> GetAll()
        {
            MessageQueue queue = new MessageQueue(QueuePath);
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogEntry) });

            var msgs = queue.GetAllMessages();
            return msgs.Select(msg => (LogEntry)msg.Body);
        }
    }
}