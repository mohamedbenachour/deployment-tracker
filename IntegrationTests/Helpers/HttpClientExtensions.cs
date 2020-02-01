using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DeploymentTrackerCore.Json;

namespace IntegrationTests.Helpers {
    public static class HttpClientExtensions {
        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient client, string url, T toPost) {
            var serialisedObject = JsonSerializer.Serialize(toPost, DefaultJsonSerializerOptions.Options);
            var content = new StringContent(serialisedObject, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await client.PostAsync(url, content);
        }
    }
}