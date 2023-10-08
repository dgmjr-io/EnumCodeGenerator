using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal abstract class GenerateEnumerationTypeAttribute(
    string? typeName = default,
    string? @namespace = default
) : Attribute
{
    public string? Namespace { get; } = @namespace;
    public string? TypeName { get; } = typeName;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal sealed class GenerateEnumerationRecordStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal sealed class GenerateEnumerationStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal sealed class GenerateEnumerationRecordClassAttribute(
    string? typeName = default,
    string? @namespace = default,
    type? baseType = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace)
{
    public type? BaseType { get; } = baseType;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal sealed class GenerateEnumerationClassAttribute(
    string? typeName = default,
    string? @namespace = default,
    type? baseType = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace)
{
    public type? BaseType { get; } = baseType;
}
