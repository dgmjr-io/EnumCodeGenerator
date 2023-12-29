using System;

using Targets = System.AttributeTargets;
using UsageAttribute = System.AttributeUsageAttribute;

[Usage(Targets.Class | Targets.Struct | Targets.Enum)]
internal abstract class GenerateEnumerationTypeAttribute(
    string? typeName = default,
    string? @namespace = default
) : Attribute
{
    public string? Namespace { get; } = @namespace;
    public string? TypeName { get; } = @typeName;
}

[Usage(Targets.Class | Targets.Struct | Targets.Enum)]
internal sealed class GenerateEnumerationRecordStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[Usage(Targets.Class | Targets.Struct | Targets.Enum)]
internal sealed class GenerateEnumerationStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[Usage(Targets.Class | Targets.Struct | Targets.Enum)]
internal sealed class GenerateEnumerationRecordClassAttribute(
    string? typeName = default,
    string? @namespace = default,
    Type? baseType = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace)
{
    public Type? BaseType { get; } = baseType;
}

[Usage(Targets.Class | Targets.Struct | Targets.Enum)]
internal sealed class GenerateEnumerationClassAttribute(
    string? typeName = default,
    string? @namespace = default,
    Type? baseType = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace)
{
    public Type? BaseType { get; } = baseType;
}
