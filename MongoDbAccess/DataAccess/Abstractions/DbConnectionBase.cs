using MongoDB.Driver;
using MongoDbAccess.DataAccess.Abstractions;
using MongoDbAccess.Models;

namespace MongoDbAccess.DataAccess.Abstractions
{
	public abstract class DbConnectionBase : IDbConnection
	{
		protected IMongoDatabase Database { get; }

		public DbConnectionBase(string databaseName, string connectionString)
		{
			var client = new MongoClient(connectionString);
			Database = client.GetDatabase(databaseName);
		}

		public virtual IMongoCollection<TDocument> GetCollection<TDocument>(string name) where TDocument : IModel
		{
			return Database.GetCollection<TDocument>(name);
		}

		public abstract string GetCollectionName<TDocument>() where TDocument : IModel;
	}
}
