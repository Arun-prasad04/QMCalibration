namespace CMT.DAL
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        void Commit();
        void RollBack();
        void BeginTransaction();
        void SaveChanges();
    }
}
