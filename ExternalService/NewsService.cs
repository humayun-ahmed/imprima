using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Imprima.ExternalService.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Imprima.ExternalService
{
	public class NewsService : INewsService
	{
		private readonly ILogger _logger;
		private readonly HttpClient httpClient;

		public NewsService(IConfigurationRoot configuration, ILogger<NewsService> logger)
		{
			_logger = logger;
			var uri = new Uri(configuration["NewsUrlHost"]);
			httpClient = new HttpClient {BaseAddress = uri};
			httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
			var sp = ServicePointManager.FindServicePoint(uri);
			sp.ConnectionLeaseTimeout = 120 * 1000; // 2 minute
		}

		public async Task<NewsRoot> GetAsync()
		{
			NewsRoot newsRoot = null;
			try
			{
				var response =
					await httpClient.GetAsync("v2/top-headlines?country=us&apiKey=9034101789784cea94358866ce8a262e");
				var responseBody = await response.Content.ReadAsStringAsync();
				newsRoot = JsonConvert.DeserializeObject<NewsRoot>(responseBody);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
			}

			return newsRoot;
		}
	}

	public interface INewsService
	{
		Task<NewsRoot> GetAsync();
	}
}