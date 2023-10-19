using Microsoft.EntityFrameworkCore;
using ChattingApplication.Core.Repositories;
using System.Linq.Expressions;

namespace ChattingApplication.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext Context;
        private DbSet<TEntity> _entity;

        public Repository(AppDbContext context)
        {
            Context = context;
            _entity = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _entity.Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entity.AddRange(entities);
        }

        public TEntity Get(int id)
        {
            return _entity.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entity.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.Where(predicate).ToList();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.SingleOrDefault(predicate);
        }

        public void Remove(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
        }
    }
}
