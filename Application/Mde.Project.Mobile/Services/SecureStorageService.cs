using Mde.Project.Mobile.Core.MauiHelpers;
using System;
namespace Mde.Project.Mobile.Services
{
    class SecureStorageService : ISecureStorageService
    {
        public async Task<string> GetAsync(string key)
        {
            return await SecureStorage.Default.GetAsync(key);
        }
        public bool Remove(string key)
        {
            return SecureStorage.Default.Remove(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await SecureStorage.Default.SetAsync(key, value);
        }
    }
}
