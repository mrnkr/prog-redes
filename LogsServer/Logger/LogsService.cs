using Gestion.Common;
using System;
using System.Collections.Generic;
using System.Messaging;
using System.Threading;

namespace LogsServer.Logger
{
    public class LogsService
    {
        private static LogsService Instance { get; set; }
        private string QueuePath { get; }
        private ICollection<LogEntry> Logs { get; }

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
            Logs = new List<LogEntry>();
        }

        public LogQuery QueryLogs()
        {
            return new LogQuery(Logs);
        }

        public void ReceiveMessages()
        {
            new Thread(() =>
            {
                MessageQueue queue = new MessageQueue(QueuePath);
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogEntry) });

                while (true)
                {
                    var log = queue.Receive();
                    Logs.Add((LogEntry)log.Body);
                }
            }).Start();
        }
    }
}