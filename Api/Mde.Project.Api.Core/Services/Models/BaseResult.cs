namespace Mde.Project.Api.Core.Services.Models
{
    public class BaseResult
    {
        public bool Success => !Errors.Any();
        public List<string> Errors { get; set; } = [];
    }
}
