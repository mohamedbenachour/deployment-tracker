using System.Collections.Generic;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Models.Entities;
using DeploymentTrackerCore.Services.DeploymentNotes.NoteActions;
using DeploymentTrackerCore.Services.UserEntityLinks;

namespace DeploymentTrackerCore.Services.DeploymentNotes {
    public class DeploymentNotesService : IDeploymentNotesService {
        private DeploymentAppContext AppContext { get; }
        private IUserEntityLinksService UserEntityLinksService { get; }

        public DeploymentNotesService(DeploymentAppContext appContext, IUserEntityLinksService userEntityLinksService) {
            this.AppContext = appContext;
            UserEntityLinksService = userEntityLinksService;
        }

        public async Task<ActionOutcome<ApiNote>> CreateNewNote(int deploymentId, ApiNewNote newNote) => await new NewDeploymentNote(AppContext, UserEntityLinksService)
            .Perform(new NewNoteRequest {
                DeploymentId = deploymentId,
                    Note = newNote,
            });

        public async Task<ActionOutcome<IEnumerable<ApiNote>>> ListNotes(int deploymentId) => await new ListNotesForDeployment(AppContext).Perform(deploymentId);

        public async Task<ActionOutcome<int>> DeleteNote(int deploymentId, int noteId) => await new DeleteNoteForDeployment(AppContext).Perform(new DeleteNoteRequest {
            DeploymentId = deploymentId,
                NoteId = noteId
        });
    }
}