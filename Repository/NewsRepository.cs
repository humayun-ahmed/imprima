using System.Collections.Generic;
using System.Threading.Tasks;
using Imprima.Repository.Contract;
using Imprima.Repository.Model;

namespace Imprima.Repository
{
	public class NewsRepository : INewsRepository
	{
		private readonly NewsDbContext _context;

		public NewsRepository(NewsDbContext context)
		{
			_context = context;
		}

		public async Task<bool> BulkInsertAsync(List<Article> articles)
		{
			foreach (var article in articles) _context.Articles.Add(article);

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> ClearData()
		{
			_context.RemoveRange(_context.Articles);
			_context.RemoveRange(_context.Sources);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}