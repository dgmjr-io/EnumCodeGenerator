namespace Dgmjr.Enumerations;

using System.Collections.Concurrent;

using Dgmjr.Abstractions;

public static class UniversalUriResolver
{
    private static readonly IDictionary<uri, object> _cache = new ConcurrentDictionary<uri, object>();
    const string Parse = nameof(Parse);
    delegate object ParseDelegate(string s);

    public static TEnumeration ResolveUri<TEnumeration>(
        uri uri
    )
    where TEnumeration : IIdentifiable,
        IHaveADescription,
        IHaveAName,
        IHaveAShortName,
        IHaveAUri,
        IHaveAValue,
        IHaveADisplayName,
        IHaveAGuid
    => (TEnumeration)(_cache[uri] = _cache.TryGetValue(uri, out var value) ? (TEnumeration)value : CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Select(GetParseDelegate<TEnumeration>).WhereNotNull().Select(d => { try { return d(uri.ToString()); } catch { return null; } }).WhereNotNull().FirstOrDefault());

    static MethodInfo? GetParseMethod<TEnumeration>(
        this type type
    )
    where TEnumeration : IIdentifiable,
        IHaveADescription,
        IHaveAName,
        IHaveAShortName,
        IHaveAUri,
        IHaveAValue,
        IHaveADisplayName,
        IHaveAGuid =>
        type.GetRuntimeMethods().FirstOrDefault(m => m.Name == Parse && typeof(TEnumeration).IsAssignableFrom(m.ReturnType) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string));

    static ParseDelegate? GetParseDelegate<TEnumeration>(this type type)
    where TEnumeration : IIdentifiable,
        IHaveADescription,
        IHaveAName,
        IHaveAShortName,
        IHaveAUri,
        IHaveAValue,
        IHaveADisplayName,
        IHaveAGuid
        =>
        type.GetParseMethod<TEnumeration>().CreateDelegate(typeof(ParseDelegate)) as ParseDelegate;
}
