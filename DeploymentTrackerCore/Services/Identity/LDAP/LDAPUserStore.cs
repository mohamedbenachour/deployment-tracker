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