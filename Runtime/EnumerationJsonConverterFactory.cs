/*
 * EnumerationJsonConverterFactory.cs
 *
 *   Created: 2023-47-28T19:47:35-05:00
 *   Modified: 2024-46-19T07:46:10-05:00
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2024 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations;

using Dgmjr.Abstractions;

public class EnumerationJsonConverterFactory : JsonConverterFactory
{
    private static readonly type[] Interfaces =
    [
        typeof(IIdentifiable),
        typeof(IHaveAGuid),
        typeof(IHaveAUri),
        typeof(IHaveAValue),
        typeof(IHaveAName),
        typeof(IHaveAShortName),
        typeof(IHaveADisplayName),
        typeof(IHaveADescription)
    ];

    public override bool CanConvert(type typeToConvert) =>
        TrueForAll(Interfaces, i => i.IsAssignableFrom(typeToConvert));

    public override JConverter? CreateConverter(type typeToConvert, Jso options)
    {
        var converterType = typeof(EnumerationJsonConverter<>).MakeGenericType(typeToConvert);
        return (JConverter?)Activator.CreateInstance(converterType);
    }
}
