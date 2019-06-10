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

using Microsoft.Extensions.Configuration;

using deployment_tracker.Models;

namespace deployment_tracker.Services.DeploymentManagement {
    public class JenkinsDeploymentManager : IDeploymentManager {
        private JenkinsDeploymentManagementConfiguration Configuration { get; }

        public JenkinsDeploymentManager(IConfiguration configuration) {
            Configuration = new JenkinsDeploymentManagementConfiguration(configuration);
        }

        public string GetTeardownUrl(IDeployedSite deployment) {
            return $"{Configuration.BaseUrl}/job/{Configuration.TeardownProject}/parambuild?{Configuration.TeardownSiteName}={deployment.SiteName}&{Configuration.TeardownDestroySql}=true";
        }
    }

    class JenkinsDeploymentManagementConfiguration {
        public JenkinsDeploymentManagementConfiguration(IConfiguration configuration) {
            var jenkinsConfiguration = configuration.GetSection("Jenkins");
            var teardownConfiguration = jenkinsConfiguration.GetSection("Teardown");
            var teardownParametersConfiguration = teardownConfiguration.GetSection("Parameters");

            BaseUrl = jenkinsConfiguration["BaseUrl"];
            TeardownProject = teardownConfiguration["ProjectName"];
            TeardownSiteName = teardownParametersConfiguration["SiteName"];
            TeardownDestroySql = teardownParametersConfiguration["DestroySql"];
        }

        public string BaseUrl { get; private set; }

        public string TeardownProject { get; private set; }
        public string TeardownSiteName { get; private set; }
        public string TeardownDestroySql { get; private set; }
    }
}