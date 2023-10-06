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

public record struct EnumerationDto(
    INamedTypeSymbol EnumType,
    string DtoTypeName,
    string DtoNamespace,
    type? baseType = default
)
{
    public readonly string? BaseType => baseType?.FullName;
    public const string CompilerGeneratedAttributes = Constants.CompilerGeneratedAttributes;
    public readonly string EnumTypeName => EnumType.MetadataName;
    public readonly string EnumNamespace => EnumType.ContainingNamespace.MetadataName;
    public readonly string EnumUnderlyingType => EnumType.EnumUnderlyingType.MetadataName;
    public readonly DateTimeOffset Timestamp = DateTimeOffset.Now;
    public string Author { get; set; } = "Unattributed";
    public string LicenseExpression { get; set; } = "Unlicense";
    public string LicenseUrl => $"https://opensource.org/license/{LicenseExpression}";
    public readonly string DataStructureType =>
        EnumType
            .GetAttributes()
            .Select(
                a =>
                    a.AttributeClass.Name == GenerateEnumerationRecordStructAttribute
                        ? record_struct
                        : a.AttributeClass.Name == GenerateEnumerationRecordClassAttribute
                            ? record_class
                            : a.AttributeClass.Name == GenerateEnumerationClassAttribute
                                ? @class
                                : a.AttributeClass.Name == GenerateEnumerationStructAttribute
                                    ? @struct
                                    : null
            )
            .WhereNotNull()
            .FirstOrDefault();

#pragma warning disable S2365
    public readonly EnumerationFieldDto[] Fields
    {
        get
        {
            var @this = this;
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
                            @this.EnumType.EnumUnderlyingType.MetadataName,
                            @this.EnumType.MetadataName,
                            @this.BaseType
                        )
                )
                .ToArray();
        }
    }
#pragma warning restore
}
