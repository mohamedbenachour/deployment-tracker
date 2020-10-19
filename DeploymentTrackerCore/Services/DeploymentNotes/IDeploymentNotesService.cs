using System.Collections.Generic;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.API.DeploymentNotes;

namespace DeploymentTrackerCore.Services.DeploymentNotes {
    public interface IDeploymentNotesService {
        Task<ActionOutcome<ApiNote>> CreateNewNote(int deploymentId, ApiNewNote newNote);

        Task<ActionOutcome<IEnumerable<ApiNote>>> ListNotes(int deploymentId);
    }
}