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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Novell.Directory.Ldap;

namespace DeploymentTrackerCore.Services.Identity.LDAP.NetStandard {
    public class LDAPClient : ILDAPClient {
        private static readonly string[] RetrievedProperties = new string[] {
            LDAPProperties.UserName.Name,
            LDAPProperties.DisplayName.Name,
            LDAPProperties.Email.Name
        };

        public LDAPClient(LDAPv2Configuration configuration, ILogger<LDAPClient> logger) {
            Configuration = configuration;
            Logger = logger;
        }

        private LDAPv2Configuration Configuration { get; }

        private ILogger<LDAPClient> Logger { get; }

        public async Task<LDAPUserEntry> GetDetailsForUser(string userName) {
            try {
                var result = (await PerformSearch(GetFilterForUser(userName))).Single();

                return FromLDAPResult(result.GetAttributeSet());
            } catch (Exception exc) {
                Logger.LogError(exc, $"Error retrieving details for user '{userName}'");
            }

            return null;
        }

        public async Task<IEnumerable<LDAPUserEntry>> ListUsers() {
            try {
                return ConvertToListOfUserEntries(await PerformSearch(GetFilterForUser("*")));
            } catch (Exception exc) {
                Logger.LogError(exc, $"Error listing available users.");
            }

            return null;
        }

        public async Task<bool> Authenticate(string userName, string password) {
            try {
                await UsingConnection(GetUsernameWithDomain(userName), password, (_) => { });

                return true;
            } catch (LdapException exc) {
                Logger.LogError(exc, $"Error authenticating user '{userName}'");
            } catch (Exception exc) {
                Logger.LogError(exc, "Unknown LDAP error received.");
            }

            return false;
        }

        private async Task<List<LdapEntry>> PerformSearch(string userFilter) {
            List<LdapEntry> users = null;

            await UsingBindConnection(async(connection) => {
                users = await (await connection.SearchAsync(
                    Configuration.BaseDN,
                    LdapConnection.ScopeSub,
                    userFilter,
                    RetrievedProperties,
                    false)).ToListAsync();
            });

            return users;
        }

        private string GetFilterForUser(string userName) => string.Format(Configuration.UserFilter, userName);

        private string GetUsernameWithDomain(string userName) => userName.Contains("@") ? userName : $"{userName}{Configuration.DefaultUserDomain}";

        private static IEnumerable<LDAPUserEntry> ConvertToListOfUserEntries(List<LdapEntry> searchResults) {
            var results = new List<LDAPUserEntry>();

            foreach (var searchResult in searchResults) {
                results.Add(FromLDAPResult(searchResult.GetAttributeSet()));
            }

            return results;
        }

        private static LDAPUserEntry FromLDAPResult(LdapAttributeSet attributeSet) {
            if (attributeSet == null) {
                return null;
            }

            return new LDAPUserEntry {
                UserName = GetStringAttribute(attributeSet, LDAPProperties.UserName.Name),
                    DisplayName = GetStringAttribute(attributeSet, LDAPProperties.DisplayName.Name),
                    Email = GetStringAttribute(attributeSet, LDAPProperties.Email.Name)
            };
        }

        private static string GetStringAttribute(LdapAttributeSet attributeSet, string attributeName) => attributeSet.ContainsKey(attributeName) ? attributeSet.GetAttribute(attributeName).StringValue : null;

        private async Task UsingConnection(string bindUsername, string bindPassword, Action<LdapConnection> withConnection) {
            using var connection = new LdapConnection();

            connection.SecureSocketLayer = true;

            await connection.ConnectAsync(Configuration.Server, Configuration.Port);

            await connection.BindAsync(bindUsername, bindPassword);

            withConnection(connection);
        }

        private async Task UsingBindConnection(Action<LdapConnection> withConnection) => await UsingConnection(Configuration.BindUsername, Configuration.BindPassword, withConnection);
    }
}