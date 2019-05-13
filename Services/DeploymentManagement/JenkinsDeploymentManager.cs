using Microsoft.Extensions.Configuration;

using deployment_tracker.Models.API;

namespace deployment_tracker.Services.DeploymentManagement {
    public class JenkinsDeploymentManager : IDeploymentManager {
        private JenkinsDeploymentManagementConfiguration Configuration { get; }

        public JenkinsDeploymentManager(IConfiguration configuration) {
            Configuration = new JenkinsDeploymentManagementConfiguration(configuration);
        }

        public string GetTeardownUrl(ApiDeployment deployment) {
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