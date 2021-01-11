using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Services.UserEntityLinks;

using FluentAssertions;
using FluentAssertions.Execution;

using IntegrationTests.EnvironmentSetup;
using IntegrationTests.Helpers;
using IntegrationTests.Helpers.DeploymentsApi.NotesApi;
using IntegrationTests.Helpers.TestSetup;

using NUnit.Framework;

namespace IntegrationTests {
    [TestFixture]
    public class Mentions {
        private UniqueDeploymentProvider DeploymentProvider { get; set; }

        [OneTimeSetUp]
        public void Setup() {
            DeploymentProvider = new UniqueDeploymentProvider();
        }

        [Test]
        public async Task ANoteWithNoMentionsDoesNotCauseMentionsToBeCreated() {
            var userA = TestNames.UserName;
            var userB = TestNames.UserName;
            var clientA = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userA);
            var clientB = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userB);

            await CreateDeploymentWithNote(clientA, "nuthin to see hear");

            var currentMentions = await GetCurrentMentions(clientB);

            currentMentions.Should().BeEmpty();
        }

        [Test]
        public async Task ANoteWithAMentionCausesAMentionToBeCreated() {
            var userA = TestNames.UserName;
            var userB = TestNames.UserName;
            var clientA = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userA);
            var clientB = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userB);

            var newNote = await CreateDeploymentWithNote(clientA, $"What do you think of this <@{userB}>?");

            var currentMentions = await GetCurrentMentions(clientB);

            currentMentions.Should().BeEquivalentTo(new List<EntityLinks> {
                    new EntityLinks {
                        ReferencedEntity = $"Deployment::{newNote.DeploymentId}::DeploymentNote::{newNote.Id}"
                    }
                }, options => options
                .Excluding(link => link.Id)
                .Excluding(link => link.CreatedBy)
                .Excluding(link => link.ModifiedBy)
            );
        }

        [Test]
        public async Task ANoteWithAMentionHasTheExpectedTimestamp() {
            var userA = TestNames.UserName;
            var userB = TestNames.UserName;
            var clientA = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userA);
            var clientB = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userB);
            var dateTimeBeforeNoteCreation = DateTime.UtcNow;

            await CreateDeploymentWithNote(clientA, $"What do you think of this <@{userB}>?");

            var newMention = (await GetCurrentMentions(clientB)).Single();

            newMention.CreatedBy.Timestamp.Should().BeAfter(dateTimeBeforeNoteCreation);
        }

        [Test]
        public async Task AMentionCanBeAcknowledged() {
            var userA = TestNames.UserName;
            var userB = TestNames.UserName;
            var clientA = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userA);
            var clientB = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userB);

            var newNote = await CreateDeploymentWithNote(clientA, $"What do you think of this <@{userB}>?");

            var currentMentions = await GetCurrentMentions(clientB);

            var mentionToAcknowledge = currentMentions.First();

            await AcknowledgeMention(clientB, mentionToAcknowledge.Id);
        }

        [Test]
        public async Task AMentionThatIsAcknowledgedWillNotAppearInTheListOfCurrentMentions() {
            var userA = TestNames.UserName;
            var userB = TestNames.UserName;
            var clientA = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userA);
            var clientB = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userB);

            var newNote = await CreateDeploymentWithNote(clientA, $"What do you think of this <@{userB}>?");

            var mentionToAcknowledge = (await GetCurrentMentions(clientB)).First();

            await AcknowledgeMention(clientB, mentionToAcknowledge.Id);

            var currentMentions = await GetCurrentMentions(clientB);

            currentMentions.Should().BeEmpty();
        }

        private async Task AcknowledgeMention(HttpClient client, int mentionId) {
            var deleteUrl = $"{TestEnvironment.URLs.Mention}/{mentionId}";

            (await client.DeleteAsync(deleteUrl)).AssertSuccessfulResponse();
        }

        private async Task<IEnumerable<EntityLinks>> GetCurrentMentions(HttpClient client) => (await (await client
                .GetAsync(TestEnvironment.URLs.Mention))
            .AssertSuccessfulResponseAndGetContent<IEnumerable<EntityLinks>>()
        );

        private async Task<ApiNote> CreateDeploymentWithNote(HttpClient client, string content) {
            var deployment = await DeploymentProvider.Create();

            return await CreateNoteForDeployment.Create(client, deployment, content);
        }
    }
}