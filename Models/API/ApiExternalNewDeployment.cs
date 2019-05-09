using System.ComponentModel.DataAnnotations;

namespace deployment_tracker.Models.API {
    public class ApiExternalNewDeployment : ApiNewDeployment {
        [Required]
        public ApiUser User { get; set; }

        [Required]
        public ApiExternalTokenContainer Token { get; set; }
    }
}