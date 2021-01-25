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

using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using DeploymentTrackerCore.Json;

using FluentAssertions;

using NUnit.Framework;

namespace IntegrationTests.Helpers {
    public static class HttpResponseExtensions {

        public static async Task<T> AssertSuccessfulResponseAndGetContent<T>(this HttpResponseMessage response) {
            response.AssertSuccessfulResponse();

            var responseBody = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(responseBody, DefaultJsonSerializerOptions.Options);
        }

        public static void AssertSuccessfulResponse(this HttpResponseMessage response) {
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        public static void AssertBadRequest(this HttpResponseMessage response) {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static void AssertForbidden(this HttpResponseMessage response) {
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}