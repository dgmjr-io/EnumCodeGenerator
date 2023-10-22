/*
 * EnumerationDto.cs
 *
 *   Created: 2023-10-10-09:00:48
 *   Modified: 2023-10-12-08:28:31
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.IO;

namespace Dgmjr.Enumerations.CodeGenerator;

using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using static Constants;

using Dgmjr.Enumerations.CodeGenerator;

internal record struct EnumerationDto(
    INamedTypeSymbol EnumType,
    string DtoTypeName,
    string DtoNamespace,
    type? BaseTypeType = default
)
{
    public readonly string? BaseType =>
        DataStructureType.Contains(@struct) && (BaseTypeType?.IsClass ?? false)
            ? throw new InvalidDataException(
                "The enumeration data structure must be a class to support inheritance."
            )
            : BaseTypeType?.FullName;
    public readonly string CompilerGeneratedAttributes => Constants.CompilerGeneratedAttributes;
    public readonly string EnumTypeName => EnumType.MetadataName;
    public readonly string EnumNamespace => EnumType.ContainingNamespace.MetadataName;
    public readonly string EnumUnderlyingType => EnumType.EnumUnderlyingType?.MetadataName ?? "int";
    public readonly string FieldsInstances
    {
        get
        {
            var @this = this;
            return Join(
                ", ",
                @this.Fields.Select(f => $"{@this.DtoTypeName}.{f.FieldName}.Instance")
            );
        }
    }
    public readonly string FieldsValues
    {
        get
        {
            var @this = this;
            return Join(", ", @this.Fields.Select(f => $"{@this.DtoTypeName}.{f.FieldName}.Value"));
        }
    }
    public readonly DateTimeOffset Timestamp = DateTimeOffset.Now;
    public readonly string DataStructureType =>
        EnumType
            .GetAttributes()
            .Select(
                a =>
                    a.AttributeClass?.Name == GenerateEnumerationRecordStructAttribute
                        ? record_struct
                        : a.AttributeClass?.Name == GenerateEnumerationRecordClassAttribute
                            ? record_class
                            : a.AttributeClass?.Name == GenerateEnumerationClassAttribute
                                ? @class
                                : a.AttributeClass?.Name == GenerateEnumerationStructAttribute
                                    ? @struct
                                    : null
            )
            .WhereNotNull()
            .FirstOrDefault();

    public readonly EnumerationFieldDto[] Fields
    {
        get
        {
            var @this = this;
#pragma warning disable S2365
            return EnumType
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Select(
                    fs =>
                        new EnumerationFieldDto(
                            fs,
                            @this.DataStructureType,
                            @this.DtoTypeName,
                            @this.DtoNamespace,
                            @this.EnumType?.EnumUnderlyingType?.MetadataName ?? "int",
                            @this.EnumType?.MetadataName
                        )
                )
                .ToArray();
#pragma warning restore S2365
        }
    }
}
