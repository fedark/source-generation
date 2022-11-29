using System;
using GeneratorAttributes;
using MongoDbAccess.Models;

namespace Mock
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(new Model().ToString());
		}
	}

	[CachedCollection(1)]         
	public class Model : IModel
	{
		public string Id { get; set; }
	}
}
