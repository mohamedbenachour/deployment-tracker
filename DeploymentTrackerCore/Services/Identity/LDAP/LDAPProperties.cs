using DeploymentTrackerCore.Shared;

namespace DeploymentTrackerCore.Services.Identity.LDAP {
    public class LDAPProperties : Enumeration {
        public static readonly LDAPProperties UserName = new(1, "samaccountname");
        public static readonly LDAPProperties DisplayName = new(2, "displayname");
        public static readonly LDAPProperties Email = new(3, "mail");

        private LDAPProperties(int id, string name) : base(id, name) {

        }
    }
}