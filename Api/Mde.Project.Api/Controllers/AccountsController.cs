using Mde.Project.Api.Dtos.Accounts;
using Mde.Project.Api.Services.Interfaces;
using Mde.Project.Api.Core.Entities.Users;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Core.Services.Files;

namespace Mde.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IFilesService _filesService;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMemoryCache memoryCache, IEmailService emailService, ITokenService tokenService, IFilesService filesService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _tokenService = tokenService;
            _filesService = filesService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var externalLogins = await _userManager.GetLoginsAsync(user);
            var userDto = new UserDto
            {
                Id = user.Id,
                ImageUrl = user.ImageUrl,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles,
                ExternalAccounts = externalLogins
            };
            return Ok(userDto);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (profilePicture != null)
            {
                if (!_filesService.IsValidImageFile(profilePicture.FileName, profilePicture.ContentType))
                {
                    return BadRequest("Invalid image file type.");
                }
                var savedFilePath = "";

                using (var stream = profilePicture.OpenReadStream())
                {
                    savedFilePath = await _filesService.SaveFileAsync(stream, profilePicture.FileName);
                }

                if (!string.IsNullOrEmpty(savedFilePath))
                {
                    user.ImageUrl = Path.GetFileName(savedFilePath);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList()[0].Description);
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDto registerUserRequestDto)
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); 
            }

            ApplicationUser newUser = new ApplicationUser
            {
                Email = registerUserRequestDto.Email,
                UserName = registerUserRequestDto.Email,
                FirstName = registerUserRequestDto.FirstName,
                LastName = registerUserRequestDto.LastName,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, registerUserRequestDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            newUser = await _userManager.FindByEmailAsync(registerUserRequestDto.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id),
                new Claim("registration-date", DateTime.UtcNow.ToString("yy-MM-dd")),
                new Claim(ClaimTypes.GivenName, registerUserRequestDto.FirstName),
                new Claim(ClaimTypes.Surname, registerUserRequestDto.LastName),
                new Claim(ClaimTypes.Email, registerUserRequestDto.Email)
            };
            await _userManager.AddClaimsAsync(newUser, claims);

            await _userManager.AddToRoleAsync(newUser, "Customer");

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserRequestDto login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false,
            lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            var applicationUser = await _userManager.FindByEmailAsync(login.Email);
            JwtSecurityToken token = await _tokenService.GenerateTokenAsync(applicationUser);

            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginUserResponseDto()
            {
                Token = serializedToken
            });
        }

        [HttpGet("google/login")]
        public async Task<IActionResult> GoogleLogin([FromQuery] bool isLogin = false, [FromQuery] string redirectUri = null, [FromQuery] string token = null)
        {
            var props = new AuthenticationProperties();

            if (!string.IsNullOrWhiteSpace(redirectUri))
            {
                props.Items["redirectUri"] = redirectUri;
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                props.Items["token"] = token;
            }

            props.RedirectUri = isLogin
                ? Url.Action("GoogleCallBack", "Accounts")
                : Url.Action("GoogleLink", "Accounts");

            return Challenge(props, new[] { GoogleDefaults.AuthenticationScheme });
        }

        [HttpGet("google/login/callback")]
        public async Task<IActionResult> GoogleCallBack()
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var redirectUri = "myapp://";
            if (authResult.Properties.Items.ContainsKey("redirectUri"))
            {
                redirectUri = authResult.Properties?.Items["redirectUri"];
            }

            if (!authResult.Succeeded)
            {
                string errorMessage = "Authentication failure.. Please try again later";
                return Redirect($"{redirectUri}?error={WebUtility.UrlEncode(errorMessage)}");
            }

            var providerKey = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByLoginAsync(GoogleDefaults.AuthenticationScheme, providerKey);
            if (user == null)
            {
                string errorMessage = "No Stock Flow accounts linked to this Google account.";
                return Redirect($"{redirectUri}?error={WebUtility.UrlEncode(errorMessage)}");
            }

            var token = await _tokenService.GenerateTokenAsync(user);
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Redirect($"{redirectUri}?access_token={WebUtility.UrlEncode(serializedToken)}");
        }

        [HttpDelete("google/link/remove")]
        [Authorize]
        public async Task<IActionResult> GoogleLinkRemove()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var logins = await _userManager.GetLoginsAsync(user);
            var googleLogin = logins.FirstOrDefault(l => l.LoginProvider == GoogleDefaults.AuthenticationScheme);

            if (googleLogin == null)
            {
                return BadRequest("Google account is not linked to this user.");
            }

            var providerKey = googleLogin.ProviderKey;

            var result = await _userManager.RemoveLoginAsync(user, GoogleDefaults.AuthenticationScheme, providerKey);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"Failed to unlink Google account. Errors: {errors}");
            }

            return Ok();
        }

        [HttpGet("google/link/callback")]
        public async Task<IActionResult> GoogleLink()
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var redirectUri = "myapp://";
            if (authResult.Properties.Items.ContainsKey("redirectUri"))
            {
                redirectUri = authResult.Properties?.Items["redirectUri"];
            }

            var token = "";
            if (authResult.Properties.Items.ContainsKey("token"))
            {
                token = authResult.Properties?.Items["token"];
            }
            else
            {
                return BadRequest("No valid token provided");
            }

            if (!authResult.Succeeded)
            {
                string errorMessage = "Authentication failure. Please try again later.";
                return Redirect($"myapp://?error={errorMessage}");
            }

            if (string.IsNullOrEmpty(token))
            {
                string errorMessage = "No token provided.";
                return Redirect($"{redirectUri}?error={errorMessage}");
            }

            var principal = _tokenService.ValidateToken(token);
            if (principal == null)
            {
                string errorMessage = "Invalid token.";
                return Redirect($"myapp://?error={errorMessage}");
            }

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                string errorMessage = "User not found.";
                return Redirect($"myapp://?error={errorMessage}");
            }

            var providerKey = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingUser = await _userManager.FindByLoginAsync(GoogleDefaults.AuthenticationScheme, providerKey);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                string errorMessage = "This Google account is already linked to another user.";
                return Redirect($"{redirectUri}?error={errorMessage}");
            }

            var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(GoogleDefaults.AuthenticationScheme, providerKey, "Google"));
            if (!result.Succeeded)
            {
                string errorMessage = "Failed to link Google account.";
                return Redirect($"{redirectUri}?error={errorMessage}");
            }

            return Redirect($"{redirectUri}?success=true");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email address.");
            }
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequestDto.Token));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordRequestDto.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordRequestDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email address.");
            }

            // Generate a 6-digit code
            var code = new Random().Next(100000, 999999).ToString();

            // Store the code in the cache with an expiration time (e.g., 15 minutes)
            _memoryCache.Set(user.Email, code, TimeSpan.FromMinutes(15));

            var fullName = $"{user.FirstName} {user.LastName}";
            // Send the code to the user via email
            var result = await _emailService.SendPasswordResetCodeAsync(fullName, user.Email, code);

            return Ok();
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(VerifyCodeRequestDto verifyCodeRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(verifyCodeRequestDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email address.");
            }

            // Retrieve the stored code from the cache
            if (_memoryCache.TryGetValue(verifyCodeRequestDto.Email, out string storedCode))
            {
                if (storedCode == verifyCodeRequestDto.Code)
                {
                    try
                    {
                        _memoryCache.Remove(verifyCodeRequestDto.Email);
                        return Ok(new VerifyCodeResponseDto()
                        {
                            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(user)))
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error generating password reset token: {ex.Message}");
                        return StatusCode(500, "An error occurred while generating the password reset token.");
                    }
                }
                else
                {
                    return BadRequest("Invalid verification code.");
                }
            }
            else
            {
                return BadRequest("Verification code expired or not found.");
            }
        }
    }
}
