using System.Threading.Tasks;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Actions.Type {
    public class NewType : IAction<ApiNewType, Models.Type> {
        public NewType(DeploymentAppContext context) {
            Context = context;
        }

        private DeploymentAppContext Context { get; }

        public async Task<ApplicationActionResult<Models.Type>> Perform(ApiNewType newType) {
            await ValidateType(newType);

            var result = await Context.Types.AddAsync(new Models.Type {
                Name = newType.Name
            });

            await Context.SaveChangesAsync();

            return new ApplicationActionResult<Models.Type> {
                Succeeded = true,
                Result = result.Entity
            };
        }

        private async Task ValidateType(ApiNewType newType) {
            if (await Context.Types.AnyAsync(t => t.Name.ToLowerInvariant() == newType.Name.ToLowerInvariant())) {
                throw new ActionNotValidException("A type with a duplicate name cannot be added");
            }
        }
    }
}