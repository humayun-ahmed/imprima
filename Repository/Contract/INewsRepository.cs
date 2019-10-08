using System.Collections.Generic;
using System.Threading.Tasks;
using Imprima.Repository.Model;

namespace Imprima.Repository.Contract
{
	public interface INewsRepository
	{
		Task<bool> BulkInsertAsync(List<Article> articles);
		Task<bool> ClearData();
	}
}