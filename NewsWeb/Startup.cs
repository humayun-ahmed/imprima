using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Imprima.Repository;
using Imprima.Repository.Contract;
using Imprima.Repository.Model;
using Infrastructure.LocalCache;
using Infrastructure.LocalCache.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace NewsWeb
{
	public class Startup
	{
		private readonly IConfiguration configuration;

		public Startup(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// Add services
			services.AddScoped<INewsRepository, NewsRepository>();
			services.AddSingleton<ILocalCacheService, LocalCacheService>();

			services.AddDbContext<NewsDbContext>(options =>
			{
				var connectionString = configuration.GetConnectionString("NewsDbContext");
				options.UseSqlServer(connectionString);
			});
			
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseExceptionHandler("/error.html");


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc(routes =>
			{
				routes.MapRoute("Default",
					"{controller=Home}/{action=Index}/{id?}"
				);
			});

			app.UseFileServer();
		}
	}
}
