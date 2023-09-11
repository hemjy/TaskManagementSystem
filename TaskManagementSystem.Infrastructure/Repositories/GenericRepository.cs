using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Infrastructure.Data.Context;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;


        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<T?> GetAsync(
                Expression<Func<T, bool>> expression,
                bool trackChanges = false,
                List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes is not null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            return !trackChanges ? await query.AsNoTracking().FirstOrDefaultAsync(expression)
                                : await query.FirstOrDefaultAsync(expression);
        }


        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = default, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = default, 
            bool trackChanges = false,
            List<Expression<Func<T, object>>> includes = default)
        {
            IQueryable<T> query = _dbSet;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (includes is not null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return !trackChanges ? query.AsNoTracking() : query;
        }

        public void Update(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> expression)
        {
            var exist = _context.Set<T>().Where(expression);
            return await exist.AnyAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}
