using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core
{
    internal class Receiver
    {
        public static DecodedMessage<byte[]> ReceiveMessage(Socket sock)
        {
            int length = -1;
            int received = 0;
            byte[] bytes = new byte[2048];

            while (true)
            {
                int bytesRec = sock.Receive(bytes, received, 1500 - received, SocketFlags.None);
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
            return response;
        }
    }
}
