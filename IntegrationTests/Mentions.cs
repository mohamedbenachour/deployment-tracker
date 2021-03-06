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
            var dateTimeBeforeNoteCreation = DateTime.UtcNow;
            var client = await SimulateAMentionAndReturnReceivingUserClient();

            var newMention = (await GetCurrentMentions(client)).Single();

            newMention.CreatedBy.Timestamp.Should().BeAfter(dateTimeBeforeNoteCreation);
        }

        [Test]
        public async Task AMentionIncludesAnId() {
            var client = await SimulateAMentionAndReturnReceivingUserClient();

            var newMention = (await GetCurrentMentions(client)).Single();

            newMention.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task AMentionCanBeAcknowledged() {
            var client = await SimulateAMentionAndReturnReceivingUserClient();

            var currentMentions = await GetCurrentMentions(client);

            var mentionToAcknowledge = currentMentions.First();

            await AcknowledgeMentionAndAssertSuccess(client, mentionToAcknowledge.Id);
        }

        [Test]
        public async Task YouCannotAcknowledgeAMentionForAnotherUser() {
            var clientTheMentionBelongsTo = await SimulateAMentionAndReturnReceivingUserClient();
            var differentClient = await TestEnvironment.ClientFactory.GetAuthenticatedClient(TestNames.UserName);

            var currentMentions = await GetCurrentMentions(clientTheMentionBelongsTo);

            var mentionToAcknowledge = currentMentions.First();

            (await AcknowledgeMention(differentClient, mentionToAcknowledge.Id)).AssertBadRequest();
        }

        [Test]
        public async Task AMentionThatIsAcknowledgedWillNotAppearInTheListOfCurrentMentions() {
            var client = await SimulateAMentionAndReturnReceivingUserClient();

            var mentionToAcknowledge = (await GetCurrentMentions(client)).First();

            await AcknowledgeMentionAndAssertSuccess(client, mentionToAcknowledge.Id);

            var currentMentions = await GetCurrentMentions(client);

            currentMentions.Should().BeEmpty();
        }

        private async Task<HttpClient> SimulateAMentionAndReturnReceivingUserClient() {
            var userA = TestNames.UserName;
            var userB = TestNames.UserName;
            var clientA = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userA);
            var clientB = await TestEnvironment.ClientFactory.GetAuthenticatedClient(userB);

            await CreateDeploymentWithNote(clientA, $"What do you think of this <@{userB}>?");

            return clientB;
        }

        private async Task AcknowledgeMentionAndAssertSuccess(HttpClient client, int mentionId) => (await AcknowledgeMention(client, mentionId)).AssertSuccessfulResponse();

        private async Task<HttpResponseMessage> AcknowledgeMention(HttpClient client, int mentionId) {
            var deleteUrl = $"{TestEnvironment.URLs.Mention}/{mentionId}";

            return await client.DeleteAsync(deleteUrl);
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