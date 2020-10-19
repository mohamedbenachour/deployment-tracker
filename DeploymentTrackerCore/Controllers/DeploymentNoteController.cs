using System.Collections.Generic;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API.DeploymentNotes;
using DeploymentTrackerCore.Models.Entities;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.DeploymentNotes;
using DeploymentTrackerCore.Services.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeploymentTrackerCore.Controllers {
    [Route("api/deployment/{deploymentId}/note")]
    [ApiController]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class DeploymentNoteController : BaseApiController {
        public DeploymentNoteController(
            IDeploymentNotesService notesService,
            IRequestState requestState,
            UserManager<ApplicationUser> userManager) : base(requestState, userManager) {
            NotesService = notesService;
        }

        private IDeploymentNotesService NotesService { get; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiNote>>> GetNotesForDeployment(int deploymentId) {
            return await Handle(() => NotesService.ListNotes(deploymentId));
        }

        [HttpPost]
        public async Task<ActionResult<ApiNote>> CreateNoteForDeployment(int deploymentId, ApiNewNote newNote) {
            return await Handle(() => NotesService.CreateNewNote(deploymentId, newNote));
        }
    }
}