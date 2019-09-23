using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Subarashii.Core.Exceptions;
using Subarashii.Core.Exchangers;
using Subarashii.PseudoRx;

namespace Subarashii.Core
{
    public class Client : IDisposable
    {
        private string IpAddr { get; set; }
        private int Port { get; set; }
        private Socket Socket { get; set; }
        private string Auth { get; set; }

        public Client(string ipAddr, int port)
        {
            IpAddr = ipAddr;
            Port = port;
            Auth = "------";
        }

        public void Connect(Action onConnect)
        {
            try
            {
                Socket = SetupConnection();
                Console.WriteLine("Connected to server on {0}:{1}", IpAddr, Port);
                onConnect();
            }
            catch
            {

            }
        }

        private Socket SetupConnection()
        {
            IPAddress ipAddress = IPAddress.Parse(IpAddr);
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

        public string Receive()
        {
            DecodedMessage<byte[]> response = Receiver.ReceiveMessage(Socket);
            return MessageDecoder.DecodePayload(response.Payload);
        }

        public T Receive<T>() where T : class
        {
            DecodedMessage<byte[]> response = Receiver.ReceiveMessage(Socket);
            return MessageDecoder.DecodePayload<T>(response.Payload);
        }

        public string ReceiveFile()
        {
            DecodedMessage<byte[]> response = Receiver.ReceiveFile(Socket, true);
            return MessageDecoder.DecodePayload(response.Payload);
        }

        public Subscription ListenToNotifications(Action<string> next)
        {
            if (!IsAuthenticated())
            {
                throw new InvalidAuthException();
            }

            var notifier = SetupConnection();

            var t = new Thread(() =>
            {
                var init = new MessageBuilder()
                    .PutOperationCode("00")
                    .PutAuthInfo(Auth)
                    .PutPayload("NOTIFY")
                    .Build();

                Sender.SendMessage(notifier, init);

                while (true)
                {
                    try
                    {
                        var notification = Receiver.ReceiveMessage(notifier);
                        next(MessageDecoder.DecodePayload(notification.Payload));
                    }
                    catch (DeadConnectionException)
                    {
                        break;
                    }
                }
            });
            t.Start();

            return new Subscription(() =>
            {
                notifier.Shutdown(SocketShutdown.Both);
                notifier.Close();
            });
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
