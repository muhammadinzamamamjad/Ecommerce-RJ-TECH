namespace RJTECH_Authentication_.Models
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity entity);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity entity);
        void Delete(int id);
    }
}
