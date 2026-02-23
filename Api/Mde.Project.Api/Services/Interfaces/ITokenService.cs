using Mde.Project.Api.Core.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mde.Project.Api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user);
        ClaimsPrincipal ValidateToken(string token);
    }
}
