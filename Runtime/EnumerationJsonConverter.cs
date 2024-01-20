namespace Dgmjr.Enumerations;

using Dgmjr.Abstractions;

public class EnumerationJsonConverter<TEnumeration> : JsonConverter<TEnumeration>
    where TEnumeration : IIdentifiable,
        IHaveADescription,
        IHaveAName,
        IHaveAShortName,
        IHaveAUri,
        IHaveAValue,
        IHaveADisplayName,
        IHaveAGuid
{
    public override TEnumeration Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        var dto = Deserialize<EnumerationSerializationDto<TEnumeration>>(reader.ValueSpan, options);
        return UniversalUriResolver.ResolveUri<TEnumeration>(dto.Uri);
    }

    public override void Write(Utf8JsonWriter writer, TEnumeration value, Jso options)
    {
        var dto = EnumerationSerializationDto<TEnumeration>.FromEnumeration(value);
        Serialize(writer, dto, options);
    }
}
