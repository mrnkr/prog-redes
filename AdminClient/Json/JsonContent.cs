using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Gestion.Admin.Cli
{
    internal static class JsonContent
    {
        public static StringContent WithObject(object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }
    }
}
