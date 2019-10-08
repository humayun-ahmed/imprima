using System;
using System.Collections.Generic;
using System.Text;
using Imprima.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprima.Repository.EtConfiguration
{
	public class ArticleConfiguration : IEntityTypeConfiguration<Article>
	{
		public void Configure(EntityTypeBuilder<Article> builder)
		{
			builder.HasKey(x => x.ArticleId);
			builder.Property(x => x.Author).HasMaxLength(200);
			builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(500);
			builder.Property(x => x.Url).HasMaxLength(2000);
			builder.Property(x => x.UrlToImage).HasMaxLength(2000);
			builder.HasOne(x=>x.Source).WithMany().HasForeignKey(x => x.SourceKeyId);
		}
	}
}
