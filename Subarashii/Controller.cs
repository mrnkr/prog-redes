using System;
using System.Net.Sockets;
using Subarashii.Core.Exchangers;

namespace Subarashii.Core
{
    public abstract class Controller
    {
        protected Socket Socket { get; set; }
        protected string CurrentOperationCode { get; set; }

        public void SetContext(Socket sock, string code)
        {
            Socket = sock;
            CurrentOperationCode = code;
        }

        protected void Text(string payload)
        {
            if (Socket == null)
            {
                throw new InvalidOperationException();
            }

            var response = new MessageBuilder()
                .MarkAsResponse()
                .PutOperationCode(CurrentOperationCode)
                .PutPayload(payload)
                .Build();

            Sender.SendMessage(Socket, response);
        }

        protected void Object<T>(T payload) where T : class
        {
            if (Socket == null)
            {
                throw new InvalidOperationException();
            }

            var response = new MessageBuilder()
                .MarkAsResponse()
                .PutOperationCode(CurrentOperationCode)
                .PutPayload<T>(payload)
                .Build();

            Sender.SendMessage(Socket, response);
        }

        protected void File(string path)
        {
            if (Socket == null)
            {
                throw new InvalidOperationException();
            }

            var builder = new MessageBuilder()
                .MarkAsResponse()
                .PutOperationCode(CurrentOperationCode);

            Sender.SendFile(Socket, builder, path);
        }
    }
}
