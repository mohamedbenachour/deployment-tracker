using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IntegrationTests.EnvironmentSetup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Helpers {
    public static class TestApplicationFactoryExtensions {

        public static async Task<HttpClient> GetAuthenticatedClient(this TestApplicationFactory<DeploymentTrackerCore.Startup> factory) {
            var client = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>(
                                "Test", options => {});
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Test");

            await SetCSRFTokenHeader(client);

            return client;
        }

        private static async Task SetCSRFTokenHeader(HttpClient client) {
            var csrfToken = await GetCSRFToken(client);

            client.DefaultRequestHeaders.Add("RequestVerificationToken", new List<string>{ csrfToken });
        }

        private static async Task<string> GetCSRFToken(HttpClient client) {
            var indexHtml = await client.GetStringAsync("/");

            var csrfTokenRegex = @"\w*csrfToken: ""([^""]+)""";
            var matches = Regex.Match(indexHtml, csrfTokenRegex);

            return matches.Groups[1].Captures[0].Value;
        }
    }
}