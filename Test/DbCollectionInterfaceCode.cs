namespace MongoExt.Generator2;

internal static class DbCollectionInterfaceCode
{
	public static string GetCode(string targetNamespace, string modelName)
	{
		return $$"""
		       using MongoExt.Interface;
		       
		       namespace {{targetNamespace}};
		       
		       public interface I{{modelName}}DbCollection : IDbCollection<{{modelName}}>
		       {
		       }
		       """;
	}
}