using System;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;



namespace Test;

[Generator]
public sealed class TestGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var pipeline =
			context.SyntaxProvider.CreateSyntaxProvider(Predicate, Transform)
				.Collect(); // D

		context.RegisterSourceOutput(pipeline, Build); // E
	}

	private bool Predicate(SyntaxNode node, CancellationToken cancellationToken)
	{
		return node is AttributeSyntax { Name: SimpleNameSyntax name } attribute
		       && name.Identifier.Text == "Test"
		       && attribute.Parent?.Parent is ClassDeclarationSyntax;
	}

	private SyntaxToken Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
	{
		return ((ClassDeclarationSyntax)context.Node).Identifier;
	}

	private void Build(SourceProductionContext context, ImmutableArray<SyntaxToken> source)
	{
		var text = """

		           public class TestB
		           {
		           }

		           """;
		context.AddSource("TestB.cs", text);
	}

	
}
