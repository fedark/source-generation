using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MongoExt.Generator.SyntaxReceivers;

/// <summary>
/// <para>
/// Captures model syntax nodes.
/// Such node should be a class that marked with <c>CachedCollection</c> attribute and inherits from <c>IModel</c> interface
/// </para>
/// The node is captured as the following tuple: (namespace, model class name, cache expiration minutes)
/// </summary>
internal class ModelSyntaxReceiver : ISyntaxReceiver
{
    private const string AttributeMarker = "CachedCollection";
    private const string BaseTypeMarker = "IModel";

    public List<(NamespaceDeclarationSyntax Namespace, string ClassName, int ExpirationMinutes)> Captures { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is AttributeSyntax attributeSyntax
            && attributeSyntax.Name.ToString() == AttributeMarker
            && attributeSyntax.ArgumentList is not null
            && attributeSyntax.ArgumentList.Arguments is var arguments
            && arguments.Count == 1
            && arguments[0].Expression is LiteralExpressionSyntax argument1
            && int.TryParse(argument1.Token.ValueText, out var expiration)
            && attributeSyntax.Parent?.Parent is ClassDeclarationSyntax classDeclarationSyntax
            && classDeclarationSyntax.BaseList is not null
            && classDeclarationSyntax.BaseList.Types
                .OfType<SimpleBaseTypeSyntax>()
                .Any(s => s.Type is IdentifierNameSyntax { Identifier.ValueText: BaseTypeMarker })
            && classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
        {
            Captures.Add((namespaceDeclarationSyntax, classDeclarationSyntax.Identifier.ValueText, expiration));
        }
    }
}