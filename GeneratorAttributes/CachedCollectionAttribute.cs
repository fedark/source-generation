using System;

namespace GeneratorAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CachedCollectionAttribute : Attribute
	{
		public long ExpirationMinutes { get; }

		public CachedCollectionAttribute(int expirationMinutes)
		{
			ExpirationMinutes = expirationMinutes;
		}
	}
}
