/*
* This file is part of Deployment Tracker.
* 
* Deployment Tracker is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* Deployment Tracker is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
*/

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

namespace IntegrationTests.Helpers
{
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