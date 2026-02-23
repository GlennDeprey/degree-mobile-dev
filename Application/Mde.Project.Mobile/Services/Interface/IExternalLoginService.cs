using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Services.Types;

namespace Mde.Project.Mobile.Services.Interface
{
    public interface IExternalLoginService
    {
        Task<BaseResultModel> LoginAsync(ExternalLoginType loginType);
        Task<BaseResultModel> LinkAsync(ExternalLoginType linkType);
        Task<BaseResultModel> UnlinkAsync(ExternalLoginType linkType);
    }
}
