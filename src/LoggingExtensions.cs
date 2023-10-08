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

namespace Dgmjr.Enumerations.CodeGenerator.Logging;

using Microsoft.Extensions.Logging;
using static Microsoft.Extensions.Logging.LogLevel;
using static EventIds;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

public static partial class LoggingExtensions
{
    private static readonly Action<ILogger, string, Exception?> _LogNewScope =
        LoggerMessage.Define<string>(Trace, LogTransactionScopeStartedId, "{0}: ");
    private static readonly Action<
        ILogger,
        IIncrementalGenerator,
        DateTimeOffset,
        Exception?
    > _beginLog = LoggerMessage.Define<IIncrementalGenerator, DateTimeOffset>(
        Trace,
        LogBeginLogId,
        "Begin Log for {0} {1:u}"
    );
    private static readonly Action<ILogger, string, DateTimeOffset, Exception?> _beginLog2 =
        LoggerMessage.Define<string, DateTimeOffset>(
            Trace,
            LogBeginLogId,
            "Begin Log for {0} {1:u}"
        );

    public static void LogNewScope(this ILogger logger, string newScope) =>
        _LogNewScope(logger, newScope, null);

    public static void LogBeginLog(
        this ILogger logger,
        IIncrementalGenerator generator,
        DateTimeOffset dto
    ) => _beginLog(logger, generator, dto.ToLocalTime(), null);

    public static void LogBeginLog(this ILogger logger, string generator, DateTimeOffset dto) =>
        _beginLog2(logger, generator, dto.ToLocalTime(), null);

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
    ) =>
        logger.LogTargetEnums(
            values.Length,
            Join(",", values.Select(value => value.TargetSymbol.MetadataName))
        );

    [LoggerMessage(
        LogTargetEnumsId,
        Information,
        "Target enums: {ValuesString}; length: {ValuesLength}"
    )]
    private static partial void LogTargetEnums(
        this ILogger logger,
        int valuesLength,
        string valuesString
    );

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

    public static void LogValuesProvider(
        this ILogger logger,
        IncrementalValueProvider<ImmutableArray<GeneratorAttributeSyntaxContext>> valuesProvider
    ) => logger.LogValuesProvider(Join(", ", valuesProvider.Select((_, __) => _.Length)));

    [LoggerMessage(LogValuesProviderId, Information, "Values: {Count}")]
    private static partial void LogValuesProvider(this ILogger logger, string count);

    [LoggerMessage(EndOfLogId, Information, "--- END OF LOG --")]
    public static partial void EndLog(this ILogger logger);
}

public static class EventIds
{
    public const int LogBeginLogId = 0;
    public const int LogCheckingAttributeClassId = 1;
    public const int LogCheckingNodeId = 2;
    public const int LogIsEnumDeclarationSyntaxId = 3;
    public const int LogTargetEnumsId = 4;
    public const int LogGeneratingInterfaceDeclarationId = 5;
    public const int LogGeneratingDtoDeclarationId = 6;
    public const int LogGeneratingNestedDataStructureDeclarationId = 7;
    public const int LogValuesProviderId = 8;
    public const int LogTransactionScopeStartedId = 9;
    public const int EndOfLogId = int.MaxValue;
}
