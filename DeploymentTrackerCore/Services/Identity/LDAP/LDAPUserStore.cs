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
using System.DirectoryServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeploymentTrackerCore.Services.Identity.LDAP.DirectoryServices;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Services.Identity.LDAP {
    public class LDAPUserStore : IUserStore<ApplicationUser>, IUserCollection {
        public LDAPUserStore(ILDAPClient ldapClient, ILogger<LDAPUserStore> logger) {
            LDAPClient = ldapClient;
            Logger = logger;
        }

        private ILDAPClient LDAPClient { get; }
        private ILogger<LDAPUserStore> Logger { get; }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }

        public void Dispose() {

        }

        private Task<ApplicationUser> GetUser(string userName) {
            var userEntry = LDAPClient.GetDetailsForUser(userName);

            if (userEntry == null) {
                return Task.FromResult<ApplicationUser>(null);
            }

            return Task.FromResult(ConvertToUser(userEntry));
        }

        private ApplicationUser ConvertToUser(LDAPUserEntry userEntry) => new ApplicationUser {
            UserName = userEntry.UserName,
            NormalizedUserName = userEntry.UserName,
            Email = userEntry.Email,
            Name = userEntry.DisplayName,
        };

        private static string GetProperty(ResultPropertyCollection collection, string propertyName) => (string)(collection[propertyName])[0];

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken) {
            return FindByNameAsync(userId, cancellationToken);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
            return GetUser(normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return GetUserNameAsync(user, cancellationToken);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return GetUserNameAsync(user, cancellationToken);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ApplicationUser> ListUsers() => LDAPClient.ListUsers().Select(ConvertToUser).Where(user => !String.IsNullOrWhiteSpace(user.UserName));
    }
}