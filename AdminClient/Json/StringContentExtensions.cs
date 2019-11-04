using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gestion.Admin.Cli
{
    public static class StringContentExtensions
    {
        public static async Task<T> GetContent<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
