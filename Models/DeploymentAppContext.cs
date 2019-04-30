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

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace deployment_tracker.Models
{
    public class DeploymentAppContext : DbContext
    {
        public DeploymentAppContext(DbContextOptions<DeploymentAppContext> options)
            : base(options)
        { }

        public DbSet<DeploymentEnvironment> Environments { get; set; }
        public DbSet<Deployment> Deployments { get; set; }
    }

    public enum DeploymentStatus {
        RUNNING,
        DESTROYED
    }

    public class Deployment {
        public int Id {get; set; }
        public virtual DeploymentEnvironment DeployedEnvironment {get; set; }
        public string BranchName {get; set; }

        public string PublicURL { get; set; }

        public DeploymentStatus Status {get; set;}
    }

    public class DeploymentEnvironment {
        public int Id {get; set;}

        public string HostName { get; set; }
        public string Name {get; set; }

        public virtual IList<Deployment> Deployments {get; set;} = new List<Deployment>();
    }
}