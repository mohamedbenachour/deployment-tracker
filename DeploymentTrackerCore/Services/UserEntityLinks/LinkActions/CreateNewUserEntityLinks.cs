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