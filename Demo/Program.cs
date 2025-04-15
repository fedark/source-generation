//Console.WriteLine(typeof(TestB));
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

Console.WriteLine(DbCollectionInterfaceCode.GetCode("XXX", "ZZZ"));
return;


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
