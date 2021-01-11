using System.Collections.Generic;
using System.Linq;

using DeploymentTrackerCore.Services.UserEntityLinks;

namespace DeploymentTrackerCore.Services.DeploymentNotes.Mentions {
    public class ExtractUserEntityLinks {
        public static IEnumerable<NewDeploymentNoteLink> Extract(int deploymentId, int deploymentNoteId, string content) {
            var userNamesMentioned = GetMatchingUserMentions.GetUserMentions(content);

            return userNamesMentioned.Select(userNameMentioned => new NewDeploymentNoteLink {
                TargetingUserName = userNameMentioned.UserName,
                    DeploymentId = deploymentId,
                    DeploymentNoteId = deploymentNoteId
            });
        }
    }
}