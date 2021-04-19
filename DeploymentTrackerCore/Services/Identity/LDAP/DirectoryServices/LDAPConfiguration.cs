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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Services.Identity.LDAP.DirectoryServices {
    public class LDAPConfiguration {
        private const string DefaultUserFilter = "(&(objectclass=user)(SAMAccountName={0}))";

        public LDAPConfiguration(IConfiguration configuration, ILogger<LDAPConfiguration> logger) {
            var ldapSection = configuration.GetSection("IdentitySource").GetSection("Ldap");

            if (ldapSection == null) {
                logger.LogError("No LDAP configuration has been specified.");

                throw new Exception("Unable to initialise LDAP configuration. No configuration present.");
            } else {
                PopulateFromSection(ldapSection);
            }
        }

        public string Server { get; private set; }

        public string BindUsername { get; private set; }
        public string BindPassword { get; private set; }

        public string UserFilter { get; private set; }

        private void PopulateFromSection(IConfigurationSection section) {
            Server = section[nameof(Server)];
            BindUsername = section[nameof(BindUsername)];
            BindPassword = section[nameof(BindPassword)];
            UserFilter = section[nameof(UserFilter)];

            if (String.IsNullOrWhiteSpace(UserFilter)) {
                UserFilter = DefaultUserFilter;
            }
        }
    }
}