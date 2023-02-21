using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MongoDbAccess.Generators
{
	internal class CollectionImplementationAttributeSyntaxReceiver : ISyntaxReceiver
    {
        private static readonly string AttributeMarker = "CollectionImplementation";

        public HashSet<string> Captures { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is AttributeSyntax attributeSyntax
                && attributeSyntax.Name.ToString() == AttributeMarker
                && attributeSyntax.Parent?.Parent is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.BaseList is not null
                && classDeclarationSyntax.BaseList.Types.OfType<SimpleBaseTypeSyntax>()
                    .FirstOrDefault(s => s.Type is IdentifierNameSyntax) is var baseNameSyntax
                && baseNameSyntax is not null
                && baseNameSyntax.Type is IdentifierNameSyntax identifierNameSyntax)
            {
                Captures.Add(identifierNameSyntax.Identifier.ValueText);
            }
        }
    }
}
