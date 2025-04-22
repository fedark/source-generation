using MongoDB.Driver;

namespace MongoExt.Interfaces;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string name) where T : IEntity;
}
