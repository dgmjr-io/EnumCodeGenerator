namespace Dgmjr.Enumerations.CodeGenerator.Tests;

using Dgmjr.CodeGeneration.Testing;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System;
using System.Reflection;

public abstract class CSharpSourceGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : IIncrementalGenerator, new()
{
    const string HttpMethodsEnum_cs = "HttpMethodsEnum.cs";
    const string ExitCodeEnum_cs = "ExitCodeEnum.cs";

    [Fact]
    public void SimpleGeneratorTest()
    {
        var inputCompilation = CreateCompilation(
            new StreamReader(
                GetType().Assembly.GetManifestResourceStream(HttpMethodsEnum_cs)
            ).ReadToEnd()
        );

        // directly create an instance of the generator
        // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
        var generator = new TSourceGenerator();

        // Create the driver that will control the generation, passing in our generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator.AsSourceGenerator());

        // Run the generation pass
        // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
        driver = driver.RunGeneratorsAndUpdateCompilation(
            inputCompilation,
            out var outputCompilation,
            out var diagnostics
        );

        // We can now assert things about the resulting compilation:
        diagnostics.Should().BeEmpty(); // there were no diagnostics created by the generators
        // outputCompilation.SyntaxTrees.Count().Should().Be(2); // we have two syntax trees, the original 'user' provided one, and the one added by the generator
        outputCompilation.GetDiagnostics().Should().BeEmpty(); // verify the compilation with the added source has no diagnostics

        // Or we can look at the results directly:
        var runResult = driver.GetRunResult();

        // The runResult contains the combined results of all generators passed to the driver
        runResult.GeneratedTrees.Length.Should().Be(1);
        runResult.Diagnostics.Should().BeEmpty();

        // Or you can access the individual results on a by-generator basis
        // var generatorResult = runResult.Results[0];
        // Debug.Assert(generatorResult.Generator == generator);
        // Debug.Assert(generatorResult.Diagnostics.IsEmpty);
        // Debug.Assert(generatorResult.GeneratedSources.Length == 1);
        // Debug.Assert(generatorResult.Exception is null);
    }

    private static Compilation CreateCompilation(string source) =>
        CSharpCompilation.Create(
            "compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[]
            {
                MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location)
            },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
}

public class EnumSourceGeneratorTest
    : CSharpSourceGeneratorVerifier<Dgmjr.Enumerations.CodeGenerator.EnumDataStructureGenerator> { }
