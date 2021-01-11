using System.Linq;

using DeploymentTrackerCore.Services.DeploymentNotes.Mentions;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests.Services.DeploymentNotes.Mentions {
    public class GetMatchingUserMentionsTests {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public void EmptyContentResultsInNoUserMentionsBeingFound(string content) => GetMatchingUserMentions.GetUserMentions(content).Should().BeEmpty();

        [TestCase("hello hello")]
        [TestCase("Just deploying this item to see if we can make it run")]
        [TestCase("Special cha&^**racters()!@#@{}{]]:'/>@prap")]
        public void ContentWithoutMentionsResultsInNoUserMentionsBeingFound(string content) => GetMatchingUserMentions.GetUserMentions(content).Should().BeEmpty();

        [TestCase("Can I have a chat with you about this <@pramod.dematagoda>?", "pramod.dematagoda")]
        [TestCase("Can I have a chat with you about this <@pramod.dematagoda>? Also <@pramod.dematagoda> what is this?", "pramod.dematagoda")]
        [TestCase("<@some-person-1> said you might not need this anymore <@pramod.dematagoda>, is this right?", "pramod.dematagoda", "some-person-1")]
        public void ContentWithMentionsResultsInTheExpectedUserMentionsBeingFound(string content, params string[] expectedUserNames) => GetMatchingUserMentions
            .GetUserMentions(content)
            .Should()
            .BeEquivalentTo(expectedUserNames
                .Select(userName => new UserMention { UserName = userName })
            );
    }
}