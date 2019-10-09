using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imprima.Repository.Model;

namespace Imprima.Repository.Contract
{
	public interface INewsRepository
	{
		Task<bool> BulkInsertAsync(List<Article> articles);
		Task<bool> ClearDataAsync();
		IQueryable<Article> Search(string title, bool cacheEnable = false);

	}
}