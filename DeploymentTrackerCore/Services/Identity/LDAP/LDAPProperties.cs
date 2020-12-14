using DeploymentTrackerCore.Shared;

namespace DeploymentTrackerCore.Services.Identity.LDAP {
    public class LDAPProperties : Enumeration {
        public static readonly LDAPProperties UserName = new LDAPProperties(1, "samaccountname");
        public static readonly LDAPProperties DisplayName = new LDAPProperties(2, "displayname");
        public static readonly LDAPProperties Email = new LDAPProperties(3, "email");

        private LDAPProperties(int id, string name) : base(id, name) {

        }
    }
}