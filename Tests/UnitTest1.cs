namespace Dgmjr.Enumerations.CodeGenerator.Tests;

using Dgmjr.CodeGeneration.Testing;

using Microsoft.CodeAnalysis;

public class Tests
{
    [Fact]
    public void Http_Methods_Enum_Produces_Enumeration()
    {
        var foo = Dgmjr.Enumerations.CodeGenerator.Constants.AttributeClasses;
        var bar = Dgmjr.Enumerations.CodeGenerator.Constants.IEnumerationDeclarationTemplate;
        var baz = Dgmjr
            .Enumerations
            .CodeGenerator
            .Constants
            .GenerateEnumerationTypeAttributeDeclarations;
        var quz = Dgmjr
            .Enumerations
            .CodeGenerator
            .Constants
            .NestedEnumerationTypeDeclarationTemplate;
        var qar = Dgmjr.Enumerations.CodeGenerator.Constants.HeaderTemplate;
        var bat = Dgmjr.Enumerations.CodeGenerator.Constants.EnumerationDeclarationTemplate;
        Console.WriteLine(foo);
        // var compilation = Dgmjr.CodeGeneration.Testing.CodeGeneratorDriver.RunGenerators(
        //     new EnumDataStructureGenerator(),
        //     Empty<MetadataReference>(),
        //     HttpMethodsEnumSrc
        // );
        // var errors = compilation.OutputCompilation.GetDiagnostics().Errors();
        // errors.Should().BeEmpty();
        // if (errors.Any())
        // {
        //     Console.WriteLine(Join("\n", errors.Select(error => error.GetMessage()).ToArray()));
        // }
        // var enumerationTypeSymbol = compilation.OutputCompilation.Assembly.GetTypeByMetadataName(
        //     "System.Net.Http.HttpMethod"
        // );
        // enumerationTypeSymbol.Should().NotBeNull();
    }

    public static string HttpMethodsEnumSrc =>
        new StreamReader(
            typeof(Tests).Assembly.GetManifestResourceStream("HttpMethodsEnum.cs")
        ).ReadToEnd();
}
