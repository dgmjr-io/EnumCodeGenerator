/*
 * LoggingExtensions.cs
 *
 *   Created: 2023-10-03-11:48:35
 *   Modified: 2023-10-03-11:48:35
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGenerator;

using Microsoft.Extensions.Logging;
using static Microsoft.Extensions.Logging.LogLevel;
using static EventIds;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

public static partial class LoggingExtensions
{
    [LoggerMessage(
        LogCheckingAttributeClassId,
        Information,
        "Checking for attribute class {AttributeClass}..."
    )]
    public static partial void LogCheckingAttributeClass(
        this ILogger logger,
        string attributeClass
    );

    [LoggerMessage(LogCheckingNodeId, Information, "Checking node {Node}...")]
    public static partial void LogCheckingNode(this ILogger logger, SyntaxNode node);

    public static void LogIsEnumDeclarationSyntax(this ILogger logger, SyntaxNode node) =>
        logger.LogIsEnumDeclarationSyntax(node is EnumDeclarationSyntax);

    [LoggerMessage(
        LogIsEnumDeclarationSyntaxId,
        Information,
        "node is EnumDeclarationSyntax enumDeclarationSyntax: {NodeIsEnumDeclarationSyntax}"
    )]
    private static partial void LogIsEnumDeclarationSyntax(
        this ILogger logger,
        bool nodeIsEnumDeclarationSyntax
    );

    public static void LogTargetEnums(
        this ILogger logger,
        ImmutableArray<GeneratorAttributeSyntaxContext> values
    ) => logger.LogTargetEnums(Join(",", values.Select(value => value.TargetSymbol.MetadataName)));

    [LoggerMessage(LogTargetEnumsId, Information, "Target enums: {ValuesString}")]
    private static partial void LogTargetEnums(this ILogger logger, string valuesString);

    [LoggerMessage(
        LogGeneratingInterfaceDeclarationId,
        Information,
        "Generating interface declaration for I{DtoTypeName}..."
    )]
    public static partial void LogGeneratingInterfaceDeclaration(
        this ILogger logger,
        string dtoTypeName
    );

    [LoggerMessage(
        LogGeneratingDtoDeclarationId,
        Information,
        "Generating DTO declaration for I{DtoTypeName}..."
    )]
    public static partial void LogGeneratingDtoDeclaration(this ILogger logger, string dtoTypeName);

    [LoggerMessage(
        LogGeneratingNestedDataStructureDeclarationId,
        Information,
        "Generating nested data structure declaration for {DtoTypeName}.{FieldName}..."
    )]
    public static partial void LogGeneratingNestedDataStructureDeclaration(
        this ILogger logger,
        string dtoTypeName,
        string fieldName
    );

    [LoggerMessage(EndOfLogId, Information, "--- END OF LOG --")]
    public static partial void EndLog(this ILogger logger);
}

public static class EventIds
{
    public const int LogCheckingAttributeClassId = 1;
    public const int LogCheckingNodeId = 2;
    public const int LogIsEnumDeclarationSyntaxId = 3;
    public const int LogTargetEnumsId = 4;
    public const int LogGeneratingInterfaceDeclarationId = 5;
    public const int LogGeneratingDtoDeclarationId = 6;
    public const int LogGeneratingNestedDataStructureDeclarationId = 7;
    public const int EndOfLogId = int.MaxValue;
}
