using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core
{
    public class Client : IDisposable
    {
        public bool IsConnected { get; set; }
        private int Port { get; set; }
        private Socket Sender { get; set; }

        public Client(int port)
        {
            Port = port;
        }

        public void Connect(Action callback)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Connected to server on Ricardo Port {0}", Port);

                    Sender = sender;
                    IsConnected = true;

                    callback();
                }
                catch
                {
                    Console.WriteLine("ERROR");
                }
            }
            catch
            {
                Console.WriteLine("ERROR2");
            }
        }

        public string Send(string code, string message)
        {
            int length = -1;
            int received = 0;
            byte[] bytes = new byte[1024];

            var request = new MessageBuilder()
                .PutOperationCode(code)
                .PutPayload(message)
                .Build();

            Sender.Send(request);

            while (true)
            {
                int bytesRec = Sender.Receive(bytes, received, 1024 - received, SocketFlags.None);
                received += bytesRec;

                if (received >= sizeof(int) && length == -1)
                {
                    length = BitConverter.ToInt32(bytes, 0);
                }

                if (received >= length)
                {
                    break;
                }
            }

            var response = MessageDecoder.Decode(bytes.Skip(sizeof(int)).Take(length).ToArray());
            if (!response.IsResponse)
            {
                throw new Whaaaa();
            }

            return MessageDecoder.DecodePayload(response.Payload);
        }

        public string Send<T>(string code, T payload) where T : class
        {
            int length = -1;
            int received = 0;
            byte[] bytes = new byte[1024];

            var request = new MessageBuilder()
                .PutOperationCode(code)
                .PutPayload(payload)
                .Build();

            Sender.Send(request);

            while (true)
            {
                int bytesRec = Sender.Receive(bytes, received, 1024 - received, SocketFlags.None);
                received += bytesRec;

                if (received >= sizeof(int) && length == -1)
                {
                    length = BitConverter.ToInt32(bytes, 0);
                }

                if (received >= length)
                {
                    break;
                }
            }

            var response = MessageDecoder.Decode(bytes.Skip(sizeof(int)).Take(length).ToArray());
            if (!response.IsResponse)
            {
                throw new Whaaaa();
            }

            return MessageDecoder.DecodePayload(response.Payload);
        }

        public void Dispose()
        {
            // Release the socket.
            Sender.Shutdown(SocketShutdown.Both);
            Sender.Close();

            IsConnected = false;
        }
    }
}
