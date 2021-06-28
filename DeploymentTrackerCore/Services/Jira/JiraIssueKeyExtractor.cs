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
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeploymentTrackerCore.Services.Jira {

    public class JiraIssueKeyExtractor {
        private ISet<string> ProjectKeys { get; }
        private string IssueKeyRegex { get; }

        public JiraIssueKeyExtractor(ISet<string> projectKey) {
            ProjectKeys = projectKey.Select(projectKey => projectKey.ToUpper()).ToImmutableHashSet();
            IssueKeyRegex = $"({String.Join("|", ProjectKeys)})-[0-9]+";
        }

        public string Extract(string source) {
            var match = Regex.Matches(source.ToUpper(), IssueKeyRegex);

            if (match.Any()) {
                return match.Last().Value;
            }

            return null;
        }
    }

}