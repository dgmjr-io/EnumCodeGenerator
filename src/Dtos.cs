using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Dgmjr.Enumerations.CodeGenerator;
using Microsoft.Extensions.Options;
namespace Dgmjr.Enumerations.CodeGenerator;
using static Constants;

public record struct EnumerationDto(INamedTypeSymbol EnumType, string DtoTypeName, string DtoNamespace)
{
    public readonly string EnumNamespace => EnumType.ContainingNamespace.ToDisplayString();
    public readonly string EnumUnderlyingType => EnumType.EnumUnderlyingType.Name;
    public readonly string DataStructureType =>
        EnumType.GetAttributes().Select(a =>
            a.AttributeClass.Name == GenerateEnumerationRecordStructAttribute ? "record struct" :
            a.AttributeClass.Name == GenerateEnumerationRecordClassAttribute ? "record class" :
            a.AttributeClass.Name == GenerateEnumerationClassAttribute ? "class" :
            a.AttributeClass.Name == GenerateEnumerationStructAttribute ? "struct" :
            null)
            .WhereNotNull()
            .FirstOrDefault();
    public readonly EnumerationFieldDto[] Fields
    {
        get
        {
            var _this = this;
            return EnumType.GetMembers().OfType<IFieldSymbol>().Select(fs => new EnumerationFieldDto(fs, _this.DataStructureType, _this.DtoTypeName, _this.DtoNamespace)).ToArray();
        }
    }
}

public record struct EnumerationFieldDto(IFieldSymbol EnumSymbol, string DataStructureType, string DtoTypeName, string DtoNamespace)
{
    private static readonly MD5 MD5 = MD5.Create();
    private readonly AttributeData? DisplayAttribute => EnumSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == nameof(DisplayAttribute));
    private readonly AttributeData? GuidAttribute => EnumSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == Constants.GuidAttribute);
    private readonly AttributeData? UrlAttribute => EnumSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == Constants.UrlAttribute);
    public readonly string Id => EnumSymbol.ConstantValue?.ToString() ?? "0";
    public readonly string FieldName => EnumSymbol.Name;
    public readonly string DisplayName => DisplayAttribute?.NamedArguments.FirstOrDefault(kvp => kvp.Key == Name).Value.Value is string displayNameString ? displayNameString : FieldName;
    public readonly string Description => DisplayAttribute?.NamedArguments.FirstOrDefault(kvp => kvp.Key == Constants.Description).Value.Value is string descriptionString ? descriptionString : FieldName;
    public readonly string ShortName => DisplayAttribute?.NamedArguments.FirstOrDefault(kvp => kvp.Key == Constants.ShortName).Value.Value is string shortNameString ? shortNameString : FieldName;
    public readonly string GroupName => DisplayAttribute?.NamedArguments.FirstOrDefault(kvp => kvp.Key == Constants.GroupName).Value.Value is string groupNameString ? groupNameString : FieldName;
    public readonly int Order => DisplayAttribute?.NamedArguments.FirstOrDefault(kvp => kvp.Key == Constants.Order).Value.Value is int orderIntValue ? orderIntValue : 0;
    public readonly string UriString => UrlAttribute?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? Format(UriPattern, DtoNamespace.Replace(".", ":"), DtoTypeName, FieldName).ToKebabCase();
    public readonly string GuidString => GuidAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? MD5.ComputeHash(UriString.ToUTF8Bytes()).ToHexString();
}
