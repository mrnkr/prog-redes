using Gestion.Services;
using Gestion.Common;
using System;
using System.Messaging;

namespace Gestion.Srv.Logger
{
    public class MessageQueueLogger : ILogger
    {
        private string QueuePath { get; }

        public MessageQueueLogger()
        {
            QueuePath = Config.GetValue<string>("msmq");
            CreateQueueIfNotExists();
        }

        private void CreateQueueIfNotExists()
        {
            try
            {
                if (!MessageQueue.Exists(QueuePath))
                {
                    MessageQueue.Create(QueuePath);
                }
            }
            catch (MessageQueueException e)
            {
                Console.WriteLine($"Error creating MessageQueue: {e.Message}");
            }
        }

        public void Log(EventType e, string description)
        {
            var log = LogEntry.WithDescription(e, description);
            SendLogToMessageQueue(log);
        }

        private void SendLogToMessageQueue(LogEntry log)
        {
            try
            {
                MessageQueue queue = new MessageQueue(QueuePath);
                Message msg = new Message(log);
                queue.Send(msg);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Error sending LogEntry to MessageQueue: {e.Message}");
            }
        }
    }
}
