using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using deployment_tracker.Services.Identity;

namespace deployment_tracker.Services.Identity.Mock {
    public class MockUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser> {
        private IDictionary<String, ApplicationUser> UsersStore { get; set; } = new Dictionary<string, ApplicationUser>();
        private ILogger Logger { get; }

        public MockUserStore(IConfiguration configuration, ILogger<MockUserStore> logger)
        {
            Logger = logger;
            PopulateUsersStore(configuration);
        }

        private void PopulateUsersStore(IConfiguration configuration) {
            var userList = configuration.GetSection("IdentitySource").GetSection("Users").GetChildren();
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            Logger.LogInformation($"Adding {userList.Count()} users to the store");

            foreach(var user in userList) {
                var userName = user["Username"].ToUpper();

                Logger.LogInformation($"Adding user {userName}");

                UsersStore.Add(userName, new ApplicationUser {
                    Id = userName,
                    UserName = userName,
                    Name = user["Name"],
                    PasswordHash = passwordHasher.HashPassword(null, user["Password"])
                });
            }

            Logger.LogInformation("Store has been populated");
        }
        
        #region createuser
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new NotImplementedException();
        }
        #endregion

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await FindByNameAsync(userId, cancellationToken);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            Logger.LogDebug($"Finding user {userName}");

            if (!UsersStore.ContainsKey(userName)) {
                return Task.FromResult((ApplicationUser) null);
            }

            return Task.FromResult(UsersStore[userName]);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            Logger.LogDebug($"Getting password for {user.UserName}");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Logger.LogDebug($"Has password for {user.UserName}");

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
}
    }
}