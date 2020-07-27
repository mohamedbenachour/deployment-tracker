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

using System.Web;
using DeploymentTrackerCore.Models;

namespace DeploymentTrackerCore.Services.DeploymentManagement.TypeBased
{
    internal class DeployedSiteStringTemplater
    {
        public string Template(IDeployedSite site, string template)
            => template.Replace("{{SiteName}}", HttpUtility.UrlEncode(site.SiteName));
    }
}