namespace Mde.Project.Mobile.Core.ResultModels
{
    public class BaseResultModel
    {
        public bool IsSuccess => string.IsNullOrEmpty(Message);
        public string Message { get; set; }
    }
}
