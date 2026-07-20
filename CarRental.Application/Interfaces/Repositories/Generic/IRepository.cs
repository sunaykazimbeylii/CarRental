using CarRental.Domain.Entities.Common;
using System.Linq.Expressions;

namespace CarRental.Application.Interfaces.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(
           Expression<Func<T, bool>>? func = null,
           Expression<Func<T, object>>? sort = null,
           bool isDesc = false,
           bool takeDeleted = false,
           int page = 0,
           int take = 0,
           params string[]? includes
           );
        Task<T> GetById(long id, params string[]? includes);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> func);
        Task SaveChangeAsync();
    }
}
