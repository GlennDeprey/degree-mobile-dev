using Mde.Project.Api.Core.Entities;
using Mde.Project.Api.Core.Services.Models;
using System.Linq.Expressions;

namespace Mde.Project.Api.Core.Services.Interfaces.Base
{
    public interface ICrudService<T>
        where T : BaseEntity
    {
        Task<ResultModel<IEnumerable<T>>> GetAllAsync(string includeProperties = "");
        Task<PagingModel<IEnumerable<T>>> GetAllAsync(int pageNumber, int pageSize, string includeProperties = "");
        Task<ResultModel<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> expression, string includeProperties = "");
        Task<PagingModel<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, string includeProperties = "");
        Task<ResultModel<T>> GetByIdAsync(Guid id, string includeProperties = "");
        Task<ResultModel<T>> GetByExpressionAsync(Expression<Func<T, bool>> expression, string includeProperties = "");
        Task<BaseResult> UpdateAsync(T entity);
        Task<ResultModel<T>> AddAsync(T entity);
        Task<BaseResult> RemoveAsync(Guid id);
        Task<BaseResult> SoftRemoveAsync(Guid id);
        Task<ResultModel<bool>> ExistsAsync(Expression<Func<T, bool>> expression);
        Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null);
    }
}
