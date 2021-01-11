using DeploymentTrackerCore.Models.API;

namespace IntegrationTests.Helpers.DeploymentsApi.NotesApi {
    public class NoteUrlForDeployment {
        public static string GetForDeployment(ApiDeployment deployment) => $"{TestEnvironment.URLs.Deployment}/{deployment.Id}/note";
    }
}