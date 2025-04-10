using MongoExt.Attributes;
using MongoExt.Interface;

namespace Demo;

[CachedCollection(1)]
public class Book : IModel
{
	public required string Id { get; set; }
	public required string Title { get; set; }
}