using Microsoft.AspNetCore.Identity;
using RealEstatePipeline.Model;

namespace RealEstatePipeline.Services
{
    public class ClientService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ClientService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetClientByIdAsync(string clientId)
        {
            var user = await _userManager.FindByIdAsync(clientId);
            if (user != null)
            {
                var isClient = await _userManager.IsInRoleAsync(user, "Client");
                if (isClient)
                {
                    return user; // User is a client, return their information
                }
            }
            return null; // User is not a client or does not exist
        }

    }
}
