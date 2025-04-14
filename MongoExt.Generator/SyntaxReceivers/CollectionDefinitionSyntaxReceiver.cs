using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MongoExt.Generator.SyntaxReceivers;

/// <summary>
/// <para>
/// Captures collection definition syntax nodes.
/// Such node should be an interface that marked with <c>CollectionDefinition</c> attribute and inherits from <c>IDbCollection<![CDATA[<TModel>]]></c> interface
/// </para>
/// The node is captured as the following tuple: (<c>TModel</c> type name, (interface name, interface methods))
/// </summary>
internal class CollectionDefinitionSyntaxReceiver : ISyntaxReceiver
{
    private const string AttributeMarker = "CollectionDefinition";
    private const string BaseTypeMarker = "IDbCollection";

    public Dictionary<string, (string BasyTypeName, MethodDeclarationSyntax[] Methods)> Captures { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is AttributeSyntax attributeSyntax
            && attributeSyntax.Name.ToString() == AttributeMarker
            && attributeSyntax.Parent?.Parent is InterfaceDeclarationSyntax interfaceDeclarationSyntax
            && interfaceDeclarationSyntax.BaseList is not null
            && interfaceDeclarationSyntax.BaseList.Types
                    .OfType<SimpleBaseTypeSyntax>()
                    .FirstOrDefault(s => s.Type is GenericNameSyntax)
                is var baseNameSyntax
            && baseNameSyntax is not null
            && baseNameSyntax.Type is GenericNameSyntax genericNameSyntax
            && genericNameSyntax.Identifier.ValueText == BaseTypeMarker
            && genericNameSyntax.TypeArgumentList.Arguments
                .OfType<IdentifierNameSyntax>()
                .FirstOrDefault() is var typeArgumentSyntax
            && typeArgumentSyntax is not null)
        {
            var methods = interfaceDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>().ToArray();
            Captures[typeArgumentSyntax.Identifier.ValueText] = (interfaceDeclarationSyntax.Identifier.ValueText, methods);
        }
    }
}