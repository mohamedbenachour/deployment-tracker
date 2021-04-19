using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeploymentTrackerCore.Services.Identity.LDAP {
    public interface ILDAPClient {
        Task<LDAPUserEntry> GetDetailsForUser(string userName);
        Task<bool> Authenticate(string userName, string password);
        Task<IEnumerable<LDAPUserEntry>> ListUsers();

    }
}