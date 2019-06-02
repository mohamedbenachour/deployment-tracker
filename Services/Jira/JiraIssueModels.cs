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

namespace deployment_tracker.Services.Jira {
    public class JiraIssueDetail {
        public JiraIssueFields Fields { get; set; }
    }

    public class JiraIssueFields {
        public JiraIssueStatus Status { get; set; }
    }

    public class JiraIssueStatus {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}