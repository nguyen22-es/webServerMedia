using Common.DTOModels;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Database.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetUserAsync(string userId);
        Task<UserDTO> GetUserEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(RegisterUserDTO user);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> DeleteUserAsync(string userId);
        Task<VODUser> GetUserAsync(LoginUserDTO loginUser, bool includeClaims = false);
    }
}
