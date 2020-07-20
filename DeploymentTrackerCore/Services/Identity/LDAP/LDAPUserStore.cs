using System.DirectoryServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Services.Identity.LDAP
{
    public class LDAPUserStore : IUserStore<ApplicationUser>
    {
        public LDAPUserStore(LDAPClient ldapClient, ILogger<LDAPUserStore> logger)
        {
            LDAPClient = ldapClient;
            Logger = logger;
        }

        private LDAPClient LDAPClient { get; }
        private ILogger<LDAPUserStore> Logger { get; }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {

        }

        private Task<ApplicationUser> GetUser(string userName)
        {
            var userProperties = LDAPClient.GetDetailsForUser(userName);

            if (userProperties == null)
            {
                return Task.FromResult<ApplicationUser>(null);
            }

            return Task.FromResult(ConvertToUser(userProperties));
        }

        private ApplicationUser ConvertToUser(ResultPropertyCollection propertyCollection)
        {
            var userName = GetProperty(propertyCollection, "samaccountname");

            return new ApplicationUser
            {
                UserName = userName,
                NormalizedUserName = userName,
                Email = GetProperty(propertyCollection, "mail"),
                Name = GetProperty(propertyCollection, "displayname"),
            };
        }

        private static string GetProperty(ResultPropertyCollection collection, string propertyName) => (string)(collection[propertyName])[0];

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return FindByNameAsync(userId, cancellationToken);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return GetUser(normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return GetUserNameAsync(user, cancellationToken);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return GetUserNameAsync(user, cancellationToken);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}