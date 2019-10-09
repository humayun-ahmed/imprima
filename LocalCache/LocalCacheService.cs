using System.Collections.Generic;
using Infrastructure.LocalCache.Contract;

namespace Infrastructure.LocalCache
{
	public class LocalCacheService : ILocalCacheService
	{
		private Dictionary<string, object> localStore;
		public LocalCacheService()
		{
			localStore = new Dictionary<string, object>();
		}
		public bool Put<T>(string key, T data)
		{
			localStore.Add(key,data);
			return true;
		}

		public T Get<T>(string key)
		{
			if (localStore.ContainsKey(key))
			{
				return (T)localStore.GetValueOrDefault(key);
			}

			return default(T);
		}
	}
}