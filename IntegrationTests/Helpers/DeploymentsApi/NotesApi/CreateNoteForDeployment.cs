using System.Net.Http;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Models.API.DeploymentNotes;

namespace IntegrationTests.Helpers.DeploymentsApi.NotesApi {
    public class CreateNoteForDeployment {
        public static async Task<ApiNote> Create(HttpClient client, ApiDeployment deployment, string content) => (await (await client.PostJsonAsync(
            NoteUrlForDeployment.GetForDeployment(deployment),
            new ApiNewNote {
                Content = content
            }
        )).AssertSuccessfulResponseAndGetContent<ApiNote>());

    }
}