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

using IntegrationTests.EnvironmentSetup;

namespace IntegrationTests {
    public class TestEnvironment {
        public const string ExternalToken = "foobar";

        public struct URLs {
            public const string Environment = "/api/environment";
            public const string Deployment = "/api/deployment";

            public const string DeploymentExternal = "/api/deployment/external";
        }

        public static TestApplicationFactory<DeploymentTrackerCore.Startup> ClientFactory = new TestApplicationFactory<DeploymentTrackerCore.Startup>();
    }
}