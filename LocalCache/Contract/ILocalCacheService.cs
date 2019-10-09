namespace Infrastructure.LocalCache.Contract
{
	public interface ILocalCacheService
	{
		bool Put<T>(string key, T data);
		T Get<T>(string key);
	}
}
