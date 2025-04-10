using MongoDB.Driver;

namespace MongoExt.Interface;

public interface IDbConnection
{
	string GetCollectionName<T>() where T : IModel;

	IMongoCollection<T> GetCollection<T>(string name) where T : IModel;
}