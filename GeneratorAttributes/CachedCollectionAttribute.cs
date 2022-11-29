using System;

namespace GeneratorAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CachedCollectionAttribute : Attribute
	{
		public TimeScale Scale { get; }
		public long CacheExpiration { get; }

		public CachedCollectionAttribute(TimeScale scale, int cacheExpiration)
		{
			Scale = scale;
			CacheExpiration = cacheExpiration;
		}
	}
}
