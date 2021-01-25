using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeploymentTrackerCore.Services.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Services.Identity.Mock {
    public class MockUserStore : IUserCollection, IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser> {
        private IDictionary<String, ApplicationUser> UsersStore { get; set; } = new Dictionary<string, ApplicationUser>();
        private ILogger Logger { get; }

        public MockUserStore(IConfiguration configuration, ILogger<MockUserStore> logger) {
            Logger = logger;
            PopulateUsersStore(configuration);
        }

        private void PopulateUsersStore(IConfiguration configuration) {
            var userList = configuration.GetSection("IdentitySource").GetSection("Users").GetChildren();
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            Logger.LogInformation($"Adding {userList.Count()} users to the store");

            foreach (var user in userList) {
                var userName = user["Username"];

                Logger.LogInformation($"Adding user {userName}");

                UsersStore.Add(userName.ToUpperInvariant(), new ApplicationUser {
                    Id = userName,
                        UserName = userName,
                        Name = user["Name"],
                        PasswordHash = passwordHasher.HashPassword(null, user["Password"])
                });
            }

            Logger.LogInformation("Store has been populated");
        }

        #region createuser
        public Task<IdentityResult> CreateAsync(ApplicationUser user,
            CancellationToken cancellationToken = default(CancellationToken)) {
            cancellationToken.ThrowIfCancellationRequested();

            throw new NotImplementedException();
        }
        #endregion

        public Task<IdentityResult> DeleteAsync(ApplicationUser user,
            CancellationToken cancellationToken = default(CancellationToken)) {
            cancellationToken.ThrowIfCancellationRequested();

            throw new NotImplementedException();
        }

        public void Dispose() { }

        public async Task<ApplicationUser> FindByIdAsync(string userId,
            CancellationToken cancellationToken = default(CancellationToken)) {
            cancellationToken.ThrowIfCancellationRequested();

            return await FindByNameAsync(userId, cancellationToken);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName,
            CancellationToken cancellationToken = default(CancellationToken)) {
            cancellationToken.ThrowIfCancellationRequested();
            if (userName == null)throw new ArgumentNullException(nameof(userName));

            var userNameToFind = userName.ToUpperInvariant();

            Logger.LogDebug($"Finding user {userNameToFind}");

            if (!UsersStore.ContainsKey(userNameToFind)) {
                return Task.FromResult((ApplicationUser)null);
            }

            return Task.FromResult(UsersStore[userNameToFind]);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)throw new ArgumentNullException(nameof(user));

            Logger.LogDebug($"Getting password for {user.UserName}");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken) {
            Logger.LogDebug($"Has password for {user.UserName}");

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)throw new ArgumentNullException(nameof(user));
            if (normalizedName == null)throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)throw new ArgumentNullException(nameof(user));
            if (passwordHash == null)throw new ArgumentNullException(nameof(passwordHash));

            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> ListUsers() => UsersStore.Values;
    }
}