using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Services.DeploymentNotes.NoteActions {
    public class NoteEntityToApiNote : FunctionalAction<DeploymentNote, ApiNote> {
        public override ApiNote Perform(DeploymentNote input) => new ApiNote {
            Content = input.Content,
            DeploymentId = input.Deployment.Id,
            Id = input.Id
        };
    }
}