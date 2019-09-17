using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        }

        public void Connect(Action onConnect)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEP);
                Console.WriteLine("Connected to server on Ricardo Port {0}", Port);

                Socket = sender;
                onConnect();
            }
            catch
            {
                Console.WriteLine("ERROR2");
            }
        }

        public void Send(string code, string msg)
        {
            var request = new MessageBuilder()
                .PutOperationCode(code)
                .PutPayload(msg)
                .Build();

            Sender.SendMessage(Socket, request);
        }

        public void Send<T>(string code, T payload) where T : class
        {
            var request = new MessageBuilder()
                .PutOperationCode(code)
                .PutPayload(payload)
                .Build();

            Sender.SendMessage(Socket, request);
        }

        public void SendFile(string code, string path)
        {
            var builder = new MessageBuilder()
                .PutOperationCode(code);

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

        public void Dispose()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }
    }
}
