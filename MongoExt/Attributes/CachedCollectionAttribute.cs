using System;

namespace MongoExt.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CachedCollectionAttribute(int expirationMinutes) : Attribute
{
    public long ExpirationMinutes { get; } = expirationMinutes;
}