

using System.Text.Json;

namespace DeploymentTrackerCore.Json {
    public static class DefaultJsonSerializerOptions {
        public static JsonSerializerOptions Options => new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }; 
    }
}