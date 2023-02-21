using Microsoft.CodeAnalysis;

namespace MongoDbAccess.Generators
{
	internal class ModelSyntaxReceiver : ISyntaxReceiver
	{
        public ModelAttributeSyntaxReceiver ModelAttribute { get; } = new();
        public CollectionDefinitionAttributeSyntaxReceiver CollectionDefinitionAttribute { get; } = new();
        public CollectionImplementationAttributeSyntaxReceiver CollectionImplementationAttribute { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            ModelAttribute.OnVisitSyntaxNode(syntaxNode);
            CollectionDefinitionAttribute.OnVisitSyntaxNode(syntaxNode);
            CollectionImplementationAttribute.OnVisitSyntaxNode(syntaxNode);
        }
    }
}
