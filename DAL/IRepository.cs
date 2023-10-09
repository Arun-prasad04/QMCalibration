using System;
//using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMT.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void DeleteRange(TEntity[] entityListToDelete);
        void Delete(object id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IEnumerable<TEntity> GetAsNoTracking(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        TEntity GetByID(object id);
        IEnumerable<TEntity> GetAll();
        void Insert(TEntity entity);
        void InsertRange(TEntity[] entityList);
        void Update(TEntity entityToUpdate);
        void UpdateRange(TEntity[] entityList);
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IQueryable<TEntity> GetQueryAsNoTracking(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
    }
}
