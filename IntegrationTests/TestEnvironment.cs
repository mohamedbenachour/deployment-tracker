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
using IntegrationTests.Helpers;

namespace IntegrationTests {
    public class TestEnvironment {
        public const string ExternalToken = "foobar";

        public struct URLs {
            public static readonly FluentUrlBuilder ApiBase = FluentUrlBuilder.WithRoot("api");

            public static readonly FluentUrlBuilder EnvironmentRoot = ApiBase.WithPart("environment");
            public static readonly string Environment = EnvironmentRoot.ToString();

            public static readonly FluentUrlBuilder DeploymentRoot = ApiBase.WithPart("deployment");
            public static readonly string Deployment = DeploymentRoot.ToString();

            public static readonly FluentUrlBuilder DeploymentExternalRoot = DeploymentRoot.WithPart("external");

            public static readonly string DeploymentExternal = DeploymentExternalRoot.ToString();
        }

        public static TestApplicationFactory<DeploymentTrackerCore.Startup> ClientFactory = new TestApplicationFactory<DeploymentTrackerCore.Startup>();
    }
}