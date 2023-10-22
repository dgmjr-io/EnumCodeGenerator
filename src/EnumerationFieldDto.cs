/*
 * EnumerationFieldDto.cs
 *
 *   Created: 2023-10-10-09:00:48
 *   Modified: 2023-10-12-08:28:34
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGenerator;

using System;
using System.Text;
using System.Text.Json;

using static Constants;

internal record struct EnumerationFieldDto(
    IFieldSymbol EnumSymbol,
    string DataStructureType,
    string DtoTypeName,
    string DtoNamespace,
    string EnumUnderlyingType,
    string EnumType,
    type? BaseTypeType = default
)
{
    private static readonly MD5 MD5 = MD5.Create();

    public const string CompilerGeneratedAttributes = Constants.CompilerGeneratedAttributes;
    public readonly string BaseType =>
        IsClass ? (BaseTypeType?.FullName ?? string.Empty) : string.Empty;
    public readonly string EnumTypeName => EnumType;
    public readonly string EnumerationName => DtoTypeName;
    public readonly string EnumNamespace => EnumSymbol.Type.ContainingNamespace.ToDisplayString();
    public readonly string? Value => EnumSymbol.ConstantValue?.ToString();
    public readonly bool IsRecord => DataStructureType.Contains(@record, OrdinalIgnoreCase);
    public readonly bool IsClass => DataStructureType.Contains(@class, OrdinalIgnoreCase);
    private readonly AttributeData? DisplayAttribute =>
        EnumSymbol
            .GetAttributes()
            .FirstOrDefault(a => a.AttributeClass.Name == nameof(DisplayAttribute));
    private readonly AttributeData? GuidAttribute =>
        EnumSymbol
            .GetAttributes()
            .FirstOrDefault(a => a.AttributeClass.Name == Constants.GuidAttribute);
    private readonly AttributeData? UrlAttribute =>
        EnumSymbol
            .GetAttributes()
            .FirstOrDefault(
                a => a.AttributeClass.Name is Constants.UrlAttribute or Constants.UriAttribute
            );
    private readonly AttributeData? SynonymsAttribute =>
        EnumSymbol
            .GetAttributes()
            .FirstOrDefault(a => a.AttributeClass.Name == Constants.SynonymsAttribute);
    public readonly string Id => EnumSymbol.ConstantValue?.ToString() ?? "0";
    public readonly string FieldName => EnumSymbol.Name;
    public readonly string DisplayName =>
        DisplayAttribute?.NamedArguments.FirstOrDefault(kvp => kvp.Key == Name).Value.Value
            is string displayNameString
            ? displayNameString
            : FieldName;
    public readonly string Description =>
        DisplayAttribute?.NamedArguments
            .FirstOrDefault(kvp => kvp.Key == Constants.Description)
            .Value.Value
            is string descriptionString
            ? descriptionString
            : FieldName;
    public readonly string ShortName =>
        DisplayAttribute?.NamedArguments
            .FirstOrDefault(kvp => kvp.Key == Constants.ShortName)
            .Value.Value
            is string shortNameString
            ? shortNameString
            : FieldName;
    public readonly string GroupName =>
        DisplayAttribute?.NamedArguments
            .FirstOrDefault(kvp => kvp.Key == Constants.GroupName)
            .Value.Value
            is string groupNameString
            ? groupNameString
            : DtoTypeName;
    public readonly int Order =>
        DisplayAttribute?.NamedArguments
            .FirstOrDefault(kvp => kvp.Key == Constants.Order)
            .Value.Value
            is int orderIntValue
            ? orderIntValue
            : 0;
    public readonly string UriString =>
        UrlAttribute?.ConstructorArguments.FirstOrDefault().Value?.ToString()
        ?? Format(
            UriPattern,
            Join(":", DtoNamespace.Split('.').Select(s => s.ToKebabCase())),
            DtoTypeName.ToKebabCase(),
            FieldName.ToKebabCase()
        );
    public readonly string GuidString =>
        GuidAttribute?.ConstructorArguments.FirstOrDefault().Value?.ToString()
        ?? ComputeMD5Hash(UriString);

    public readonly string Synonyms =>
        Join(
            ", ",
            (
                SynonymsAttribute?.ConstructorArguments.FirstOrDefault().Value as string[]
                ?? Empty<string>()
            )?.Select(s => $"\"{s}\"")
        );

    private static string ComputeMD5Hash(string s) =>
        MD5.ComputeHash(s.ToUTF8Bytes()).ToHexString();
}
