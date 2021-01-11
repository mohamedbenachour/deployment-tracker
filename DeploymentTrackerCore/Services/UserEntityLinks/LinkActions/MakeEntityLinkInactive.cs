using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.UserEntityLinks.LinkActions {
    public class MakeEntityLinkInactive : IResultBasedAction<int, bool> {
        public MakeEntityLinkInactive(DeploymentAppContext appContext) {
            AppContext = appContext;
        }

        private DeploymentAppContext AppContext { get; }

        public async Task<ActionOutcome<bool>> Perform(int userEntityLinkId) {
            var userEntityLink = await AppContext.UserEntityLinks
                .SingleAsync(entityLink => entityLink.Id == userEntityLinkId);

            userEntityLink.IsActive = false;

            await AppContext.SaveChangesAsync();

            return ActionOutcome<bool>.WithResult(true);
        }
    }
}