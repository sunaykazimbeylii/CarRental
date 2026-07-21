using CarRental.Application.Interfaces.Repository.Generic;
using CarRental.Domain.Entities.Common;
using CarRental.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRentalSystem.API.Implementations.Repository.Generic
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll(
     Expression<Func<T, bool>>? func = null,
     Expression<Func<T, object>>? sort = null,
     bool isDesc = false,
     bool takeDeleted = false,
     int page = 0,
     int take = 0,
     params string[]? includes)
        {
            IQueryable<T> query = _dbSet;

            if (!takeDeleted)
                query = query.Where(x => !x.IsDeleted);

            if (func is not null)
                query = query.Where(func);

            if (includes is not null)
                query = _getIncludes(query, includes);

            if (sort is not null)
            {
                query = isDesc
                    ? query.OrderByDescending(sort)
                    : query.OrderBy(sort);
            }

            if (page > 0 && take > 0)
                query = query.Skip((page - 1) * take).Take(take);

            return query;
        }

        public async Task<T> GetById(long id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes is not null)
            {
                query = _getIncludes(query, includes);
            }
            return await query.FirstOrDefaultAsync(c => c.Id == id);

        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);

        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);

        }
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected IQueryable<T> _getIncludes(IQueryable<T> query, params string[]? includes)
        {

            for (int i = 0; i < includes.Length; i++)
            {
                query = query.Include(includes[i]);
            }
            return query;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> func)
        {
            return await _dbSet.AnyAsync(func);
        }
    }
}
