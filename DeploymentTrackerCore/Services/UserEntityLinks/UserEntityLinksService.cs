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
    }
}