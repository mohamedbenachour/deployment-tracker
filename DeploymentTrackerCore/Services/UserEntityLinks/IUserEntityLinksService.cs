using System.Collections.Generic;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;

namespace DeploymentTrackerCore.Services.UserEntityLinks {
    public interface IUserEntityLinksService {
        Task<ActionOutcome<int>> CreateDeploymentNoteLinks(IEnumerable<NewDeploymentNoteLink> noteLinks);

        Task<ActionOutcome<IEnumerable<EntityLinks>>> FetchEntityLinksForCurrentUser();

        Task<ActionOutcome<bool>> MakeInactive(int userEntityLinkId);
    }
}