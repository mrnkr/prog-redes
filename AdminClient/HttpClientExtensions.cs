using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gestion.Admin.Cli
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetObjectAsync<T>(this HttpClient http, string uri)
        {
            var response = await http.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<T> PostAndExpectObjectAsync<T>(this HttpClient http, string uri, object body)
        {
            var response = await http.PostAsync(uri, JsonContent.WithObject(body));
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
