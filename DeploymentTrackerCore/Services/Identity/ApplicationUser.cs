using System;
using System.Collections.Generic;
using System.Security.Claims;
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

        public ClaimsIdentity GetClaimIdentity(string authenticationScheme)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ApplicationClaims.FullName, Name),
                new Claim(ClaimTypes.Role, "Member")
            };

            return new ClaimsIdentity(claims, authenticationScheme);
        }
    }
}