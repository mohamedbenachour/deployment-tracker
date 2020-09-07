using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Models.API.DeploymentNotes;

using FluentAssertions;
using FluentAssertions.Execution;

using IntegrationTests.Helpers;
using IntegrationTests.Helpers.TestSetup;

using NUnit.Framework;

namespace IntegrationTests {
    [TestFixture]
    public class DeploymentNotes {
        private UniqueDeploymentProvider DeploymentProvider { get; set; }

        [OneTimeSetUp]
        public void Setup() {
            DeploymentProvider = new UniqueDeploymentProvider();
        }

        [Test]
        public async Task ANewDeploymentDoesNotHaveAnyNotes() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var deployment = await DeploymentProvider.Create();

            var deploymentNotes = (await (await client.GetAsync(GetNotesUrlForDeployment(deployment)))
                .AssertSuccessfulResponseAndGetContent<IEnumerable<ApiNote>>());

            deploymentNotes.Should().BeEmpty();
        }

        [Test]
        public async Task ANoteCanBeAddedAgainstADeployment() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var deployment = await DeploymentProvider.Create();

            await CreateNoteForDeployment(client, deployment, "Hello World!!");
        }

        [Test]
        public async Task ANoteThatHasBeenAddedForADeploymentCanBeRetrieved() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var noteContent = "lorem ipsum something something random; I forget what it has sometimes really...";

            var deployment = await DeploymentProvider.Create();

            await CreateNoteForDeployment(client, deployment, noteContent);

            var deploymentNotes = (await (await client.GetAsync(GetNotesUrlForDeployment(deployment)))
                .AssertSuccessfulResponseAndGetContent<IEnumerable<ApiNote>>());

            using(new AssertionScope()) {
                deploymentNotes.Should().HaveCount(1);
                deploymentNotes.Single().Content.Should().Be(noteContent);
            }
        }

        [Test]
        public async Task NotesForOtherDeploymentsAreNotAccidentallyReturned() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var noteForDeploymentA = "A";
            var noteForDeploymentB = "B";

            var deploymentA = await DeploymentProvider.Create();
            var deploymentB = await DeploymentProvider.Create();

            await CreateNoteForDeployment(client, deploymentA, noteForDeploymentA);
            await CreateNoteForDeployment(client, deploymentB, noteForDeploymentB);

            var deploymentNotes = (await (await client.GetAsync(GetNotesUrlForDeployment(deploymentB)))
                .AssertSuccessfulResponseAndGetContent<IEnumerable<ApiNote>>());

            using(new AssertionScope()) {
                deploymentNotes.Should().HaveCount(1);
                deploymentNotes.Single().Content.Should().Be(noteForDeploymentB);
            }
        }

        private async Task CreateNoteForDeployment(HttpClient client, ApiDeployment deployment, string content) => (await client.PostJsonAsync(
            GetNotesUrlForDeployment(deployment),
            new ApiNewNote {
                Content = content
            }
        )).AssertSuccessfulResponse();

        private static string GetNotesUrlForDeployment(ApiDeployment deployment) => $"{TestEnvironment.URLs.Deployment}/{deployment.Id}/note";
    }
}