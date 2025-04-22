# About

This repo contains source generators utilizing incremental generators approach. 
Below is the list of generators.

## `MongoExt.Generator`

Helps to implement repository pattern for mongo collections. Additionally, the db interactions are cached into memory.

### Usage

In your project reference `MongoExt` and `MongoExt.Generator`.

```c#
<ItemGroup>
  <ProjectReference Include="..\MongoExt.Generator\MongoExt.Generator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
  <ProjectReference Include="..\MongoExt\MongoExt.csproj" />
</ItemGroup>
```

Create an entity market with the `Entity` attribute. The first argument is a collection name in the database. The second one is cache expiration in minutes.

```c#
[Entity("books", 10)]
public class Book : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Title { get; set; }
}
```

The generator will produce a partial interface for repository and its implementation based on cached collection.

```c#
public partial interface IBookRepository : IRepository<Book>
{
}
```

```c#
public partial class BookRepository(IMongoDbContext dbContext, IMemoryCache cache) 
    : CachedRepository<Book>(dbContext, "books", cache, TimeSpan.FromMinutes(10)), IBookRepository
{
}
```

So that you can extend the repository with your own methods.
