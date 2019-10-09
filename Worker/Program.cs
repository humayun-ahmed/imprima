using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Imprima.ExternalService;
using Imprima.ExternalService.Dto;
using Imprima.Repository;
using Imprima.Repository.Contract;
using Imprima.Repository.Model;
using Infrastructure.LocalCache;
using Infrastructure.LocalCache.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Imprima.Worker
{
	internal class Program
	{
		public static IConfigurationRoot configuration;

		private static async Task Main(string[] args)
		{
			try
			{
				var serviceCollection = new ServiceCollection();
				ConfigureServices(serviceCollection);

				// Create service provider
				IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

				var newsService = serviceProvider.GetService<INewsService>();


				var news = await newsService.GetAsync();

				var iMapper = serviceProvider.GetService<IMapper>();
				List<Article> articles = new List<Article>();
				foreach (var articleDto in news.Articles)
				{
					var article = iMapper.Map<ArticleDto, Article>(articleDto);
					articles.Add(article);
				}

				var newsRepository = serviceProvider.GetService<INewsRepository>();
				await newsRepository.ClearDataAsync();
				await newsRepository.BulkInsertAsync(articles);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.WriteLine("The End!");
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			// Add logging
			serviceCollection.AddSingleton(new LoggerFactory()
				.AddSerilog()
			);
			serviceCollection.AddLogging();

			// Build configuration
			configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
				.AddJsonFile("appsettings.json", false)
				.Build();


			// Initialize serilog logger
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(LogEventLevel.Debug)
				.MinimumLevel.Debug()
				.Enrich.FromLogContext()
				.CreateLogger();

			// Add access to generic IConfigurationRoot
			serviceCollection.AddSingleton(configuration);

			// Add services
			serviceCollection.AddSingleton<INewsService, NewsService>();
			serviceCollection.AddScoped<INewsRepository, NewsRepository>();
			serviceCollection.AddSingleton(ConfigMapper());
			serviceCollection.AddSingleton<ILocalCacheService, LocalCacheService>();


			serviceCollection.AddDbContext<NewsDbContext>(options =>
			{
				var connectionString = configuration.GetConnectionString("NewsDbContext");
				options.UseSqlServer(connectionString);
			});
		}

		private static IMapper ConfigMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ArticleDto, Article>();
				cfg.CreateMap<SourceDto, Source>();
			});
			IMapper iMapper = config.CreateMapper();
			return iMapper;
		}
	}
}
