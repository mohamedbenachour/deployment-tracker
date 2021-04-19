using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.DeploymentNotes.NoteActions {
    public class DeleteNoteForDeployment : IResultBasedAction<DeleteNoteRequest, int> {
        private DeploymentAppContext AppContext { get; }

        public DeleteNoteForDeployment(DeploymentAppContext appContext) {
            AppContext = appContext;
        }

        public async Task<ActionOutcome<int>> Perform(DeleteNoteRequest input) {
            var matchingNotes = await AppContext.DeploymentNote
                .AsQueryable()
                .Where(deploymentNote => deploymentNote.Id == input.NoteId && deploymentNote.Deployment.Id == input.DeploymentId)
                .ToListAsync();

            if (!matchingNotes.Any()) {
                return ActionOutcome<int>.WithError("The specified note does not exist");
            }

            matchingNotes.First().IsActive = false;
            await AppContext.SaveChangesAsync();

            return ActionOutcome<int>.WithResult(input.NoteId);
        }
    }

    public class DeleteNoteRequest {
        public int DeploymentId { get; set; }
        public int NoteId { get; set; }
    }
}