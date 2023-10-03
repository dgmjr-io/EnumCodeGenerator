using System.Reflection.Emit;
using System.Xml.Linq;

namespace Dgmjr.Enumerations.CodeGenerator;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using Dgmjr.CodeGeneration.Logging;
using Dgmjr.Enumerations.CodeGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using Scriban.Parsing;

using static Dgmjr.Enumerations.CodeGenerator.Constants;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

using IigInitCtx = IncrementalGeneratorInitializationContext;
using SrcProdCtx = SourceProductionContext;

[Generator]
public class EnumDataStructureGenerator : IIncrementalGenerator, ILog
{
    public ILogger Logger => _logger;
    private SourceGeneratorLogger _logger;

    public void Initialize(IigInitCtx context)
    {
        // context.RegisterPostInitializationOutput(
        //     ctx =>
        //         ctx.AddSource(
        //             $"{GenerateEnumerationTypeAttributes}.g.cs",
        //             $"""
        //     {HeaderFilledIn($"{GenerateEnumerationTypeAttributes}.g.cs")}

        //     {GenerateEnumerationTypeAttributeDeclarations}
        //     """
        //         )
        // );

        if (!System.Diagnostics.Debugger.IsAttached)
        {
            System.Diagnostics.Debugger.Launch();
        }

        using (
            _logger = (
                new SourceGeneratorLoggingProvider(
                    context
                ).CreateLogger<EnumDataStructureGenerator>() as SourceGeneratorLogger
            )!
        )
        {
            foreach (var attributeClass in AttributeClasses)
            {
                Logger.LogInformation($"Checking for attribute class {attributeClass}...");

                var valuesProvider = context.SyntaxProvider
                    .ForAttributeWithMetadataName(attributeClass, Selector, Transformer)
                    .Collect();
                // context.RegisterPostInitializationOutput(
                //     ctx =>
                //         ctx.AddSource(
                //             $"{GenerateEnumerationTypeAttributes}_{attributeClass}_Classes.g.cs",
                //             $"/* {valuesProvider} */"
                //         )
                // );
                context.RegisterSourceOutput(valuesProvider, Generate);
            }
        }
    }

    private bool Selector(SyntaxNode node, CancellationToken _)
    {
        Logger.LogInformation($"Checking {node}...");
        Logger.LogInformation(
            $"node is EnumDeclarationSyntax enumDeclarationSyntax: {node is EnumDeclarationSyntax}..."
        );
        return node is EnumDeclarationSyntax enumDeclarationSyntax
            && AttributeClasses
                .Intersect(
                    enumDeclarationSyntax.AttributeLists.SelectMany(
                        al => al.Attributes.Select(a => a.Name.ToFullString())
                    )
                )
                .Any();
    }

    internal GeneratorAttributeSyntaxContext Transformer(
        GeneratorAttributeSyntaxContext context,
        CancellationToken _
    ) => context;

    private void Generate(
        SrcProdCtx context,
        ImmutableArray<GeneratorAttributeSyntaxContext> values
    )
    {
        Logger.LogInformation(
            $"Target enums: {Join(", ", values.Select(value => value.TargetSymbol.MetadataName))}"
        );
        foreach (
            var enumSymbol in values
                .Select(v => v.TargetSymbol is INamedTypeSymbol ints ? ints : null)
                .WhereNotNull()
        )
        {
            context.AddSource($"{enumSymbol}_Selected.g.cs", "");

            // Check if the enum has any of the specified attributes
            var attributes = enumSymbol.GetAttributes();
            var recordStructAttribute = attributes.FirstOrDefault(
                a => a.AttributeClass?.MetadataName == GenerateEnumerationRecordStructAttribute
            );
            var structAttribute = attributes.FirstOrDefault(
                a => a.AttributeClass?.MetadataName == GenerateEnumerationStructAttribute
            );
            var recordClassAttribute = attributes.FirstOrDefault(
                a => a.AttributeClass?.MetadataName == GenerateEnumerationRecordClassAttribute
            );
            var classAttribute = attributes.FirstOrDefault(
                a => a.AttributeClass?.MetadataName == GenerateEnumerationClassAttribute
            );

            // Determine the DTO class name and namespace
            var dtoTypeName =
                GetAttributeValue<string>(recordStructAttribute, 0)
                ?? GetAttributeValue<string>(structAttribute, 0)
                ?? GetAttributeValue<string>(recordClassAttribute, 0)
                ?? GetAttributeValue<string>(classAttribute, 0)
                ?? $"{enumSymbol.Name}Enumeration";

            var dtoNamespace =
                GetAttributeValue<string>(recordStructAttribute, 1)
                ?? GetAttributeValue<string>(structAttribute, 1)
                ?? GetAttributeValue<string>(recordClassAttribute, 1)
                ?? GetAttributeValue<string>(classAttribute, 1)
                ?? enumSymbol.ContainingNamespace.MetadataName;

            var dataStructureType =
                recordStructAttribute != null
                    ? record_struct
                    : structAttribute != null
                        ? @struct
                        : recordClassAttribute != null
                            ? record_class
                            : classAttribute != null
                                ? @class
                                : "invalid";

            Logger.LogInformation($"Generating interface declaration for I${dtoTypeName}...");
            // Generate the interface for the enum
            var interfaceDeclaration = GenerateInterfaceDeclaration(
                enumSymbol,
                dtoTypeName,
                dtoNamespace
            );
            context.AddSource(
                $"I{dtoTypeName}.g.cs",
                $"""
            {HeaderFilledIn($"I{dtoTypeName}.g.cs")}

            {interfaceDeclaration.NormalizeWhitespace().GetText()}
            """
            );

            Logger.LogInformation($"Generating data structute declaration for ${dtoTypeName}...");
            // Generate the data structure for the enum
            var dataStructureDeclaration = GenerateDataStructureDeclaration(
                enumSymbol,
                dtoTypeName,
                dtoNamespace,
                dataStructureType
            );

            // Generate nested classes for each enum field
            dataStructureDeclaration = dataStructureDeclaration.WithMembers(
                List(
                    enumSymbol
                        .GetMembers()
                        .OfType<IFieldSymbol>()
                        .Select(fieldSymbol =>
                        {
                            // Get the field name and value
                            var fieldName = fieldSymbol.Name;
                            var fieldValue = fieldSymbol.ConstantValue;

                            Logger.LogInformation(
                                $"Generating nested data structute declaration for ${fieldName}..."
                            );

                            // Generate the nested class declaration
                            var nestedClassDeclaration = GenerateNestedClassDeclaration(
                                enumSymbol,
                                dtoTypeName,
                                fieldName,
                                fieldValue,
                                dataStructureType,
                                dtoNamespace
                            );
                            return nestedClassDeclaration as MemberDeclarationSyntax;
                        })
                        .ToArray()
                )
            );

            // Add the data structure to the compilation
            context.AddSource(
                $"{dtoTypeName}s.g.cs",
                $"""
            {HeaderFilledIn($"{dtoTypeName}s.g.cs")}

            {dataStructureDeclaration.NormalizeWhitespace().GetText()}
            """
            );
        }
    }

    private static CompilationUnitSyntax GenerateInterfaceDeclaration(
        INamedTypeSymbol enumSymbol,
        string dtoTypeName,
        string dtoNamespace
    )
    {
        // Create the base type for the interface
        var baseType = ParseTypeName(
            $"IEnumeration<{dtoNamespace}.I{dtoTypeName}, {enumSymbol.EnumUnderlyingType}, {enumSymbol.ContainingNamespace.ToDisplayString()}.{enumSymbol.Name}>"
        );

        // Create the interface declaration
        var interfaceDeclaration = InterfaceDeclaration($"I{dtoTypeName}")
            .AddModifiers(Token(PublicKeyword))
            .AddBaseListTypes(SimpleBaseType(baseType));

        var namespaceDeclaration = NamespaceDeclaration(ParseName(dtoNamespace))
            .AddMembers(interfaceDeclaration);

        return CompilationUnit()
            .AddUsings(
                UsingDirective(ParseName("System")),
                UsingDirective(ParseName("System.Reflection")),
                UsingDirective(ParseName("Dgmjr.Enumerations.Abstractions")),
                UsingDirective(
                    AliasQualifiedName(
                        IdentifierName(enumSymbol.ToDisplayString()),
                        IdentifierName("TheEnum")
                    )
                )
            )
            .AddMembers(namespaceDeclaration);
    }

    private CompilationUnitSyntax GenerateDataStructureDeclaration(
        INamedTypeSymbol enumSymbol,
        string dtoTypeName,
        string dtoNamespace,
        string dataStructureType
    )
    {
        Logger.LogInformation($"Generating DTO type {dtoTypeName}...");
        // Create the base type for the data structure
        var baseType = ParseTypeName(dtoTypeName);

        // Create the data structure declaration
        var dataStructureDeclaration = ClassDeclaration(dtoTypeName)
            .AddModifiers(Token(PublicKeyword))
            .AddBaseListTypes(SimpleBaseType(baseType));

        // Create the non-public parameterless constructor
        var constructorDeclaration = ConstructorDeclaration(dtoTypeName)
            .AddModifiers(Token(PrivateKeyword))
            .WithBody(Block());

        // Add the constructor to the data structure
        dataStructureDeclaration = dataStructureDeclaration.AddMembers(constructorDeclaration);

        dataStructureDeclaration = dataStructureDeclaration.AddMembers(
            enumSymbol
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Select(
                    fieldSymbol =>
                        GenerateNestedClassDeclaration(
                            enumSymbol,
                            dtoTypeName,
                            fieldSymbol.Name,
                            fieldSymbol.ConstantValue,
                            dataStructureType,
                            dtoNamespace
                        )
                )
                .OfType<MemberDeclarationSyntax>()
                .ToArray()
        );

        var namespaceDeclaration = NamespaceDeclaration(ParseName(dtoNamespace))
            .AddMembers(dataStructureDeclaration);

        var compilationUnit = CompilationUnit()
            .AddUsings(
                UsingDirective(ParseName("System")),
                UsingDirective(ParseName("System.Reflection")),
                UsingDirective(ParseName("Dgmjr.Enumerations.Abstractions"))
            )
            .AddMembers(namespaceDeclaration);

        return compilationUnit;
    }

    private TypeDeclarationSyntax GenerateNestedClassDeclaration(
        INamedTypeSymbol enumSymbol,
        string dtoTypeName,
        string fieldName,
        object fieldValue,
        string dataStructureType,
        string dtoNamespace
    )
    {
        Logger.LogInformation($"Generating nested class for {enumSymbol.Name}.{fieldValue}");

        var nestedTypeDeclaration = ParseSyntaxTree(
                NestedEnumerationTypeDeclarationTemplate.Render(
                    new EnumerationFieldDto(
                        enumSymbol.GetMembers(fieldName).OfType<IFieldSymbol>().FirstOrDefault(),
                        dataStructureType,
                        dtoTypeName,
                        dtoNamespace,
                        enumSymbol.EnumUnderlyingType.ToDisplayString(),
                        enumSymbol.MetadataName
                    )
                )
            )
            .GetCompilationUnitRoot()
            .DescendantNodesAndSelf(_ => true)
            .First(n => n is TypeDeclarationSyntax);

        return (nestedTypeDeclaration as TypeDeclarationSyntax)!;
    }

    private static T? GetAttributeValue<T>(AttributeData? attributeData, int index)
    {
        if (attributeData is not null)
        {
            var namedArgument = attributeData.ConstructorArguments.Skip(index).FirstOrDefault();
            if (namedArgument.Value is T value)
                return value;
        }
        return default;
    }
}
