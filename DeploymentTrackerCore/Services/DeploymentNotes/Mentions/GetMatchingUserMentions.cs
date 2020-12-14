using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeploymentTrackerCore.Services.DeploymentNotes.Mentions {
    public class GetMatchingUserMentions {
        private const string MatchingGroupName = "userName";

        public static IEnumerable<UserMention> GetUserMentions(string content) {
            var foundUserNames = new HashSet<string>();

            if (string.IsNullOrWhiteSpace(content)) {
                return new List<UserMention>();
            }

            var mentionMatches = Regex.Matches(content, $@"<@(?'{MatchingGroupName}'(\w|\.|-)+)>", RegexOptions.Compiled);

            foreach (var match in mentionMatches.AsEnumerable()) {
                var userNameGroup = match.Groups[MatchingGroupName];

                foundUserNames.Add(userNameGroup.Value);
            }

            return foundUserNames.Select(userName => new UserMention {
                UserName = userName
            });
        }
    }
}