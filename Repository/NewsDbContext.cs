using System;
using System.Collections.Generic;
using System.Text;
using Imprima.Repository.EtConfiguration;
using Imprima.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Imprima.Repository
{
	public class NewsDbContext : DbContext
	{
		public virtual DbSet<Article> Articles { get; set; }
		public virtual DbSet<Source> Sources { get; set; }

		public NewsDbContext(DbContextOptions<NewsDbContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ArticleConfiguration());
			modelBuilder.ApplyConfiguration(new SourceConfiguration());
		}
	}
}