using System.Threading.Tasks;
using MongoExt.Attributes;
using MongoExt.Interface;

namespace Demo;

[CollectionDefinition]
public interface IBookCollection : IDbCollection<Book>
{
	Task<string> GetTitleAsync(Book book);
}