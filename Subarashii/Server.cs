using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

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

                try
                {
                    listener.Bind(remoteEP);
                    listener.Listen(8);

                    Console.WriteLine("Listening on port {0}", Port);

                    while (true)
                    {
                        // Program is suspended while waiting for an incoming connection.
                        Socket handler = listener.Accept();
                        new Thread(() => {
                            bool connectionDead = false;

                            while (true)
                            {
                                int length = -1;
                                int received = 0;
                                byte[] bytes = new byte[1024];

                                // An incoming connection needs to be processed.
                                while (true)
                                {
                                    int bytesRec = handler.Receive(bytes, received, 1024 - received, SocketFlags.None);
                                    received += bytesRec;

                                    if (bytesRec == 0)
                                    {
                                        connectionDead = true;
                                        break;
                                    }

                                    if (received >= sizeof(int) && length == -1)
                                    {
                                        length = BitConverter.ToInt32(bytes, 0);
                                    }

                                    if (received >= length)
                                    {
                                        break;
                                    }
                                }

                                if (connectionDead)
                                {
                                    break;
                                }

                                var decoded = MessageDecoder.Decode(bytes.Skip(sizeof(int)).Take(length).ToArray());
                                var result = HandleRequest(decoded);

                                if (result.GetType() == typeof(string))
                                {
                                    var response = new MessageBuilder()
                                        .MarkAsResponse()
                                        .PutOperationCode(decoded.Code)
                                        .PutPayload((string)result)
                                        .Build();

                                    handler.Send(response);
                                }
                                else
                                {
                                    var response = new MessageBuilder()
                                        .MarkAsResponse()
                                        .PutOperationCode(decoded.Code)
                                        .PutPayload(result)
                                        .Build();

                                    handler.Send(response);
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
            catch
            {

            }
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

                var ctrlInstance = Activator.CreateInstance(ctrl);

                if (decoded.IsFile)
                {
                    ret = handler.Invoke(ctrlInstance, new object[] { decoded.Payload, decoded.Auth });
                }
                else
                {
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
                }
            }

            return ret;
        }
    }
}
