using MongoDB.Driver;
using MongoExt.Interfaces;

namespace MongoExt.Impl;

public class MongoDbContext(string connectionString, string databaseName) : IMongoDbContext
{
    private IMongoDatabase Database { get; } = new MongoClient(connectionString).GetDatabase(databaseName);

    public IMongoCollection<T> GetCollection<T>(string name) where T : IEntity => Database.GetCollection<T>(name);
}
