using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.DeploymentNotes.NoteActions {
    public class NewDeploymentNote : IResultBasedAction<NewNoteRequest, ApiNote> {
        private DeploymentAppContext AppContext { get; }

        public NewDeploymentNote(DeploymentAppContext appContext) {
            AppContext = appContext;
        }

        public async Task<ActionOutcome<ApiNote>> Perform(NewNoteRequest input) {
            var newNote = new DeploymentNote {
                Content = input.Note.Content,
                Deployment = await GetDeployment(input.DeploymentId)
            };

            AppContext.Notes.Add(newNote);

            await AppContext.SaveChangesAsync();

            return ActionOutcome<ApiNote>.WithResult(new ApiNote {
                Id = newNote.Id,
                    Content = newNote.Content,
                    DeploymentId = newNote.Deployment.Id
            });
        }

        public async Task<Deployment> GetDeployment(int deploymentId) => await AppContext.Deployments.SingleAsync(deployment => deployment.Id == deploymentId);
    }

    public class NewNoteRequest {
        public ApiNewNote Note { get; set; }
        public int DeploymentId { get; set; }
    }
}