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
using System.DirectoryServices;
using SearchScope = System.DirectoryServices.SearchScope;

namespace DeploymentTrackerCore.Services.Identity.LDAP {
    public class LDAPClient {
        private static readonly string[] RetrievedProperties = new string[] {
            LDAPProperties.UserName.Name,
            LDAPProperties.DisplayName.Name,
            LDAPProperties.Email.Name
        };

        public LDAPClient(LDAPConfiguration configuration) {
            Configuration = configuration;
        }

        private LDAPConfiguration Configuration { get; }

        public LDAPUserEntry GetDetailsForUser(string userName) {
            try {
                using(var dirEntry = ConstructDirectoryEntry(Configuration.BindUsername, Configuration.BindPassword)) {
                    DirectorySearcher ds = GetSearcher(dirEntry);

                    ds.Filter = String.Format(Configuration.UserFilter, userName);

                    var result = ds.FindOne();

                    return FromLDAPResult(result?.Properties);
                }
            } catch (Exception) { }

            return null;
        }

        public bool Authenticate(string userName, string password) {
            try {
                using(var dirEntry = ConstructDirectoryEntry(userName, password)) {
                    DirectorySearcher ds = GetSearcher(dirEntry);

                    ds.FindOne();
                }
                return true;
            } catch (Exception) { }

            return false;
        }

        private static LDAPUserEntry FromLDAPResult(ResultPropertyCollection propertyCollection) {
            if (propertyCollection == null) {
                return null;
            }

            return new LDAPUserEntry {
                UserName = GetProperty(propertyCollection, LDAPProperties.UserName.Name),
                    DisplayName = GetProperty(propertyCollection, LDAPProperties.DisplayName.Name),
                    Email = GetProperty(propertyCollection, LDAPProperties.Email.Name)
            };
        }

        private static string GetProperty(ResultPropertyCollection collection, string propertyName) => (string)(collection[propertyName])[0];

        private DirectorySearcher GetSearcher(DirectoryEntry directoryEntry) {
            var searcher = new DirectorySearcher(directoryEntry, String.Empty, RetrievedProperties);

            searcher.SearchScope = SearchScope.Subtree;

            return searcher;
        }

        private DirectoryEntry ConstructDirectoryEntry(string userName, string password) => new DirectoryEntry(Configuration.Server, userName, password);
    }
}