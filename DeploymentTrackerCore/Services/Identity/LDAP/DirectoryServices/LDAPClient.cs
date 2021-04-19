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
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using SearchScope = System.DirectoryServices.SearchScope;

namespace DeploymentTrackerCore.Services.Identity.LDAP.DirectoryServices {
    public class LDAPClient : ILDAPClient {
        private static readonly string[] RetrievedProperties = new string[] {
            LDAPProperties.UserName.Name,
            LDAPProperties.DisplayName.Name,
            LDAPProperties.Email.Name
        };

        public LDAPClient(LDAPConfiguration configuration, ILogger<LDAPClient> logger) {
            Configuration = configuration;
            Logger = logger;
        }

        private LDAPConfiguration Configuration { get; }

        private ILogger<LDAPClient> Logger { get; }

        public Task<LDAPUserEntry> GetDetailsForUser(string userName) {
            try {
                using var dirEntry = ConstructDirectoryEntry(Configuration.BindUsername, Configuration.BindPassword);
                DirectorySearcher ds = GetSearcher(dirEntry);

                ds.Filter = string.Format(Configuration.UserFilter, userName);

                var result = ds.FindOne();

                return Task.FromResult(FromLDAPResult(result?.Properties));
            } catch (Exception exc) {
                Logger.LogError(exc, $"Error retrieving details for user '{userName}'");
            }

            return Task.FromResult<LDAPUserEntry>(null);
        }

        public Task<IEnumerable<LDAPUserEntry>> ListUsers() {
            try {
                using var dirEntry = ConstructDirectoryEntry(Configuration.BindUsername, Configuration.BindPassword);
                DirectorySearcher ds = GetSearcher(dirEntry);

                var results = ds.FindAll();

                return Task.FromResult(ConvertToListOfUserEntries(results));
            } catch (Exception exc) {
                Logger.LogError(exc, $"Error listing available users.");
            }

            return Task.FromResult<IEnumerable<LDAPUserEntry>>(null);
        }

        public Task<bool> Authenticate(string userName, string password) {
            try {
                using(var dirEntry = ConstructDirectoryEntry(userName, password)) {
                    FetchPropertyToValidateUserCredentials(dirEntry);
                }
                return Task.FromResult(true);
            } catch (Exception exc) {
                Logger.LogError(exc, $"Error authenticating user '{userName}'");
            }

            return Task.FromResult(false);
        }

        private static void FetchPropertyToValidateUserCredentials(DirectoryEntry directoryEntry) {
            var _ = directoryEntry.NativeGuid;
        }

        private IEnumerable<LDAPUserEntry> ConvertToListOfUserEntries(SearchResultCollection searchResults) {
            var results = new List<LDAPUserEntry>();

            foreach (SearchResult searchResult in searchResults) {
                results.Add(FromLDAPResult(searchResult?.Properties));
            }

            return results;
        }

        private static LDAPUserEntry FromLDAPResult(ResultPropertyCollection propertyCollection) {
            if (propertyCollection == null) {
                return null;
            }

            var propertyDictionary = PropertyCollectionToDictionary(propertyCollection);

            return new LDAPUserEntry {
                UserName = GetProperty(propertyDictionary, LDAPProperties.UserName.Name),
                    DisplayName = GetProperty(propertyDictionary, LDAPProperties.DisplayName.Name),
                    Email = GetProperty(propertyDictionary, LDAPProperties.Email.Name)
            };
        }

        private static string GetProperty(IDictionary<string, object> properties, string propertyName) {
            if (properties.ContainsKey(propertyName)) {
                return (string)properties[propertyName];
            }

            return null;

        }

        private static IDictionary<string, object> PropertyCollectionToDictionary(ResultPropertyCollection collection) {
            var result = new Dictionary<string, object>();

            foreach (string propertyName in collection.PropertyNames) {
                result[propertyName] = collection[propertyName][0];
            }

            return result;
        }

        private static DirectorySearcher GetSearcher(DirectoryEntry directoryEntry) {
            var searcher = new DirectorySearcher(directoryEntry, null, RetrievedProperties);

            searcher.SearchScope = SearchScope.Subtree;

            return searcher;
        }

        private DirectoryEntry ConstructDirectoryEntry(string userName, string password) => new DirectoryEntry(Configuration.Server, userName, password);
    }
}