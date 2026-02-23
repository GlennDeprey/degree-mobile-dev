using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Authentication.Models;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Authentication.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<BaseResultModel> TryUploadProfilePictureAsync(Stream imageStream, string fileName);
        Task<ResultModel<UserProfileModel>> TryGetProfileAsync();
        Task<bool> TrySignInAsync(string email, string password);
        Task<bool> TrySignUpAsync(string email, string firstname, string lastname, string password);
        Task<bool> TryForgotPasswordAsync(string email);
        Task<string> TryVerifyCodeAsync(string email, string code);
        Task<bool> TryResetPasswordAsync(string email, string password, string token);
        Task<bool> IsInRole(string role, bool onlyRole = false);
        Task<UserInfoModel> GetUserInfoAsync();
        bool SignOut();
    }
}
