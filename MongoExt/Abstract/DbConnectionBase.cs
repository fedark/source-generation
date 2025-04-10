using MongoDB.Driver;
using MongoExt.Interface;

namespace MongoExt.Abstract;

public abstract class DbConnectionBase : IDbConnection
{
	protected IMongoDatabase Database { get; }

	public DbConnectionBase(string connectionString, string databaseName)
	{
		var client = new MongoClient(connectionString);
		Database = client.GetDatabase(databaseName);
	}

	public virtual IMongoCollection<T> GetCollection<T>(string name) where T : IModel
	{
		return Database.GetCollection<T>(name);
	}

	public abstract string GetCollectionName<T>() where T : IModel;
}