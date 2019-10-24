using Gestion.Services;
using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace AdminServer
{
    public class RemotingClient
    {
        public static void Setup()
        {
            IDictionary props = new Hashtable();
            props["port"] = 0;
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            new TcpChannel(props, null, serverProvider);
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            TcpChannel chan = new TcpChannel(props, null, serverProvider);
            ChannelServices.RegisterChannel(chan, false);
        }

        public static IContext GetContext()
        {
            return (IContext)Activator.GetObject(
                type: typeof(IContext),
                "tcp://localhost:1234/GestionContext");
        }
    }
}