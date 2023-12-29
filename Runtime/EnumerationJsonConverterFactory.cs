namespace Dgmjr.Enumerations;

using Dgmjr.Abstractions;

public class EnumerationJsonConverterFactory : JsonConverterFactory
{
    private static readonly type[] Interfaces = [typeof(IIdentifiable), typeof(IHaveAGuid), typeof(IHaveAUri), typeof(IHaveAValue), typeof(IHaveAName), typeof(IHaveAShortName), typeof(IHaveADisplayName), typeof(IHaveADescription)];

    public override bool CanConvert(type typeToConvert) =>
        TrueForAll(Interfaces, i => i.IsAssignableFrom(typeToConvert));

    public override JConverter? CreateConverter(
        type typeToConvert,
        Jso options
    )
    {
        var converterType = typeof(EnumerationJsonConverter<>).MakeGenericType(typeToConvert);
        return (JConverter?)Activator.CreateInstance(converterType);
    }
}
