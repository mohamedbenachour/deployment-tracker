using System;
using System.Security.Principal;

namespace DeploymentTrackerCore.Services.Identity {
    public class ApplicationUser : IIdentity {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual String PasswordHash { get; set; }
        public virtual string NormalizedUserName { get; set; }
        public string Name { get; set; }

        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}