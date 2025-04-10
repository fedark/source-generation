using System.Threading.Tasks;

namespace Demo;

public partial class BookMongoCollection
{
	public override Task<string> GetTitle(Book book)
	{
		return Task.FromResult(book.Title);
	}

	public static void Test()
	{
		
	}
}