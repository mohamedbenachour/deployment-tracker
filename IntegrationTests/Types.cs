using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using DeploymentTrackerCore.Models.API;
using FluentAssertions;
using IntegrationTests.Helpers;
using NUnit.Framework;

namespace IntegrationTests {
    [TestFixture]
    public class Types {

        [Test]
        [AutoData]
        public async Task ATypeCanBeAdded(ApiNewType typeToAdd) {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.Type, typeToAdd);

            var addedType = response.AssertSuccessfulResponseAndGetContent<ApiType>();

            addedType.Should().BeEquivalentTo(typeToAdd, (options) => options.ExcludingMissingMembers());
        }

        [Test]
        [AutoData]
        public async Task ATypeWithADuplicateNameCannotBeAdded(ApiNewType typeToAdd) {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            await client.PostJsonAsync(TestEnvironment.URLs.Type, typeToAdd);
            var response = await client.PostJsonAsync(TestEnvironment.URLs.Type, typeToAdd);

            response.AssertBadRequest();
        }


        [Test]
        public async Task TypesCanBeRetrieved() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var response = await client.GetAsync(TestEnvironment.URLs.Type);

            response.AssertSuccessfulResponseAndGetContent<IEnumerable<ApiType>>().Should().NotBeNull();
        }
    }
}