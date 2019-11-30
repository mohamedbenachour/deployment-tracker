using System;
using Microsoft.Extensions.Configuration;

namespace DeploymentTrackerCore.Services.Configuration {
    public class ConfigurationService {
        private bool AllowManualDeploymentsToBeAdded { get; }

        public ConfigurationService(IConfiguration configuration) {
            AllowManualDeploymentsToBeAdded = Boolean.Parse(configuration[nameof(AllowManualDeploymentsToBeAdded)]);
        }

        public bool GetAllowManualDeploymentsToBeAdded() => AllowManualDeploymentsToBeAdded;
    }
}