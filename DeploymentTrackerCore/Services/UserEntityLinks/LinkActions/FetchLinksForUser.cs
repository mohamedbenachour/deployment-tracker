using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.UserEntityLinks.LinkActions {
    public class FetchLinksForUser : IResultBasedAction<IRequestState, IEnumerable<EntityLinks>> {
        public FetchLinksForUser(DeploymentAppContext appContext) {
            AppContext = appContext;
        }

        private DeploymentAppContext AppContext { get; }

        public async Task<ActionOutcome<IEnumerable<EntityLinks>>> Perform(IRequestState requestState) {
            var userEntityLinks = await AppContext.UserEntityLinks
                .Where(entityLink => entityLink.TargetUserName == requestState.GetUser().Username)
                .Where(entityLink => entityLink.IsActive)
                .ToListAsync();

            return ActionOutcome<IEnumerable<EntityLinks>>.WithResult(userEntityLinks.Select(userEntityLink => new EntityLinks {
                CreatedBy = userEntityLink.CreatedBy,
                    ReferencedEntity = userEntityLink.ReferencedEntity,
                    ModifiedBy = userEntityLink.ModifiedBy
            }));
        }
    }
}