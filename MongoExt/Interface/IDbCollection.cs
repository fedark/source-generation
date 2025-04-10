using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoExt.Interface;

public interface IDbCollection<T>
{
	Task CreateAsync(T document);
	Task<IList<T>> GetAllAsync();
	Task<T?> GetAsync(string id);
	Task RemoveAsync(string id);
	Task UpdateAsync(T document);
}