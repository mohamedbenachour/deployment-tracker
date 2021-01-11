/*
 * This file is part of Deployment Tracker.
 * 
 * Deployment Tracker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Deployment Tracker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Services.UserEntityLinks.LinkActions {
    public class MakeEntityLinkInactive : IResultBasedAction<int, bool> {
        public MakeEntityLinkInactive(DeploymentAppContext appContext, IRequestState requestState) {
            RequestState = requestState;
            AppContext = appContext;
        }

        private IRequestState RequestState { get; }
        private DeploymentAppContext AppContext { get; }

        public async Task<ActionOutcome<bool>> Perform(int userEntityLinkId) {
            var userEntityLink = await AppContext.UserEntityLinks
                .Where(entityLink => entityLink.TargetUserName == RequestState.GetUser().Username)
                .SingleOrDefaultAsync(entityLink => entityLink.Id == userEntityLinkId);

            if (userEntityLink == null) {
                return ActionOutcome<bool>.WithError("No such entity link found.");
            }

            userEntityLink.IsActive = false;

            await AppContext.SaveChangesAsync();

            return ActionOutcome<bool>.WithResult(true);
        }
    }
}