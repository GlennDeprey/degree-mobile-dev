using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities;
using Mde.Project.Api.Core.Services.Interfaces.Base;
using Mde.Project.Api.Core.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Mde.Project.Api.Core.Services.Base
{
    public abstract class CrudService<T> : ICrudService<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly string _entityName;
        
        public CrudService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _entityName = AddSpacesToCamelCase(typeof(T).Name);
        }
        protected string AddSpacesToCamelCase(string text)
        {
            return Regex.Replace(text, "(\\B[A-Z])", " $1");
        }
        public async Task<ResultModel<T>> AddAsync(T entity)
        {
            var result = new ResultModel<T>();
            try
            {
                entity.CreatedOn = DateTime.Now;
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                var query = _dbSet.AsQueryable();

                var navigationProperties = _context.Model.FindEntityType(typeof(T))
                    .GetNavigations()
                    .Select(n => n.Name);

                foreach (var navigationProperty in navigationProperties)
                {
                    query = query.Include(navigationProperty);
                }

                var reloadedEntity = await query.FirstOrDefaultAsync(e => e.Id == entity.Id);
                result.Data = reloadedEntity;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while adding {_entityName}.");
                return result;
            }
        }

        public async Task<ResultModel<T>> GetByExpressionAsync(Expression<Func<T, bool>> expression, string includeProperties = "")
        {
            var result = new ResultModel<T>();
            try
            {
                var query = _dbSet.AsQueryable();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties
                                 .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                var entity = await query.FirstOrDefaultAsync(expression);
                if (entity == null)
                {
                    result.Errors.Add($"{_entityName} not found.");
                    return result;
                }

                result.Data = entity;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while getting {_entityName}.");
                return result;
            }
        }

        public async Task<BaseResult> UpdateAsync(T entity)
        {
            var result = new BaseResult();
            try
            {
                entity.EditedOn = DateTime.Now;
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while updating {_entityName}.");
                return result;
            }
        }
        public async Task<BaseResult> RemoveAsync(Guid id)
        {
            var result = new BaseResult();
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
                if (entity == null)
                {
                    result.Errors.Add($"{_entityName} not found.");
                    throw new Exception();
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while removing {_entityName}.");
                return result;
            }
        }
        public async Task<BaseResult> SoftRemoveAsync(Guid id)
        {
            var result = new BaseResult();
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
                entity.DeletedOn = DateTime.Now;
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while removing {_entityName}.");
                return result;
            }
        }
        public async Task<ResultModel<T>> GetByIdAsync(Guid id, string includeProperties = "")
        {
            var result = new ResultModel<T>();

            try
            {
                var query = _dbSet.AsQueryable();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                var entity = await query.FirstOrDefaultAsync(e => e.Id == id);
                if (entity == null)
                {
                    result.Errors.Add($"{_entityName} not found.");
                    return result;
                }

                result.Data = entity;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while getting {_entityName}.");
                return result;
            }
        }

        public async Task<ResultModel<IEnumerable<T>>> GetAllAsync(string includeProperties = "")
        {
            var result = new ResultModel<IEnumerable<T>>();
            try
            {
                var query = _dbSet.AsQueryable();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                result.Data = await query.ToListAsync();
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while getting the items of {_entityName}.");
                return result;
            }
        }

        public async Task<ResultModel<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> expression, string includeProperties = "")
        {
            var result = new ResultModel<IEnumerable<T>>();
            try
            {
                var query = _dbSet.AsQueryable();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }
                if (expression != null)
                {
                    query = query.Where(expression);
                }

                result.Data = await query.ToListAsync();
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while getting the items of {_entityName}.");
                return result;
            }
        }
        public async Task<PagingModel<IEnumerable<T>>> GetAllAsync(int pageNumber, int pageSize, string includeProperties = "")
        {
            var result = new PagingModel<IEnumerable<T>>();
            try
            {
                var query = _dbSet.AsQueryable();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                var data = await query.OrderBy(o => o.DeletedOn != null)
                    .ThenBy(o => o.Id)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize).ToListAsync();
                var totalItems = await GetCountAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                result.TotalItems = totalItems;
                result.TotalPages = totalPages;
                result.Data = data;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while getting the items of {_entityName}.");
                return result;
            }
        }

        public async Task<PagingModel<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, string includeProperties = "")
        {
            var result = new PagingModel<IEnumerable<T>>();
            try
            {
                var query = _dbSet.AsQueryable();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                if (expression != null)
                {
                    query = query.Where(expression);
                }
                var data = await query.OrderBy(o => o.DeletedOn != null)
                    .ThenBy(o => o.Id)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize).ToListAsync();
                var totalItems = await GetCountAsync(expression);
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                result.TotalItems = totalItems;
                result.TotalPages = totalPages;
                result.Data = data;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while getting the items of {_entityName}.");
                return result;
            }
        }

        public async Task<ResultModel<bool>> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            var result = new ResultModel<bool>();
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(expression);

                result.Data = entity != null;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while searching for {_entityName}.");
                return result;
            }
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null)
        {
            var query = _dbSet.AsQueryable();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            return await query.CountAsync();
        }
    }
}
