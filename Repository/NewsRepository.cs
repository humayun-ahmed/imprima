using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imprima.Repository.Contract;
using Imprima.Repository.Model;
using Infrastructure.LocalCache.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Imprima.Repository
{
	public class NewsRepository : INewsRepository
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly NewsDbContext _context;
		private const string ALL_ARTICLE_KEY="AllArticlesKey";

		public NewsRepository(NewsDbContext context, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
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

		public IQueryable<Article> Search(string title, bool cacheEnable = false)
		{
			IQueryable<Article> articles = null;
			if (cacheEnable)
			{
				var localCacheService = _serviceProvider.GetService<ILocalCacheService>();
				var list = localCacheService.Get<List<Article>>(ALL_ARTICLE_KEY);
				if (list == null)
				{
					localCacheService.Put(ALL_ARTICLE_KEY, _context.Articles.ToList());
					articles = _context.Articles;
				}
				else
				{
					articles = list.AsQueryable();
				}
			}
			else
			{
				articles = _context.Articles;
			}

			if (string.IsNullOrEmpty(title))
			{
				return articles;
			}
			else
			{
				return articles.Where(x => x.Title == title);
			}
		}
	}
}