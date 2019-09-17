using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Subarashii.Core.Exchangers
{
    internal class Sender
    {
        public static void SendMessage(Socket sock, byte[] msg)
        {
            sock.Send(msg);
        }

        public static void SendFile(Socket sock, MessageBuilder builder, string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                builder.MarkAsFile();

                byte[] data = new byte[1400];
                byte[] pathPrefix = Encoding.UTF8.GetBytes(path.Split('\\').Last());
                BitConverter.GetBytes(pathPrefix.Length).CopyTo(data, 0);
                pathPrefix.CopyTo(data, sizeof(int));

                var transferInit = builder
                    .PutPayload(data)
                    .Build();
                SendMessage(sock, transferInit);

                while (fs.Read(data, pathPrefix.Length + sizeof(int), data.Length - pathPrefix.Length - sizeof(int)) > 0)
                {
                    var request = builder
                        .PutPayload(data)
                        .Build();

                    SendMessage(sock, request);
                    Reciever.RecieveMessage(sock);
                }

                var wrapper = builder
                    .MarkAsText()
                    .PutPayload($"FileName={path.Split('\\').Last()}")
                    .Build();

                SendMessage(sock, wrapper);
            }
        }
    }
}
