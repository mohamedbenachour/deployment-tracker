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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Services.UserEntityLinks.LinkActions {
    public class CreateNewUserEntityLinks : IResultBasedAction<IEnumerable<NewDeploymentNoteLink>, int> {
        public CreateNewUserEntityLinks(DeploymentAppContext context, IRequestState requestState) {
            AppContext = context;
            RequestState = requestState;
        }

        private DeploymentAppContext AppContext { get; }
        private IRequestState RequestState { get; }

        public async Task<ActionOutcome<int>> Perform(IEnumerable<NewDeploymentNoteLink> links) {
            var linksToCreate = links.Select(CreateLink);

            AppContext.UserEntityLinks.AddRange(linksToCreate);

            await AppContext.SaveChangesAsync();

            return ActionOutcome<int>.WithResult(links.Count());
        }

        private UserEntityLink CreateLink(NewDeploymentNoteLink newLink) => new UserEntityLink {
            IsActive = true,
            ReferencedEntity = GetEntity(newLink.DeploymentId, newLink.DeploymentNoteId),
            TargetUserName = newLink.TargetingUserName
        };

        private string GetEntity(int deploymentId, int deploymentNoteId) => $"{LinkTypes.Deployment}::{deploymentId}::{LinkTypes.DeploymentNote}::{deploymentNoteId}";
    }
}