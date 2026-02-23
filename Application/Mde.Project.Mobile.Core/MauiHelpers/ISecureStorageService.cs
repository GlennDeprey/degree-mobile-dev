
namespace Mde.Project.Mobile.Core.MauiHelpers
{
    public interface ISecureStorageService
    {
        Task<string> GetAsync(string key);
        bool Remove(string key);
        Task SetAsync(string key, string value);
    }
}
