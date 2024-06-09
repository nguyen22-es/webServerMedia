using Common.DTOModels;
using System.Threading.Tasks;


namespace webServerMedia.Services
{
    public interface ITokenService
    {
        Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDto);
        Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDto, string userId);
    }
}
