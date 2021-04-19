using System.Collections.Generic;

namespace DeploymentTrackerCore.Services.Identity.LDAP {
    public interface ILDAPClient {
        LDAPUserEntry GetDetailsForUser(string userName);
        bool Authenticate(string userName, string password);
        IEnumerable<LDAPUserEntry> ListUsers();

    }
}