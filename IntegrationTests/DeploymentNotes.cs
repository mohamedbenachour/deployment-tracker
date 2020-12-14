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
using IntegrationTests.Helpers.DeploymentsApi.NotesApi;
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

            var deploymentNotes = await GetNotesForDeployment(client, deployment);

            deploymentNotes.Should().BeEmpty();
        }

        [Test]
        public async Task ADeploymentNoteCanBeDeleted() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var deployment = await DeploymentProvider.Create();

            await CreateNoteForDeployment.Create(client, deployment, "About to be deleted");

            var deploymentNoteToDelete = (await GetNotesForDeployment(client, deployment)).First().Id;

            await DeleteDeploymentNote(client, deployment, deploymentNoteToDelete);
        }

        [Test]
        public async Task ADeletedDeploymentNoteIsNotReturned() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var deployment = await DeploymentProvider.Create();

            await CreateNoteForDeployment.Create(client, deployment, "About to be deleted");

            var deploymentNoteToDelete = (await GetNotesForDeployment(client, deployment)).First().Id;

            await DeleteDeploymentNote(client, deployment, deploymentNoteToDelete);

            var deploymentNotes = await GetNotesForDeployment(client, deployment);

            deploymentNotes.Should().BeEmpty();
        }

        private async Task DeleteDeploymentNote(HttpClient client, ApiDeployment deployment, int noteId) {
            var deleteUrl = $"{NoteUrlForDeployment.GetForDeployment(deployment)}/{noteId}";

            (await client.DeleteAsync(deleteUrl)).AssertSuccessfulResponse();
        }

        [Test]
        public async Task ANoteCanBeAddedAgainstADeployment() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var deployment = await DeploymentProvider.Create();

            await CreateNoteForDeployment.Create(client, deployment, "Hello World!!");
        }

        [Test]
        public async Task ANoteThatHasBeenAddedForADeploymentCanBeRetrieved() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var noteContent = "lorem ipsum something something random; I forget what it has sometimes really...";

            var deployment = await DeploymentProvider.Create();

            await CreateNoteForDeployment.Create(client, deployment, noteContent);

            var deploymentNotes = await GetNotesForDeployment(client, deployment);

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

            await CreateNoteForDeployment.Create(client, deploymentA, noteForDeploymentA);
            await CreateNoteForDeployment.Create(client, deploymentB, noteForDeploymentB);

            var deploymentNotes = await GetNotesForDeployment(client, deploymentB);

            using(new AssertionScope()) {
                deploymentNotes.Should().HaveCount(1);
                deploymentNotes.Single().Content.Should().Be(noteForDeploymentB);
            }
        }

        private async Task<IEnumerable<ApiNote>> GetNotesForDeployment(HttpClient client, ApiDeployment deployment) => (await (await client.GetAsync(NoteUrlForDeployment.GetForDeployment(deployment)))
            .AssertSuccessfulResponseAndGetContent<IEnumerable<ApiNote>>());
    }
}