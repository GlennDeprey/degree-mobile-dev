namespace Mde.Project.Mobile.Core.ResultModels
{
    public class ResultModel<T> : BaseResultModel
    {
        public T Data { get; set; }
    }
}
