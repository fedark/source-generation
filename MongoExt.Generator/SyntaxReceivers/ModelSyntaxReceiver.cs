using Microsoft.CodeAnalysis;

namespace MongoExt.Generator.SyntaxReceivers;

internal class ModelSyntaxReceiver : ISyntaxReceiver
{
    public ModelAttributeSyntaxReceiver ModelAttribute { get; } = new();
    public CollectionDefinitionAttributeSyntaxReceiver CollectionDefinitionAttribute { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        ModelAttribute.OnVisitSyntaxNode(syntaxNode);
        CollectionDefinitionAttribute.OnVisitSyntaxNode(syntaxNode);
    }
}