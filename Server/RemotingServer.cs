using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace Gestion.Srv
{
    public class RemotingServer
    {
        public static void ExposeContextThroughRemoting()
        {
            IDictionary props = new Hashtable();
            props["port"] = 1234;
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            TcpChannel chan = new TcpChannel(props, null, serverProvider);
            ChannelServices.RegisterChannel(chan, false);
            RemotingServices.Marshal(Context.GetInstance(), "GestionContext");
        }
    }
}
