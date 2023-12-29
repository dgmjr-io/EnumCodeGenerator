namespace Dgmjr.Enumerations;

public sealed class EnumerationJsonConverterAttribute : JConverterAttribute
{
    public EnumerationJsonConverterAttribute()
        : base(typeof(EnumerationJsonConverterFactory)) { }
}
