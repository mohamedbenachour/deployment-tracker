using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.DeploymentNotes.NoteActions {
    public class ListNotesForDeployment : IResultBasedAction<int, IEnumerable<ApiNote>> {
        private DeploymentAppContext AppContext { get; }
        public ListNotesForDeployment(DeploymentAppContext appContext) {
            AppContext = appContext;
        }

        public async Task<ActionOutcome<IEnumerable<ApiNote>>> Perform(int deploymentId) {
            var converter = new NoteEntityToApiNote();

            return ActionOutcome<IEnumerable<ApiNote>>.WithResult(
                (await AppContext.DeploymentNote.Where(note => note.IsActive && note.Deployment.Id == deploymentId)
                    .Include(note => note.Deployment)
                    .ToListAsync())
                .Select(converter.Function));
        }
    }
}