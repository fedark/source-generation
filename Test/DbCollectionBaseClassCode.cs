namespace MongoExt.Generator2;

internal static class DbCollectionBaseClassCode
{
	public static string GetCode(string targetNamespace, string modelName, int cacheExpirationMinutes)
	{
		return $$"""
		       using System;
		       using System.Collections.Generic;
		       using System.Threading.Tasks;
		       using Microsoft.Extensions.Caching.Memory;
		       using MongoDB.Driver;
		       using MongoDbAccess.DataAccess.Abstractions;
		       using MongoDbAccess.DataAccess;

		       namespace {{targetNamespace}};
		       
		       public abstract class {{modelName}}DbCollectionBase : I{{modelName}}DbCollection
		       {
		           protected IMongoCollection<{{modelName}}> Documents { get; }
		           protected MongoCollectionCache<{{modelName}}> Cache { get; }
		           private CachedMongoCollection<{{modelName}}> CachedCollection { get; }
		   
		           protected {{modelName}}DbCollectionBase(IDbConnection db, IMemoryCache cache)
		           {
		               var name = db.GetCollectionName<{{modelName}}>();
		               Documents = db.GetCollection<{{modelName}}>(name);
		               Cache = new(cache, TimeSpan.FromMinutes({{cacheExpirationMinutes}}));
		               CachedCollection = new(db, Cache);
		           }
		   
		           public Task CreateAsync({{modelName}} document)
		           {
		               return CachedCollection.CreateAsync(document);
		           }
		   
		           public Task<{{modelName}}?> GetAsync(string id)
		           {
		               return CachedCollection.GetAsync(id);
		           }
		   
		           public Task<IList<{{modelName}}>> GetAllAsync()
		           {
		               return CachedCollection.GetAllAsync();
		           }
		   
		           public Task UpdateAsync({{modelName}} document)
		           {
		               return CachedCollection.UpdateAsync(document);
		           }
		   
		           public Task RemoveAsync(string id)
		           {
		               return CachedCollection.RemoveAsync(id);
		           }
		       }
		       """;
	}
}