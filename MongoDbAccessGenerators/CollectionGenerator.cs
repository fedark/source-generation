using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MongoDbAccessGenerators
{
    [Generator]
	public class CollectionGenerator : ISourceGenerator
	{
        public void Execute(GeneratorExecutionContext context)
        {
			if (context.SyntaxReceiver is not CollectionAttributeSyntaxReceiver syntaxReceiver)
			{
				return;
			}

			foreach (var (@namespace, className, expirationMinutes) in syntaxReceiver.Captures)
			{
                context.AddSource($"{className}Collection.g.cs",
                    GetBaseClass(@namespace, className, expirationMinutes).GetText(Encoding.UTF8));

                context.AddSource($"Mongo{className}Collection.g.cs",
                    GetImplClass(@namespace, className).GetText(Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
			context.RegisterForSyntaxNotifications(() => new CollectionAttributeSyntaxReceiver());
        }

		private CompilationUnitSyntax GetBaseClass(NamespaceDeclarationSyntax namespaceDeclarationSyntax, string modelName, int expirationMinutes)
		{
            return CompilationUnit()
            .WithUsings(
				List(
                    new UsingDirectiveSyntax[] 
                    {
                        UsingDirective(
                            IdentifierName("System")),
                        UsingDirective(
                            QualifiedName(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("Microsoft"),
                                        IdentifierName("Extensions")),
                                    IdentifierName("Caching")),
                                IdentifierName("Memory"))),
                        UsingDirective(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("MongoDbAccess"),
                                    IdentifierName("DataAccess")),
                                IdentifierName("Abstractions")))
                    }))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(namespaceDeclarationSyntax.Name)
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration($"{modelName}Collection")
                            .WithModifiers(
                                TokenList(
                                    new[]{
                                        Token(SyntaxKind.PublicKeyword),
                                        Token(SyntaxKind.AbstractKeyword)}))
                            .WithBaseList(
                                BaseList(
                                    SingletonSeparatedList<BaseTypeSyntax>(
                                        SimpleBaseType(
                                            GenericName(
                                                Identifier("CachedMongoCollection"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        IdentifierName(modelName))))))))
                            .WithMembers(
								List(
                                    new MemberDeclarationSyntax[]{
                                        PropertyDeclaration(
                                            IdentifierName("TimeSpan"),
                                            Identifier("CacheExpiration"))
                                        .WithModifiers(
                                            TokenList(
                                                new []{
                                                    Token(SyntaxKind.ProtectedKeyword),
                                                    Token(SyntaxKind.OverrideKeyword)}))
                                        .WithExpressionBody(
                                            ArrowExpressionClause(
                                                InvocationExpression(
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("TimeSpan"),
                                                        IdentifierName("FromMinutes")))
                                                .WithArgumentList(
                                                    ArgumentList(
														SingletonSeparatedList(
                                                            Argument(
                                                                LiteralExpression(
                                                                    SyntaxKind.NumericLiteralExpression,
                                                                    Literal(expirationMinutes))))))))
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken)),
                                        ConstructorDeclaration(
                                            Identifier($"{modelName}Collection"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.ProtectedKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SeparatedList<ParameterSyntax>(
                                                    new SyntaxNodeOrToken[]{
                                                        Parameter(
                                                            Identifier("db"))
                                                        .WithType(
                                                            IdentifierName("IDbConnection")),
                                                        Token(SyntaxKind.CommaToken),
                                                        Parameter(
                                                            Identifier("cache"))
                                                        .WithType(
                                                            IdentifierName("IMemoryCache"))})))
                                        .WithInitializer(
                                            ConstructorInitializer(
                                                SyntaxKind.BaseConstructorInitializer,
                                                ArgumentList(
                                                    SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]{
                                                            Argument(
                                                                IdentifierName("db")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(
                                                                IdentifierName("cache"))}))))
                                        .WithBody(
                                            Block())}))))))
            .NormalizeWhitespace();
        }
    
        private CompilationUnitSyntax GetImplClass(NamespaceDeclarationSyntax namespaceDeclarationSyntax, string modelName)
		{
            return CompilationUnit()
                .WithUsings(
					List(
                        new UsingDirectiveSyntax[]{
                            UsingDirective(
                                QualifiedName(
                                    QualifiedName(
                                        QualifiedName(
                                            IdentifierName("Microsoft"),
                                            IdentifierName("Extensions")),
                                        IdentifierName("Caching")),
                                    IdentifierName("Memory"))),
                            UsingDirective(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("MongoDbAccess"),
                                        IdentifierName("DataAccess")),
                                    IdentifierName("Abstractions")))}))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(namespaceDeclarationSyntax.Name)
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration($"Mongo{modelName}Collection")
                                .WithModifiers(
                                    TokenList(
                                        Token(SyntaxKind.PublicKeyword)))
                                .WithBaseList(
                                    BaseList(
                                        SingletonSeparatedList<BaseTypeSyntax>(
                                            SimpleBaseType(
                                                IdentifierName($"{modelName}Collection")))))
                                .WithMembers(
                                    SingletonList<MemberDeclarationSyntax>(
                                        ConstructorDeclaration(
                                            Identifier($"Mongo{modelName}Collection"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SeparatedList<ParameterSyntax>(
                                                    new SyntaxNodeOrToken[]{
                                                        Parameter(
                                                            Identifier("db"))
                                                        .WithType(
                                                            IdentifierName("IDbConnection")),
                                                        Token(SyntaxKind.CommaToken),
                                                        Parameter(
                                                            Identifier("cache"))
                                                        .WithType(
                                                            IdentifierName("IMemoryCache"))})))
                                        .WithInitializer(
                                            ConstructorInitializer(
                                                SyntaxKind.BaseConstructorInitializer,
                                                ArgumentList(
                                                    SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]{
                                                            Argument(
                                                                IdentifierName("db")),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(
                                                                IdentifierName("cache"))}))))
                                        .WithBody(
                                            Block())))))))
                .NormalizeWhitespace();
        }
    }

	public class CollectionAttributeSyntaxReceiver : ISyntaxReceiver
	{
		private const string BaseTypeMarker = "IModel";
		private static readonly string AttributeMarker = "CachedCollection";

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
				&& classDeclarationSyntax.BaseList.Types.OfType<SimpleBaseTypeSyntax>()
					.Any(s => s.Type is IdentifierNameSyntax identifierNameSyntax
						&& identifierNameSyntax.Identifier.ValueText == BaseTypeMarker)
				&& classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
			{
				Captures.Add((namespaceDeclarationSyntax, classDeclarationSyntax.Identifier.ValueText, expiration));
			}
		}
	}
}
