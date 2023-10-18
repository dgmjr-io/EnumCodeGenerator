/*
 * EnumerationGenerator.cs
 *
 *   Created: 2023-10-10-09:00:48
 *   Modified: 2023-10-12-08:28:38
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGenerator;

extern alias Scrib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

using Dgmjr.Enumerations.CodeGenerator;
using static Dgmjr.Enumerations.CodeGenerator.Constants;
using Dgmjr.Enumerations.CodeGenerator.Logging;

using IigInitCtx = IncrementalGeneratorInitializationContext;

using Scrib::Scriban.Parsing;

using SrcProdCtx = SourceProductionContext;

[Generator]
public class EnumerationGenerator : IIncrementalGenerator, ILog
{
    public ILogger Logger => (_logger as ILogger)!;

    private IDisposable _logger;

    public void Initialize(IigInitCtx context)
    {
        context.RegisterPostInitializationOutput(
            ctx =>
                ctx.AddSource(
                    $"{GenerateEnumerationTypeAttributes}.g.cs",
                    RenderEnumerationTypeAttributeDeclarations()
                )
        );

        // if (!Debugger.IsAttached)
        // {
        //     Debugger.Launch();
        // }

        using (
            _logger = (
                new LoggerProvider(context).CreateLogger<EnumerationGenerator>()
                as IDisposable
            )!
        )
        {
            foreach (var attributeClass in AttributeClasses)
            {
                Logger?.LogCheckingAttributeClass(attributeClass);

                var valuesProvider = context.SyntaxProvider
                    .ForAttributeWithMetadataName(attributeClass, Selector, Transformer)
                    .Collect();
                context.RegisterPostInitializationOutput(
                    ctx =>
                        ctx.AddSource(
                            $"{GenerateEnumerationTypeAttributes}_{attributeClass}_Classes.g.cs",
                            $"/* {valuesProvider} */"
                        )
                );
                Logger?.LogValuesProvider(valuesProvider);
                context.RegisterSourceOutput(valuesProvider, Generate);
            }

            Logger?.EndLog();
        }
    }

    private bool Selector(SyntaxNode node, CancellationToken _)
    {
        Logger?.LogCheckingNode(node);
        Logger?.LogIsEnumDeclarationSyntax(node);
        return node is EnumDeclarationSyntax enumDeclarationSyntax
            && AttributeClasses
                .Intersect(
                    enumDeclarationSyntax.AttributeLists.SelectMany(
                        al =>
                            al.Attributes
                                .Select(a => a.Name.ToFullString())
                                .Concat(
                                    al.Attributes.Select(
                                        a => a.Name.ToFullString() + nameof(Attribute)
                                    )
                                )
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
        Logger?.LogTargetEnums(values);
        foreach (
            var enumSymbol in values
                .Select(v => v.TargetSymbol is INamedTypeSymbol ints ? ints : null)
                .WhereNotNull()
        )
        {
            var fileName = $"{enumSymbol}_Selected.g.cs";
            context.AddSource(fileName, "");

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

            var baseType =
                GetAttributeValue<type>(recordStructAttribute, 2)
                ?? GetAttributeValue<type>(structAttribute, 2)
                ?? GetAttributeValue<type>(recordClassAttribute, 2)
                ?? GetAttributeValue<type>(classAttribute, 2)
                ?? null;

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

            Logger?.LogGeneratingInterfaceDeclaration(dtoTypeName);
            // Generate the interface for the enum
            var interfaceDeclaration = GenerateInterfaceDeclaration(
                enumSymbol,
                dtoTypeName,
                dtoNamespace,
                baseType
            );
            fileName = $"I{dtoTypeName}.g.cs";
            context.AddSource(
                fileName,
                $"""
            {RenderHeader(fileName)}

            {interfaceDeclaration.NormalizeWhitespace().GetText()}
            """
            );

            Logger?.LogGeneratingDtoDeclaration(dtoTypeName);
            // Generate the data structure for the enum
            var dataStructureDeclaration = GenerateDataStructureDeclaration(
                enumSymbol,
                dtoTypeName,
                dtoNamespace,
                dataStructureType,
                baseType
            );
            context.AddSource(
                fileName,
                $"""
            {RenderHeader(fileName)}

            {dataStructureDeclaration.NormalizeWhitespace().GetText()}
            """
            );

            // Generate nested classes for each enum field
            foreach (var fieldSymbol in enumSymbol.GetMembers().OfType<IFieldSymbol>())
            {
                // Get the field name and value
                var fieldName = fieldSymbol.Name;
                var fieldValue = fieldSymbol.ConstantValue;

                Logger?.LogGeneratingNestedDataStructureDeclaration(dtoTypeName, fieldName);

                // Generate the nested class declaration
                var nestedClassDeclaration = GenerateNestedClassDeclaration(
                    enumSymbol,
                    dtoTypeName,
                    fieldName,
                    fieldValue,
                    dataStructureType,
                    dtoNamespace
                );

                fileName = $"{dtoTypeName}.{fieldName}.g.cs";
                context.AddSource(
                    fileName,
                    $"""
                    {RenderHeader(fileName)}
                    {nestedClassDeclaration.NormalizeWhitespace().GetText()}
                    """
                );
            }
        }
    }

    private static CompilationUnitSyntax GenerateInterfaceDeclaration(
        INamedTypeSymbol enumSymbol,
        string dtoTypeName,
        string dtoNamespace,
        type? baseType
    )
    {
        return ParseCompilationUnit(
            RenderIEnumerationDeclaration(
                new EnumerationDto(enumSymbol, dtoTypeName, dtoNamespace, baseType)
            )
        );
    }

    private CompilationUnitSyntax GenerateDataStructureDeclaration(
        INamedTypeSymbol enumSymbol,
        string dtoTypeName,
        string dtoNamespace,
        string dataStructureType,
        type baseType
    )
    {
        Logger?.LogInformation($"Generating DTO type {dtoTypeName}...");

        return ParseCompilationUnit(
            RenderEnumerationDeclaration(
                new EnumerationDto(enumSymbol, dtoTypeName, dtoNamespace, baseType)
            )
        );
    }

    private CompilationUnitSyntax GenerateNestedClassDeclaration(
        INamedTypeSymbol enumSymbol,
        string dtoTypeName,
        string fieldName,
        object fieldValue,
        string dataStructureType,
        string dtoNamespace
    )
    {
        Logger?.LogInformation($"Generating nested class for {enumSymbol.Name}.{fieldValue}");

        var nestedTypeDeclaration = ParseCompilationUnit(
            RenderNestedEnumerationTypeDeclaration(
                new EnumerationFieldDto(
                    enumSymbol.GetMembers(fieldName).OfType<IFieldSymbol>().FirstOrDefault(),
                    dataStructureType,
                    dtoTypeName,
                    dtoNamespace,
                    enumSymbol.EnumUnderlyingType.ToDisplayString(),
                    enumSymbol.MetadataName
                )
            )
        );

        return nestedTypeDeclaration;
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
