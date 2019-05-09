using System.ComponentModel.DataAnnotations;

namespace deployment_tracker.Models.API {
    public class ApiExternalDeploymentDestroyed : ApiDeploymentDestroyed {
        [Required]
        public ApiUser User { get; set; }
        [Required]
        public ApiExternalTokenContainer Token { get; set; }
    }
}