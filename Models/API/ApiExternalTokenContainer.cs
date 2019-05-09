using System.ComponentModel.DataAnnotations;

namespace deployment_tracker.Models.API {
    public class ApiExternalTokenContainer {
        [Required]
        public string Value { get; set; }
    }
}