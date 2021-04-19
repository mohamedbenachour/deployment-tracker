using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeploymentTrackerCore.Services.Identity {
    public interface IUserCollection {
        Task<IEnumerable<ApplicationUser>> ListUsers();
    }
}