using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDbAccess.Models;

namespace MongoDbAccess.DataAccess
{
	public class MongoCollectionCache<TDocument> where TDocument : IModel
	{
		private static readonly string CacheName = $"{typeof(TDocument).Name}Data";

		private IMemoryCache MemoryCache { get; }
		private TimeSpan CacheExpiration { get; }

		public MongoCollectionCache(IMemoryCache memoryCache, TimeSpan cacheExpiration)
		{
			MemoryCache = memoryCache;
			CacheExpiration = cacheExpiration;
		}

		public IList<TDocument>? Get()
		{
			return MemoryCache.Get<IList<TDocument>>(CacheName);
		}

		public void Set(IList<TDocument> documents)
		{
			MemoryCache.Set(CacheName, documents, CacheExpiration);
		}

		public void Remove()
		{
			MemoryCache.Remove(CacheName);
		}
	}
}
