using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Subarashii.Core.Exceptions;

namespace Subarashii.Core
{
    public class Server
    {
        private int Port { get; set; }

        public Server(int port)
        {
            Port = port;
        }

        public void Run()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(remoteEP);
                listener.Listen(8);

                Console.WriteLine("Listening on port {0}", Port);

                while (true)
                {
                    Socket handler = listener.Accept();
                    new Thread(() => {
                        while (true)
                        {
                            try
                            {
                                var decoded = ReceiveMessage(handler);
                                var result = HandleRequest(decoded);
                                RespondToMessage(handler, decoded, result);
                            }
                            catch (DeadConnectionException)
                            {
                                Console.WriteLine("Connection is dead, closing...");
                                break;
                            }
                        }

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }).Start();
                }
            }
            catch
            {

            }
        }

        private DecodedMessage<byte[]> ReceiveMessage(Socket handler)
        {
            int length = -1;
            int received = 0;
            byte[] bytes = new byte[2048];

            // An incoming connection needs to be processed.
            while (true)
            {
                int bytesRec = handler.Receive(bytes, received, 2048 - received, SocketFlags.None);
                received += bytesRec;

                if (bytesRec == 0)
                {
                    throw new DeadConnectionException();
                }

                if (received >= sizeof(int) && length == -1)
                {
                    length = BitConverter.ToInt32(bytes, 0);
                }

                if (received >= length || length == 0)
                {
                    break;
                }
            }

            return MessageDecoder.Decode(bytes.Skip(sizeof(int)).Take(length).ToArray());
        }

        private object HandleRequest(DecodedMessage<byte[]> decoded)
        {
            var controllers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(Controller).IsAssignableFrom(t));

            object ret = null;

            foreach (var ctrl in controllers)
            {
                var handler = ctrl
                    .GetMethods()
                    .Where(m => m.GetCustomAttribute<Handler>().OperationId == decoded.Code)
                    .First();

                if (handler == null)
                {
                    continue;
                }

                ret = CallHandler(ctrl, handler, decoded);
                break;
            }

            return ret;
        }

        private object CallHandler(Type ctrl, MethodInfo handler, DecodedMessage<byte[]> decoded)
        {
            object ret = null;
            var ctrlInstance = Activator.CreateInstance(ctrl);

            var castTo = handler.GetParameters().ElementAt(0).ParameterType;

            if (castTo != typeof(string))
            {
                var decodedPayload = MessageDecoder.DecodePayload(decoded.Payload, castTo);
                ret = handler.Invoke(ctrlInstance, new object[] { decodedPayload, decoded.Auth });
            }
            else
            {
                var decodedPayload = MessageDecoder.DecodePayload(decoded.Payload);
                ret = handler.Invoke(ctrlInstance, new object[] { decodedPayload, decoded.Auth });
            }

            return ret;
        }

        private void RespondToMessage(Socket handler, DecodedMessage<byte[]> decoded, object result)
        {
            var responseBuilder = new MessageBuilder()
                .MarkAsResponse()
                .PutOperationCode(decoded.Code);

            if (result.GetType() == typeof(string))
            {
                responseBuilder.PutPayload((string)result);
            }
            else
            {
                responseBuilder.PutPayload(result);
            }

            handler.Send(responseBuilder.Build());
        }
    }
}
