namespace MongoExt.Generator;

internal static class Code
{
    public static string EntityAttribute()
    {
        return $$"""
namespace {{Names.Namespace}};

[AttributeUsage(AttributeTargets.Class)]
public class {{Names.EntityAttributeFull}}(string collectionName, int cacheExpirationMinutes) : Attribute
{
}
""";
    }

    public static string EntityRepositoryInterface(string @namespace, string entityName)
    {
        return $$"""
using MongoExt.Interfaces;

namespace {{@namespace}};

public partial interface I{{entityName}}Repository : IRepository<{{entityName}}>
{
}
""";
    }

    public static string EntityRepository(string @namespace, string entityName, string collectionName, int cacheExpirationMinutes)
    {
        return $$"""
using Microsoft.Extensions.Caching.Memory;
using MongoExt.Interfaces;
using MongoExt.Impl;

namespace {{@namespace}};

public partial class {{entityName}}Repository(IMongoDbContext dbContext, IMemoryCache cache) 
    : CachedRepository<{{entityName}}>(dbContext, "{{collectionName}}", cache, TimeSpan.FromMinutes({{cacheExpirationMinutes}})), I{{entityName}}Repository
{
}
""";
    }

    public static string Trace(params string[] values)
    {
        var trace = string.Join("|", values);

        return $$"""
namespace {{Names.Namespace}};

public static class GenerationTrace
{
	public static string GetTrace()
	{
        return "{{trace}}";
	}
}
""";
    }
}