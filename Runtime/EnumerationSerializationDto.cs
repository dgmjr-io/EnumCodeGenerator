namespace Dgmjr.Enumerations;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Dgmjr.Abstractions;

public record class EnumerationSerializationDto(uri Uri,
    string Description,
    IDictionary<string, object> Ids,
    IStringDictionary Names
)
{
    protected const string Id = "id";
    protected const string Guid = "guid";
    protected const string Name = "name";
    protected const string Short = "short";
    protected const string Display = "display";
    public static readonly EnumerationSerializationDto Empty =
        new(
            uri.Empty,
            string.Empty,
            ImmutableDictionary<string, object>.Empty,
            ImmutableDictionary<string, string>.Empty
        );

    public static EnumerationSerializationDto FromEnumeration(
        object e
    ) =>
        new(
            (e as IHaveAUri).Uri.ToString(),
            (e as IHaveADescription).Description,
            new Dictionary<string, object>() { { Id, (e as IIdentifiable).Id }, { Guid, (e as IHaveAGuid).Guid } },
            new StringDictionary()
            {
                { Name, (e as IHaveAName).Name },
                { Short, (e as IHaveAShortName).ShortName },
                { Display, (e as IHaveADisplayName).DisplayName }
            }
        );

    public uri Uri { get; init; } = Uri;
    public string Description { get; init; } = Description;
    public IDictionary<string, object> Ids { get; init; } =
        new Dictionary<string, object>(Ids, StringComparer.OrdinalIgnoreCase);
    public IStringDictionary Names { get; init; } =
        new StringDictionary(Names, StringComparer.OrdinalIgnoreCase);
}

public record class EnumerationSerializationDto<TEnumeration>(
    uri Uri,
    string Description,
    IDictionary<string, object> Ids,
    IStringDictionary Names
) : EnumerationSerializationDto(Uri, Description, Ids, Names)
    where TEnumeration : IIdentifiable,
        IHaveADescription,
        IHaveAName,
        IHaveAShortName,
        IHaveAUri,
        IHaveAValue,
        IHaveADisplayName,
        IHaveAGuid
{
    public static EnumerationSerializationDto<TEnumeration> FromEnumeration(
        TEnumeration e
    ) =>
        new(
            e.Uri.ToString(),
            e.Description,
            new Dictionary<string, object>() { { Id, e.Id }, { Guid, e.Guid } },
            new StringDictionary()
            {
                { Name, e.Name },
                { Short, e.ShortName },
                { Display, e.DisplayName }
            }
        );
}
