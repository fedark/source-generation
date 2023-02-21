using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDbAccess.DataAccess.Abstractions;
using MongoDbAccess.Models;

namespace MongoDbAccess.DataAccess
{
	public class CachedMongoCollection<TDocument> : IDbCollection<TDocument> where TDocument : IModel
	{
		protected IMongoCollection<TDocument> Documents { get; }
		protected MongoCollectionCache<TDocument> Cache { get; }

		public CachedMongoCollection(IDbConnection db, MongoCollectionCache<TDocument> cache)
		{
			var name = db.GetCollectionName<TDocument>();
			Documents = db.GetCollection<TDocument>(name);
			Cache = cache;
		}

		public async Task CreateAsync(TDocument document)
		{
			await Documents.InsertOneAsync(document);
			Cache.Remove();
		}

		public async Task<TDocument?> GetAsync(string id)
		{
			var result = await Documents.FindAsync(d => d.Id == id);
			return result.FirstOrDefault();
		}

		public async Task<IList<TDocument>> GetAllAsync()
		{
			if (Cache.Get() is var cachedDocuments && cachedDocuments is null)
			{
				var allDocuments = await Documents.FindAsync(_ => true);
				cachedDocuments = await allDocuments.ToListAsync();

				Cache.Set(cachedDocuments);
			}

			return cachedDocuments;
		}

		public async Task UpdateAsync(TDocument document)
		{
			await Documents.ReplaceOneAsync(k => k.Id == document.Id, document);
			Cache.Remove();
		}

		public async Task RemoveAsync(string id)
		{
			await Documents.DeleteOneAsync(k => k.Id == id);
			Cache.Remove();
		}
	}
}