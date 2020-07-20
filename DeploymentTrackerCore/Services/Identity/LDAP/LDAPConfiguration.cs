using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Services.Identity.LDAP
{
    public class LDAPConfiguration
    {
        private const string DefaultUserFilter = "(&(objectclass=user)(SAMAccountName={0}))";

        public LDAPConfiguration(IConfiguration configuration, ILogger<LDAPConfiguration> logger)
        {
            var ldapSection = configuration.GetSection("IdentitySource").GetSection("Ldap");

            if (ldapSection == null)
            {
                logger.LogError("No LDAP configuration has been specified.");

                throw new Exception("Unable to initialise LDAP configuration. No configuration present.");
            }
            else
            {
                PopulateFromSection(ldapSection);
            }
        }

        public string Server { get; private set; }

        public string BindUsername { get; private set; }
        public string BindPassword { get; private set; }

        public string UserFilter { get; private set; }

        private void PopulateFromSection(IConfigurationSection section)
        {
            Server = section[nameof(Server)];
            BindUsername = section[nameof(BindUsername)];
            BindPassword = section[nameof(BindPassword)];
            UserFilter = section[nameof(UserFilter)];

            if (String.IsNullOrWhiteSpace(UserFilter))
            {
                UserFilter = DefaultUserFilter;
            }
        }
    }
}