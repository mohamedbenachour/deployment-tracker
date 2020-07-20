using System;
using System.DirectoryServices;
using SearchScope = System.DirectoryServices.SearchScope;

namespace DeploymentTrackerCore.Services.Identity.LDAP
{
    public class LDAPClient
    {
        public LDAPClient(LDAPConfiguration configuration)
        {
            Configuration = configuration;
        }

        private LDAPConfiguration Configuration { get; }

        public ResultPropertyCollection GetDetailsForUser(string userName)
        {
            try
            {
                using (var dirEntry = ConstructDirectoryEntry(Configuration.BindUsername, Configuration.BindPassword))
                {
                    DirectorySearcher ds = new DirectorySearcher(dirEntry);
                    ds.SearchScope = SearchScope.Subtree;
                    ds.Filter = String.Format(Configuration.UserFilter, userName);

                    var result = ds.FindOne();

                    return result?.Properties;
                }
            }
            catch (Exception) { }

            return null;
        }

        public bool Authenticate(string userName, string password)
        {
            try
            {
                using (var dirEntry = ConstructDirectoryEntry(userName, password))
                {
                    DirectorySearcher ds = new DirectorySearcher(dirEntry);
                    ds.SearchScope = SearchScope.Subtree;

                    ds.FindOne();
                }
                return true;
            }
            catch (Exception) { }

            return false;
        }

        private DirectoryEntry ConstructDirectoryEntry(string userName, string password) => new DirectoryEntry(Configuration.Server, userName, password);
    }
}