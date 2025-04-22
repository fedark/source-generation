using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace MongoExt.Generator;

[Generator]
public class RepositoryGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c =>
        {
            c.AddSource(
                $"{Names.EntityAttributeFull}.g.cs",
                SourceText.From(Code.EntityAttribute(), Encoding.UTF8));
        });

        var targetModels = context.SyntaxProvider.ForAttributeWithMetadataName(
                $"{Names.Namespace}.{Names.EntityAttributeFull}",
                IsTargetSyntax,
                GetTargetModel)
            .Where(m => m is not null);

        context.RegisterSourceOutput(
            targetModels,
            static (c, m) => GenerateForModel(c, m!.Value));
    }

    private static bool IsTargetSyntax(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is ClassDeclarationSyntax classDeclaration
            && classDeclaration.BaseList is not null
            && classDeclaration.BaseList.Types
                .OfType<SimpleBaseTypeSyntax>()
                .Any(s => s.Type is IdentifierNameSyntax { Identifier.ValueText: Names.EntityInterface })
            && classDeclaration.AttributeLists.FirstOrDefault() is { } attributeList
            && attributeList.Attributes.FirstOrDefault() is { } attribute
            && attribute.ArgumentList is not null
            && attribute.ArgumentList.Arguments is var arguments
            && arguments.Count == 2
            && arguments[0].Expression is LiteralExpressionSyntax
            && arguments[1].Expression is LiteralExpressionSyntax;
    }

    private static EntityData? GetTargetModel(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;

        var @namespace = classDeclaration.Parent is BaseNamespaceDeclarationSyntax namespaceDeclaration
            ? ((IdentifierNameSyntax)namespaceDeclaration.Name).Identifier.ValueText
            : Names.Namespace;

        var entityName = classDeclaration.Identifier.ValueText;

        var attributeArguments = classDeclaration.AttributeLists.First().Attributes.First().ArgumentList!.Arguments;
        var collectionName = ((LiteralExpressionSyntax)attributeArguments[0].Expression).Token.ValueText;
        var expirationMinutes = int.Parse(((LiteralExpressionSyntax)attributeArguments[1].Expression).Token.ValueText);

        return new(@namespace, entityName, collectionName, expirationMinutes);
    }

    private static void GenerateForModel(SourceProductionContext context, EntityData model)
    {
        //context.AddSource("trace.g.cs", SourceText.From(Code.Trace("trace"), Encoding.UTF8));

        context.AddSource(
            $"I{model.EntityName}Repository.g.cs",
            SourceText.From(Code.EntityRepositoryInterface(model.Namespace, model.EntityName), Encoding.UTF8));

        context.AddSource(
            $"{model.EntityName}Repository.g.cs",
            SourceText.From(Code.EntityRepository(model.Namespace, model.EntityName, model.CollectionName, model.ExpirationMinutes), Encoding.UTF8));
    }

    private readonly record struct EntityData
    {
        public readonly string Namespace;
        public readonly string EntityName;
        public readonly string CollectionName;
        public readonly int ExpirationMinutes;

        public EntityData(string @namespace, string entityName, string collectionName, int expirationMinutes)
        {
            Namespace = @namespace;
            EntityName = entityName;
            CollectionName = collectionName;
            ExpirationMinutes = expirationMinutes;
        }
    }
}
