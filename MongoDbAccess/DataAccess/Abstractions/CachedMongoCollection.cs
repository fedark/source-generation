using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDbAccess.Models;

namespace MongoDbAccess.DataAccess.Abstractions
{
	public abstract class CachedMongoCollection<TDocument> : IDbCollection<TDocument> where TDocument : IModel
	{
		protected static readonly string CacheName = $"{typeof(TDocument).Name}Data";

		protected IMongoCollection<TDocument> Documents { get; }
		protected IMemoryCache Cache { get; }

		protected abstract TimeSpan CacheExpiration { get; }

		public CachedMongoCollection(IDbConnection db, IMemoryCache cache)
		{
			var name = db.GetCollectionName<TDocument>();
			Documents = db.GetCollection<TDocument>(name);
			Cache = cache;
		}

		public async Task CreateAsync(TDocument document)
		{
			await Documents.InsertOneAsync(document);
			Cache.Remove(CacheName);
		}

		public async Task<TDocument?> GetAsync(string id)
		{
			var result = await Documents.FindAsync(d => d.Id == id);
			return result.FirstOrDefault();
		}

		public async Task<IList<TDocument>> GetAllAsync()
		{
			if (Cache.Get<IList<TDocument>>(CacheName) is var cachedDocuments && cachedDocuments is null)
			{
				var allDocuments = await Documents.FindAsync(_ => true);
				cachedDocuments = await allDocuments.ToListAsync();

				Cache.Set(CacheName, cachedDocuments, CacheExpiration);
			}

			return cachedDocuments;
		}

		public async Task UpdateAsync(TDocument document)
		{
			await Documents.ReplaceOneAsync(k => k.Id == document.Id, document);
			Cache.Remove(CacheName);
		}

		public async Task RemoveAsync(string id)
		{
			await Documents.DeleteOneAsync(k => k.Id == id);
			Cache.Remove(CacheName);
		}
	}
}