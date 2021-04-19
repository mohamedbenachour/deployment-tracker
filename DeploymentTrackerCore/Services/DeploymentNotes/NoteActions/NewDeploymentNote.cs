using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Models.Entities;
using DeploymentTrackerCore.Services.UserEntityLinks;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.DeploymentNotes.NoteActions {
    public class NewDeploymentNote : IResultBasedAction<NewNoteRequest, ApiNote> {
        private DeploymentAppContext AppContext { get; }
        private IUserEntityLinksService UserEntityLinksService { get; }

        public NewDeploymentNote(DeploymentAppContext appContext, IUserEntityLinksService userEntityLinksService) {
            AppContext = appContext;
            UserEntityLinksService = userEntityLinksService;
        }

        public async Task<ActionOutcome<ApiNote>> Perform(NewNoteRequest input) {
            var newNote = new DeploymentNote {
                Content = input.Note.Content,
                Deployment = await GetDeployment(input.DeploymentId),
                IsActive = true
            };

            AppContext.DeploymentNote.Add(newNote);

            await AppContext.SaveChangesAsync();

            var createdNote = new ApiNote {
                Id = newNote.Id,
                Content = newNote.Content,
                DeploymentId = newNote.Deployment.Id
            };

            await new CreateMentionsForDeploymentNote(UserEntityLinksService).Perform(createdNote);

            return ActionOutcome<ApiNote>.WithResult(createdNote);
        }

        public async Task<Deployment> GetDeployment(int deploymentId) => await AppContext.Deployments.SingleAsync(deployment => deployment.Id == deploymentId);
    }

    public class NewNoteRequest {
        public ApiNewNote Note { get; set; }
        public int DeploymentId { get; set; }
    }
}