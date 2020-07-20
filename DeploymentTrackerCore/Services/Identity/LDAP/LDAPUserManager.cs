using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeploymentTrackerCore.Services.Identity.LDAP
{
    public class LDAPUserManager : UserManager<ApplicationUser>
    {
        public LDAPUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger, LDAPClient ldapClient) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            LDAPClient = ldapClient;
        }

        private LDAPClient LDAPClient { get; }

        public override Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return Task.FromResult(LDAPClient.Authenticate(user.UserName, password));
        }
    }
}