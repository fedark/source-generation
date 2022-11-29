using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeneratorAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators
{
    [Generator]
	public class CollectionGenerator : ISourceGenerator
	{
        public void Execute(GeneratorExecutionContext context)
        {
			var syntaxReceiver = (CollectionAttributeSyntaxReceiver)context.SyntaxReceiver;
			if (syntaxReceiver is null)
			{
				return;
			}

			foreach (var capture in syntaxReceiver.Captures)
			{

			}
        }

        public void Initialize(GeneratorInitializationContext context)
        {
			context.RegisterForSyntaxNotifications(() => new CollectionAttributeSyntaxReceiver());
        }
    }

	public class CollectionAttributeSyntaxReceiver : ISyntaxReceiver
	{
		private const string BaseTypeMarker = "IModel";
		private static readonly string AttributeMarker = nameof(CachedCollectionAttribute).Replace("Attribute", "");

		public List<(string NamespaceName, string ClassName, TimeScale TimeScale, int Expiration)> Captures { get; } = new();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			Debugger.Launch();
			if (syntaxNode is AttributeSyntax attributeSyntax 
				&& attributeSyntax.Name.ToString() == AttributeMarker
				&& attributeSyntax.Parent?.Parent is ClassDeclarationSyntax { BaseList: not null } classDeclarationSyntax
				&& classDeclarationSyntax.BaseList.Types.OfType<SimpleBaseTypeSyntax>()
					.Any(s => s.Type is IdentifierNameSyntax identifierNameSyntax
						&& identifierNameSyntax.Identifier.ValueText == BaseTypeMarker)
				&& classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax
				&& namespaceDeclarationSyntax.Name is not null)
			{
				System.Console.WriteLine(namespaceDeclarationSyntax.Name);
				//Captures.Add((classDeclarationSyntax, attributeSyntax));
			}
		}
	}
}
