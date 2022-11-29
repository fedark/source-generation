using MongoDB.Driver;
using MongoDbAccess.Models;

namespace MongoDbAccess.DataAccess.Abstractions
{
	public interface IDbConnection
	{
		string GetCollectionName<TDocument>() where TDocument : IModel;

		IMongoCollection<TDocument> GetCollection<TDocument>(string name) where TDocument : IModel;
	}
}
