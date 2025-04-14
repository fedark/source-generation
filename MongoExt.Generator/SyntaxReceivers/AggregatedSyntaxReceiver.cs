using Microsoft.CodeAnalysis;

namespace MongoExt.Generator.SyntaxReceivers;

internal class AggregatedSyntaxReceiver : ISyntaxReceiver
{
    public ModelSyntaxReceiver ModelSyntaxReceiver { get; } = new();
    public CollectionDefinitionSyntaxReceiver CollectionDefinitionSyntaxReceiver { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        ModelSyntaxReceiver.OnVisitSyntaxNode(syntaxNode);
        CollectionDefinitionSyntaxReceiver.OnVisitSyntaxNode(syntaxNode);
    }
}