using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDbAccess.DataAccess.Abstractions
{
	public interface IDbCollection<TDocument>
	{
		Task CreateAsync(TDocument document);
		Task<IList<TDocument>> GetAllAsync();
		Task<TDocument?> GetAsync(string id);
		Task RemoveAsync(string id);
		Task UpdateAsync(TDocument document);
	}
}
