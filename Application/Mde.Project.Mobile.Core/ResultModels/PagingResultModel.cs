namespace Mde.Project.Mobile.Core.ResultModels
{
    public class PagingResultModel<T> : BaseResultModel
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
