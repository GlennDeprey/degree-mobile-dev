using Mde.Project.Api.Services.Interfaces;
using Mde.Project.Api.Core.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mde.Project.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private async Task<List<Claim>> GenerateClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>();
            // Loading the user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Add claims incase of seed data
            if (!userClaims.Any(claim => claim.Type == ClaimTypes.NameIdentifier))
            {
                var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
                await _userManager.AddClaimAsync(user, claim);
                claims.Add(claim);
            }

            if (!userClaims.Any(claim => claim.Type == ClaimTypes.GivenName))
            {
                var claim = new Claim(ClaimTypes.GivenName, user.FirstName);
                await _userManager.AddClaimAsync(user, claim);
                claims.Add(claim);
            }
            if (!userClaims.Any(claim => claim.Type == ClaimTypes.Surname))
            {
                var claim = new Claim(ClaimTypes.Surname, user.LastName);
                await _userManager.AddClaimAsync(user, claim);
                claims.Add(claim);
            }
            if (!userClaims.Any(claim => claim.Type == ClaimTypes.Email))
            {
                var claim = new Claim(ClaimTypes.Email, user.Email);
                await _userManager.AddClaimAsync(user, claim);
                claims.Add(claim);
            }
            // Loading the roles and put them in a claim of a Role ClaimType
            var roleClaims = await _userManager.GetRolesAsync(user);
            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim));
            }
            return claims;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireExpirationTime = true,
                ValidIssuer = _configuration["JWTConfiguration:Issuer"],
                ValidAudience = _configuration["JWTConfiguration:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["JWTConfiguration:SigningKey"]))
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = await GenerateClaimsAsync(user);
            var expirationDays = _configuration.GetValue<int>("JWTConfiguration:TokenExpirationDays");
            var signInKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTConfiguration:SigningKey"));
            var token = new JwtSecurityToken
            (
            issuer: _configuration.GetValue<string>("JWTConfiguration:Issuer"),
            audience: _configuration.GetValue<string>("JWTConfiguration:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signInKey),
            SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
