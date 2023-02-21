using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MongoDbAccess.Generators
{
    [Generator]
	public class CollectionGenerator : ISourceGenerator
	{
        public void Execute(GeneratorExecutionContext context)
        {
			if (context.SyntaxReceiver is not ModelAttributeSyntaxReceiver syntaxReceiver)
			{
				return;
			}

			foreach (var (@namespace, className, expirationMinutes) in syntaxReceiver.ModelCaptures)
			{
				if (!syntaxReceiver.CollectionInterfaceCaptures.TryGetValue(className, out var interfaceName))
				{
                    interfaceName = $"I{className}Collection";

                    context.AddSource($"I{className}Collection.g.cs",
                        GetInterface(@namespace, className).GetText(Encoding.UTF8));
                }

				context.AddSource($"Mongo{className}Collection.g.cs",
                    GetImplClass(@namespace, interfaceName, className, expirationMinutes).GetText(Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
			context.RegisterForSyntaxNotifications(() => new ModelAttributeSyntaxReceiver());
        }

        private CompilationUnitSyntax GetInterface(NamespaceDeclarationSyntax namespaceDeclarationSyntax, string modelName)
		{
            return CompilationUnit()
                .WithUsings(
                    SingletonList<UsingDirectiveSyntax>(
                        UsingDirective(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("MongoDbAccess"),
                                    IdentifierName("DataAccess")),
                                IdentifierName("Abstractions")))))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(namespaceDeclarationSyntax.Name)
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                InterfaceDeclaration($"I{modelName}Collection")
                                .WithModifiers(
                                    TokenList(
                                        new[]{
                                            Token(SyntaxKind.PublicKeyword)}))
                                .WithBaseList(
                                    BaseList(
                                        SingletonSeparatedList<BaseTypeSyntax>(
                                            SimpleBaseType(
                                                GenericName(
                                                    Identifier("IDbCollection"))
                                                .WithTypeArgumentList(
                                                    TypeArgumentList(
                                                        SingletonSeparatedList<TypeSyntax>(
                                                            IdentifierName(modelName))))))))))))
                .NormalizeWhitespace();

        }

		private CompilationUnitSyntax GetImplClass(NamespaceDeclarationSyntax namespaceDeclarationSyntax, string baseTypeName, string modelName, int expirationMinutes)
		{
            return CompilationUnit()
            .WithUsings(
                List<UsingDirectiveSyntax>(
                    new UsingDirectiveSyntax[]{
                        UsingDirective(
                            IdentifierName("System")),
                        UsingDirective(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("System"),
                                    IdentifierName("Collections")),
                                IdentifierName("Generic"))),
                        UsingDirective(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("System"),
                                    IdentifierName("Threading")),
                                IdentifierName("Tasks"))),
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
                                IdentifierName("MongoDB"),
                                IdentifierName("Driver"))),
                        UsingDirective(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("MongoDbAccess"),
                                    IdentifierName("DataAccess")),
                                IdentifierName("Abstractions"))),
                        UsingDirective(
                            QualifiedName(
                                IdentifierName("MongoDbAccess"),
                                IdentifierName("DataAccess")))}))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(namespaceDeclarationSyntax.Name)
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration($"Mongo{modelName}Collection")
                            .WithModifiers(
                                TokenList(
                                    new[]{
                                        Token(SyntaxKind.PublicKeyword),
                                        Token(SyntaxKind.PartialKeyword)}))
                            .WithBaseList(
                                BaseList(
                                    SingletonSeparatedList<BaseTypeSyntax>(
                                        SimpleBaseType(
                                            IdentifierName(baseTypeName)))))
                            .WithMembers(
                                List<MemberDeclarationSyntax>(
                                    new MemberDeclarationSyntax[]{
                                        PropertyDeclaration(
                                            GenericName(
                                                Identifier("IMongoCollection"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        IdentifierName(modelName)))),
                                            Identifier("Documents"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.ProtectedKeyword)))
                                        .WithAccessorList(
                                            AccessorList(
                                                SingletonList<AccessorDeclarationSyntax>(
                                                    AccessorDeclaration(
                                                        SyntaxKind.GetAccessorDeclaration)
                                                    .WithSemicolonToken(
                                                        Token(SyntaxKind.SemicolonToken))))),
                                        PropertyDeclaration(
                                            GenericName(
                                                Identifier("MongoCollectionCache"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        IdentifierName(modelName)))),
                                            Identifier("Cache"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.ProtectedKeyword)))
                                        .WithAccessorList(
                                            AccessorList(
                                                SingletonList<AccessorDeclarationSyntax>(
                                                    AccessorDeclaration(
                                                        SyntaxKind.GetAccessorDeclaration)
                                                    .WithSemicolonToken(
                                                        Token(SyntaxKind.SemicolonToken))))),
                                        PropertyDeclaration(
                                            GenericName(
                                                Identifier("CachedMongoCollection"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        IdentifierName(modelName)))),
                                            Identifier("CachedCollection"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PrivateKeyword)))
                                        .WithAccessorList(
                                            AccessorList(
                                                SingletonList<AccessorDeclarationSyntax>(
                                                    AccessorDeclaration(
                                                        SyntaxKind.GetAccessorDeclaration)
                                                    .WithSemicolonToken(
                                                        Token(SyntaxKind.SemicolonToken))))),
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
                                        .WithBody(
                                            Block(
                                                LocalDeclarationStatement(
                                                    VariableDeclaration(
                                                        IdentifierName(
                                                            Identifier(
                                                                TriviaList(),
                                                                SyntaxKind.VarKeyword,
                                                                "var",
                                                                "var",
                                                                TriviaList())))
                                                    .WithVariables(
                                                        SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                            VariableDeclarator(
                                                                Identifier("name"))
                                                            .WithInitializer(
                                                                EqualsValueClause(
                                                                    InvocationExpression(
                                                                        MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            IdentifierName("db"),
                                                                            GenericName(
                                                                                Identifier("GetCollectionName"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                                        IdentifierName(modelName))))))))))),
                                                ExpressionStatement(
                                                    AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        IdentifierName("Documents"),
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("db"),
                                                                GenericName(
                                                                    Identifier("GetCollection"))
                                                                .WithTypeArgumentList(
                                                                    TypeArgumentList(
                                                                        SingletonSeparatedList<TypeSyntax>(
                                                                            IdentifierName(modelName))))))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(
                                                                        IdentifierName("name"))))))),
                                                ExpressionStatement(
                                                    AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        IdentifierName("Cache"),
                                                        ImplicitObjectCreationExpression()
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SeparatedList<ArgumentSyntax>(
                                                                    new SyntaxNodeOrToken[]{
                                                                        Argument(
                                                                            IdentifierName("cache")),
                                                                        Token(SyntaxKind.CommaToken),
                                                                        Argument(
                                                                            InvocationExpression(
                                                                                MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    IdentifierName("TimeSpan"),
                                                                                    IdentifierName("FromMinutes")))
                                                                            .WithArgumentList(
                                                                                ArgumentList(
                                                                                    SingletonSeparatedList<ArgumentSyntax>(
                                                                                        Argument(
                                                                                            LiteralExpression(
                                                                                                SyntaxKind.NumericLiteralExpression,
                                                                                                Literal(expirationMinutes)))))))}))))),
                                                ExpressionStatement(
                                                    AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        IdentifierName("CachedCollection"),
                                                        ImplicitObjectCreationExpression()
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SeparatedList<ArgumentSyntax>(
                                                                    new SyntaxNodeOrToken[]{
                                                                        Argument(
                                                                            IdentifierName("db")),
                                                                        Token(SyntaxKind.CommaToken),
                                                                        Argument(
                                                                            IdentifierName("Cache"))}))))))),
                                        MethodDeclaration(
                                            IdentifierName("Task"),
                                            Identifier("CreateAsync"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SingletonSeparatedList<ParameterSyntax>(
                                                    Parameter(
                                                        Identifier("document"))
                                                    .WithType(
                                                        IdentifierName(modelName)))))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ReturnStatement(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("CachedCollection"),
                                                                IdentifierName("CreateAsync")))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(
                                                                        IdentifierName("document"))))))))),
                                        MethodDeclaration(
                                            GenericName(
                                                Identifier("Task"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        NullableType(
                                                            IdentifierName(modelName))))),
                                            Identifier("GetAsync"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SingletonSeparatedList<ParameterSyntax>(
                                                    Parameter(
                                                        Identifier("id"))
                                                    .WithType(
                                                        PredefinedType(
                                                            Token(SyntaxKind.StringKeyword))))))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ReturnStatement(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("CachedCollection"),
                                                                IdentifierName("GetAsync")))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(
                                                                        IdentifierName("id"))))))))),
                                        MethodDeclaration(
                                            GenericName(
                                                Identifier("Task"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        GenericName(
                                                            Identifier("IList"))
                                                        .WithTypeArgumentList(
                                                            TypeArgumentList(
                                                                SingletonSeparatedList<TypeSyntax>(
                                                                    IdentifierName(modelName))))))),
                                            Identifier("GetAllAsync"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ReturnStatement(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("CachedCollection"),
                                                                IdentifierName("GetAllAsync"))))))),
                                        MethodDeclaration(
                                            IdentifierName("Task"),
                                            Identifier("UpdateAsync"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SingletonSeparatedList<ParameterSyntax>(
                                                    Parameter(
                                                        Identifier("document"))
                                                    .WithType(
                                                        IdentifierName(modelName)))))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ReturnStatement(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("CachedCollection"),
                                                                IdentifierName("UpdateAsync")))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(
                                                                        IdentifierName("document"))))))))),
                                        MethodDeclaration(
                                            IdentifierName("Task"),
                                            Identifier("RemoveAsync"))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SingletonSeparatedList<ParameterSyntax>(
                                                    Parameter(
                                                        Identifier("id"))
                                                    .WithType(
                                                        PredefinedType(
                                                            Token(SyntaxKind.StringKeyword))))))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ReturnStatement(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("CachedCollection"),
                                                                IdentifierName("RemoveAsync")))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(
                                                                        IdentifierName("id")))))))))}))))))
            .NormalizeWhitespace();
        }
    }

	internal class ModelAttributeSyntaxReceiver : ISyntaxReceiver
	{
		private static readonly string ModelBaseTypeMarker = "IModel";
		private static readonly string ModelAttributeMarker = "CachedCollection";

        private static readonly string CollectionInterfaceBaseTypeMarker = "IDbCollection";
        private static readonly string CollectionInterfaceAttributeMarker = "CollectionInterface";

        public List<(NamespaceDeclarationSyntax Namespace, string ClassName, int ExpirationMinutes)> ModelCaptures { get; } = new();
        public Dictionary<string, string> CollectionInterfaceCaptures { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
            TryCaptureModelSyntaxNode(syntaxNode);
            TryCaptureCollectionInterfaceSyntaxNode(syntaxNode);
		}

        private void TryCaptureModelSyntaxNode(SyntaxNode syntaxNode)
		{
			if (syntaxNode is AttributeSyntax attributeSyntax
				&& attributeSyntax.Name.ToString() == ModelAttributeMarker
                && attributeSyntax.ArgumentList is not null
				&& attributeSyntax.ArgumentList.Arguments is var arguments 
				&& arguments.Count == 1
				&& arguments[0].Expression is LiteralExpressionSyntax argument1
				&& int.TryParse(argument1.Token.ValueText, out var expiration)
				&& attributeSyntax.Parent?.Parent is ClassDeclarationSyntax classDeclarationSyntax
				&& classDeclarationSyntax.BaseList is not null
				&& classDeclarationSyntax.BaseList.Types.OfType<SimpleBaseTypeSyntax>()
					.Any(s => s.Type is IdentifierNameSyntax identifierNameSyntax
						&& identifierNameSyntax.Identifier.ValueText == ModelBaseTypeMarker)
				&& classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
			{
                ModelCaptures.Add((namespaceDeclarationSyntax, classDeclarationSyntax.Identifier.ValueText, expiration));
			}
		}

        private void TryCaptureCollectionInterfaceSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is AttributeSyntax attributeSyntax
                && attributeSyntax.Name.ToString() == CollectionInterfaceAttributeMarker
                && attributeSyntax.Parent?.Parent is InterfaceDeclarationSyntax interfaceDeclarationSyntax
                && interfaceDeclarationSyntax.BaseList is not null
                && interfaceDeclarationSyntax.BaseList.Types.OfType<SimpleBaseTypeSyntax>()
                    .FirstOrDefault(s => s.Type is GenericNameSyntax) is var baseNameSyntax
                && baseNameSyntax is not null
                && baseNameSyntax.Type is GenericNameSyntax genericNameSyntax
                && genericNameSyntax.Identifier.ValueText == CollectionInterfaceBaseTypeMarker
                && genericNameSyntax.TypeArgumentList.Arguments.OfType<IdentifierNameSyntax>()
                    .FirstOrDefault() is var identifierNameSyntax
                && identifierNameSyntax is not null)
            {
                CollectionInterfaceCaptures[identifierNameSyntax.Identifier.ValueText] = interfaceDeclarationSyntax.Identifier.ValueText;
            }
        }
    }
}
