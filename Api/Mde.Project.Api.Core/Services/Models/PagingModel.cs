namespace Mde.Project.Api.Core.Services.Models
{
    public class PagingModel<T> : BaseResult
    {
        public T Data { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
