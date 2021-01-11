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
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;
using DeploymentTrackerCore.Services.UserEntityLinks.LinkActions;

namespace DeploymentTrackerCore.Services.UserEntityLinks {
    public class UserEntityLinksService : IUserEntityLinksService {

        public UserEntityLinksService(DeploymentAppContext appContext, IRequestState requestState) {
            AppContext = appContext;
            RequestState = requestState;
        }

        private DeploymentAppContext AppContext { get; }
        private IRequestState RequestState { get; }

        public async Task<ActionOutcome<IEnumerable<EntityLinks>>> FetchEntityLinksForCurrentUser() => await new FetchLinksForUser(AppContext).Perform(RequestState);

        public async Task<ActionOutcome<int>> CreateDeploymentNoteLinks(IEnumerable<NewDeploymentNoteLink> noteLinks) => await new CreateNewUserEntityLinks(AppContext, RequestState).Perform(noteLinks);

        public async Task<ActionOutcome<bool>> MakeInactive(int userEntityLinkId) => await new MakeEntityLinkInactive(AppContext).Perform(userEntityLinkId);
    }
}