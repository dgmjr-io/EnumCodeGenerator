public sealed class GenerateEnumerationRecordStructAttribute : Attribute
{
    public GenerateEnumerationRecordStructAttribute(
        string? typeName = null,
        string? @namespace = null
    ) { }
}

public sealed class GenerateEnumerationStructAttribute : Attribute
{
    public GenerateEnumerationStructAttribute(string? typeName = null, string? @namespace = null)
    { }
}

public sealed class GenerateEnumerationRecordClassAttribute : Attribute
{
    public GenerateEnumerationRecordClassAttribute(
        string? typeName = null,
        string? @namespace = null
    ) { }
}

public sealed class GenerateEnumerationClassAttribute : Attribute
{
    public GenerateEnumerationClassAttribute(string? typeName = null, string? @namespace = null) { }
}
