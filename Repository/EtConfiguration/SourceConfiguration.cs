using System;
using System.Collections.Generic;
using System.Text;
using Imprima.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprima.Repository.EtConfiguration
{
	public class SourceConfiguration : IEntityTypeConfiguration<Source>
	{
		public void Configure(EntityTypeBuilder<Source> builder)
		{
			builder.HasKey(s => s.SourceId);
		}
	}
}
