using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Services.DeploymentNotes.Mentions;
using DeploymentTrackerCore.Services.UserEntityLinks;

namespace DeploymentTrackerCore.Services.DeploymentNotes.NoteActions {
    public class CreateMentionsForDeploymentNote : IResultBasedAction<ApiNote, int> {
        public CreateMentionsForDeploymentNote(IUserEntityLinksService userEntityLinksService) {
            UserEntityLinksService = userEntityLinksService;
        }

        private IUserEntityLinksService UserEntityLinksService { get; }

        public async Task<ActionOutcome<int>> Perform(ApiNote input) {
            var linksToAdd = ExtractUserEntityLinks
                .Extract(input.DeploymentId, input.Id, input.Content);

            await UserEntityLinksService.CreateDeploymentNoteLinks(linksToAdd);

            return ActionOutcome<int>.WithResult(linksToAdd.Count());
        }
    }
}