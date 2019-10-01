using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Subarashii.Core.Exceptions;
using Subarashii.Core.Exchangers;

namespace Subarashii.Core
{
    public class Server
    {
        private int Port { get; set; }
        private IDictionary<string, Socket> Notifiers { get; set; }

        public Server(int port)
        {
            Port = port;
            Notifiers = new Dictionary<string, Socket>();
        }

        public void Run()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    .Single();
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
                                var decoded = Receiver.ReceiveMessage(handler);

                                if (decoded.IsFile)
                                {
                                    decoded = Receiver.ReceiveFile(handler);
                                }

                                if (decoded.Code == "00")
                                {
                                    if (Notifiers.ContainsKey(decoded.Auth))
                                    {
                                        Notifiers.Remove(decoded.Auth);
                                    }

                                    Notifiers.Add(decoded.Auth, handler);
                                    return;
                                }

                                RouteRequest(handler, decoded);
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

        public void SendNotification(string receiver, string msg)
        {
            Socket sock = null;
            Notifiers.TryGetValue(receiver, out sock);

            if (sock == null)
            {
                throw new InvalidOperationException();
            }

            var notification = new MessageBuilder()
                .PutOperationCode("00")
                .PutPayload(msg)
                .Build();

            try
            {
                Sender.SendMessage(sock, notification);
                Receiver.ReceiveMessage(sock);
            }
            catch (DeadConnectionException)
            {
                Notifiers.Remove(receiver);
            }
        }

        private void RouteRequest(Socket sock, DecodedMessage<byte[]> decoded)
        {
            var controllers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(Controller).IsAssignableFrom(t));

            foreach (var ctrl in controllers)
            {
                var handler = ctrl
                    .GetMethods()
                    .Where(m => m.GetCustomAttribute<Handler>().OperationId == decoded.Code)
                    .FirstOrDefault();

                if (handler == null)
                {
                    continue;
                }

                CallHandler(ctrl, handler, sock, decoded);
                break;
            }
        }

        private void CallHandler(Type ctrl, MethodInfo handler, Socket sock, DecodedMessage<byte[]> decoded)
        {
            var ctrlInstance = Activator.CreateInstance(ctrl);
            ctrl.GetMethod("SetContext").Invoke(ctrlInstance, new object[] { sock, decoded.Code });
            var castTo = handler.GetParameters().ElementAt(0).ParameterType;

            if (castTo != typeof(string))
            {
                var decodedPayload = MessageDecoder.DecodePayload(decoded.Payload, castTo);
                handler.Invoke(ctrlInstance, new object[] { decodedPayload, decoded.Auth });
            }
            else
            {
                var decodedPayload = MessageDecoder.DecodePayload(decoded.Payload);
                handler.Invoke(ctrlInstance, new object[] { decodedPayload, decoded.Auth });
            }
        }
    }
}
