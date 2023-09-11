using System.Linq.Expressions;

namespace TaskManagementSystem.Core.Interfaces.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Begins tracking the entity, and any other reachable
        /// entity that are not already being tracked in the entity added state
        /// such that they will be inserted into the database when <seealso cref="IUnitOfWork.SaveAsync"/>
        /// is called.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Add task that represents the asynchronous Add operation.</returns>
        Task AddAsync(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(List<T> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="trackChanges"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<T?> GetAsync(
                Expression<Func<T, bool>> expression,
                bool trackChanges = false,
                List<Expression<Func<T, object>>> includes = null!);

        IQueryable<T> GetAll(
            Expression<Func<T, bool>> expression = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            bool trackChanges = false,
            List<Expression<Func<T, object>>> includes = null!);

        void Update(T entity);

        Task<bool> Exist(Expression<Func<T, bool>> expression);
        void Delete(T entity);
    }
}
