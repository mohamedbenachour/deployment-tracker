using System;
using System.Collections.Generic;
using System.Linq;
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

        public ClaimsIdentity GetClaimIdentity (string authenticationScheme) {
            var claims = new List<Claim> {
                new Claim (ClaimTypes.Actor, UserName),
                new Claim (ClaimTypes.Name, Name),
                new Claim (ClaimTypes.Role, "Member"),
                new Claim (ClaimTypes.Email, Email ?? String.Empty)
            };

            return new ClaimsIdentity (claims, authenticationScheme);
        }

        public static ApplicationUser FromClaimsPrincipal (ClaimsPrincipal claimsPrincipal) => new ApplicationUser {
            UserName = GetClaimValue (claimsPrincipal, ClaimTypes.Actor),
            Name = GetClaimValue (claimsPrincipal, ClaimTypes.Name),
            Email = GetClaimValue (claimsPrincipal, ClaimTypes.Email)
        };

        private static string GetClaimValue (ClaimsPrincipal claimsPrincipal, string claimName) => claimsPrincipal.Claims.SingleOrDefault (claim => claim.Type == claimName)?.Value;
    }
}