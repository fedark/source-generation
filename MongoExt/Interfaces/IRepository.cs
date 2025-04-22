namespace MongoExt.Interfaces;

public interface IRepository<T> where T : IEntity
{
    Task CreateAsync(T entity);
    Task<T?> GetAsync(string id);
    Task UpdateAsync(T entity);
    Task RemoveAsync(string id);
}
