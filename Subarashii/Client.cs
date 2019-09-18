using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Subarashii.Core.Exceptions;
using Subarashii.Core.Exchangers;

namespace Subarashii.Core
{
    public class Client : IDisposable
    {
        private int Port { get; set; }
        private Socket Socket { get; set; }
        private string Auth { get; set; }

        public Client(int port)
        {
            Port = port;
            Auth = "------";
        }

        public void Connect(Action onConnect)
        {
            try
            {
                Socket = SetupConnection();
                Console.WriteLine("Connected to server on Ricardo Port {0}", Port);
                onConnect();
            }
            catch
            {

            }
        }

        private Socket SetupConnection()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);

            return sender;
        }

        public void Authenticate(string auth)
        {
            Auth = auth;
        }

        public void AnnulAuth()
        {
            Auth = "------";
        }

        public void Send(string code, string msg)
        {
            var request = new MessageBuilder()
                .PutOperationCode(code)
                .PutAuthInfo(Auth)
                .PutPayload(msg)
                .Build();

            Sender.SendMessage(Socket, request);
        }

        public void Send<T>(string code, T payload) where T : class
        {
            var request = new MessageBuilder()
                .PutOperationCode(code)
                .PutAuthInfo(Auth)
                .PutPayload(payload)
                .Build();

            Sender.SendMessage(Socket, request);
        }

        public void SendFile(string code, string path)
        {
            var builder = new MessageBuilder()
                .PutOperationCode(code)
                .PutAuthInfo(Auth);

            Sender.SendFile(Socket, builder, path);
        }

        public string Recieve()
        {
            DecodedMessage<byte[]> response = Reciever.RecieveMessage(Socket);
            return MessageDecoder.DecodePayload(response.Payload);
        }

        public T Recieve<T>() where T : class
        {
            DecodedMessage<byte[]> response = Reciever.RecieveMessage(Socket);
            return MessageDecoder.DecodePayload<T>(response.Payload);
        }

        public string RecieveFile()
        {
            DecodedMessage<byte[]> response = Reciever.RecieveFile(Socket, true);
            return MessageDecoder.DecodePayload(response.Payload);
        }

        public void ListenToNotifications(Action<string> next)
        {
            if (!IsAuthenticated())
            {
                throw new InvalidAuthException();
            }

            var notifier = SetupConnection();

            new Thread(() =>
            {
                var init = new MessageBuilder()
                    .PutOperationCode("00")
                    .PutAuthInfo(Auth)
                    .PutPayload("NOTIFY")
                    .Build();

                Sender.SendMessage(notifier, init);

                while (true)
                {
                    var notification = Reciever.RecieveMessage(notifier);
                    next(MessageDecoder.DecodePayload(notification.Payload));
                }
            }).Start();
        }

        private bool IsAuthenticated()
        {
            return Auth != "------";
        }

        public void Dispose()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }
    }
}
