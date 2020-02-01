using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DeploymentTrackerCore.Json;
using NUnit.Framework;

namespace IntegrationTests.Helpers {
    public static class HttpResponseExtensions {

        public static async Task<T> AssertSuccessfulResponseAndGetContent<T>(this HttpResponseMessage response) {
            response.AssertSuccessfulResponse();

            var responseBody = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(responseBody, DefaultJsonSerializerOptions.Options);
        }

        public static void AssertSuccessfulResponse(this HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                throw new AssertionException($"Expected successful response but got: {response.ReasonPhrase}\n\n{response.Content.ReadAsStringAsync().Result}");
            }
        }

        public static void AssertBadRequest(this HttpResponseMessage response) {
            if (response.StatusCode != HttpStatusCode.BadRequest) {
                throw new AssertionException($"Expected BadRequest but got: {response.StatusCode}");
            }
        }
    }
}