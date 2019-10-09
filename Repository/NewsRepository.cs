using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imprima.Repository.Contract;
using Imprima.Repository.Model;
using Microsoft.EntityFrameworkCore;

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

		public async Task<bool> ClearDataAsync()
		{
			_context.RemoveRange(_context.Articles);
			_context.RemoveRange(_context.Sources);
			await _context.SaveChangesAsync();
			return true;
		}

		public IQueryable<Article> Search(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				return _context.Articles;
			}
			else
			{
				return _context.Articles.Where(x => x.Title == title);
			}
		}
	}
}