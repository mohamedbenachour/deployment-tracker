using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Threading;

using deployment_tracker.Services.Identity;

namespace deployment_tracker.Services.Identity.Mock {
    public class MockUserStore : IUserStore<ApplicationUser> {
        public MockUserStore()
        {
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
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

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
            throw new NotImplementedException();
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

            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);

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