namespace Mde.Project.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetCodeAsync(string fullName, string email, string code);
    }
}
