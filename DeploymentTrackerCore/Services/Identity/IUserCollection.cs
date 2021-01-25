using System.Collections;
using System.Collections.Generic;

namespace DeploymentTrackerCore.Services.Identity {
    public interface IUserCollection {
        IEnumerable<ApplicationUser> ListUsers();
    }
}