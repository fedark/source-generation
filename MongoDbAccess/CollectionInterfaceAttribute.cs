using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbAccess
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class CollectionInterfaceAttribute : Attribute
	{
	}
}
