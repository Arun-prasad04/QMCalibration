using CMT.DATAMODELS;
using System;
//using System.Collections.Generic;
using System.Linq;
namespace CMT.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public CMTDatabaseContext Context { get; set; }
        public Dictionary<Type, object> Repositories { get; set; }
        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction Transaction { get; set; }
        public UnitOfWork(CMTDatabaseContext context)
        {
            Repositories = new Dictionary<Type, object>();
            Context = context;
            Transaction = null;
        }

        public virtual IRepository<T> Repository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)) == true)
            {
                return Repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repo = new Repository<T>(Context);
            Repositories.Add(typeof(T), repo);
             return repo;
        }

        public virtual void Commit()
        {
            Transaction.Commit();
        }

        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }
        
        public virtual void RollBack()
        {
            Context.Dispose();
          //  Transaction.Rollback();
        }

        public virtual void BeginTransaction()
        {
            Transaction=Context.Database.BeginTransaction();
        }
    }
}
