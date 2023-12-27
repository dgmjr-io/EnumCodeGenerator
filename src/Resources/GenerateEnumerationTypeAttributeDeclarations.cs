using System;
using static System.AttributeTargets;

using ATargets = System.AttributeTargets;

[AttributeUsage(Class | Struct | ATargets.Enum)]
internal abstract class GenerateEnumerationTypeAttribute : Attribute
{
    protected GenerateEnumerationTypeAttribute(
        string? typeName = default,
        string? @namespace = default
    )
    {
        Namespace = @namespace;
        TypeName = typeName;
    }

    public string? Namespace { get; }
    public string? TypeName { get; }
}

[AttributeUsage(Class | Struct | ATargets.Enum)]
internal sealed class GenerateEnumerationRecordStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(Class | Struct | ATargets.Enum)]
internal sealed class GenerateEnumerationStructAttribute(
    string? typeName = default,
    string? @namespace = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace) { }

[AttributeUsage(Class | Struct | ATargets.Enum)]
internal sealed class GenerateEnumerationRecordClassAttribute(
    string? typeName = default,
    string? @namespace = default,
    Type? baseType = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace)
{
    public Type? BaseType { get; } = baseType;
}

[AttributeUsage(Class | Struct | ATargets.Enum)]
internal sealed class GenerateEnumerationClassAttribute(
    string? typeName = default,
    string? @namespace = default,
    Type? baseType = default
) : GenerateEnumerationTypeAttribute(typeName, @namespace)
{
    public Type? BaseType { get; } = baseType;
}
