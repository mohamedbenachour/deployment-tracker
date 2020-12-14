using DeploymentTrackerCore.Shared;

namespace DeploymentTrackerCore.Services.UserEntityLinks {
    public class LinkTypes : Enumeration {
        public static readonly LinkTypes Deployment = new LinkTypes(1, "Deployment");
        public static readonly LinkTypes DeploymentNote = new LinkTypes(2, "DeploymentNote");

        private LinkTypes(int id, string name) : base(id, name) {

        }
    }
}