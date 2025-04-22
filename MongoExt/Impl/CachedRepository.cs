using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoExt.Interfaces;

namespace MongoExt.Impl;

public abstract class CachedRepository<T>(IMongoDbContext dbContext,
    string collectionName,
    IMemoryCache cache,
    TimeSpan cacheExpiration)
    : IRepository<T> where T : IEntity
{
    protected static readonly string CacheName = $"{typeof(T).Name}Data";
    protected IMemoryCache Cache { get; } = cache;
    protected TimeSpan CacheExpiration { get; } = cacheExpiration;
    protected IMongoCollection<T> Entities { get; } = dbContext.GetCollection<T>(collectionName);

    public async Task CreateAsync(T entity)
    {
        await Entities.InsertOneAsync(entity).ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(string id)
    {
        if (Cache.TryGetValue(GetKey(id), out var entity))
            return (T?)entity;

        var entities = await Entities.FindAsync(d => d.Id == id).ConfigureAwait(false);
        entity = entities.FirstOrDefault();

        if (entity is IEntity e && e.Id is not null)
        {
            Cache.Set(GetKey(e.Id), entity, CacheExpiration);
        }

        return (T?)entity;
    }

    public async Task UpdateAsync(T entity)
    {
        await Entities.ReplaceOneAsync(k => k.Id == entity.Id, entity).ConfigureAwait(false);

        if (entity.Id is not null)
        {
            Cache.Remove(GetKey(entity.Id)); 
        }
    }

    public async Task RemoveAsync(string id)
    {
        await Entities.DeleteOneAsync(k => k.Id == id);
        Cache.Remove(GetKey(id));
    }

    private string GetKey(string id) => $"{CacheName}.{id}";
}
