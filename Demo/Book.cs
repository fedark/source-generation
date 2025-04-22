using MongoExt.Interfaces;
using MongoExt.Generator;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Demo;

[Entity("books", 10)]
public class Book : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Title { get; set; }
}

public partial interface IBookRepository
{
    void Add(Book book);
}

public partial class BookRepository
{
    public void Add(Book book) => CreateAsync(book).Wait();
}
