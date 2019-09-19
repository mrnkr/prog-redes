using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core.Exchangers
{
    internal class Receiver
    {
        public static DecodedMessage<byte[]> ReceiveMessage(Socket sock)
        {
            int length = -1;
            int received = 0;
            byte[] bytes = new byte[1500];

            try
            {
                while (true)
                {
                    int bytesRec = sock.Receive(bytes, received, 1500 - received, SocketFlags.None);
                    received += bytesRec;

                    if (received >= sizeof(int) && length == -1)
                    {
                        length = BitConverter.ToInt32(bytes, 0);
                    }

                    if (received >= length && length > 0)
                    {
                        break;
                    }
                }
            }
            catch
            {
                throw new DeadConnectionException();
            }


            var response = MessageDecoder.Decode(bytes.Skip(sizeof(int)).Take(length).ToArray());
            return response;
        }

        public static DecodedMessage<byte[]> ReceiveFile(Socket sock, bool skipFirst = false)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            FileStream fs = null;

            do
            {
                var decoded = ReceiveMessage(sock);

                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }

                if (!decoded.IsFile)
                {
                    fs.Close();
                    return decoded;
                }

                int pathLength = BitConverter.ToInt32(decoded.Payload, 0);
                string path = Encoding.UTF8.GetString(decoded.Payload.Skip(sizeof(int)).Take(pathLength).ToArray());

                if (fs == null)
                {
                    fs = File.OpenWrite(Path.Combine(userFolder, "Downloads", path));
                }

                byte[] data = decoded.Payload.Skip(sizeof(int) + pathLength).ToArray();
                fs.Write(data, 0, data.Length);
                SendAck(sock, decoded.Code);
            } while (true);
        }

        private static void SendAck(Socket sock, string code)
        {
            var ack = new MessageBuilder()
                .MarkAsResponse()
                .PutOperationCode(code)
                .PutPayload("OK")
                .Build();

            Sender.SendMessage(sock, ack);
        }
    }
}
