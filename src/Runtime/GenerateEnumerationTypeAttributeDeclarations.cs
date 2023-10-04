namespace System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public abstract class GenerateEnumerationTypeAttribute(
    string? typeName = default,
    string? @namespace = default
) : Attribute
{
    public string? Namespace { get; } = @namespace;
    public string? TypeName { get; } = typeName;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public sealed class GenerateEnumerationRecordStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public sealed class GenerateEnumerationStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public sealed class GenerateEnumerationRecordClassAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public sealed class GenerateEnumerationClassAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }
