public sealed class GenerateEnumerationRecordStructAttribute(
    string? TypeName = null,
    string? Namespace = null
) : Attribute { }

public sealed class GenerateEnumerationStructAttribute(
    string? TypeName = null,
    string? Namespace = null
) : Attribute { }

public sealed class GenerateEnumerationRecordClassAttribute(
    string? TypeName = null,
    string? Namespace = null
) : Attribute { }

public sealed class GenerateEnumerationClassAttribute(
    string? TypeName = null,
    string? Namespace = null
) : Attribute { }
