/*
 * Dtos.cs
 *
 *   Created: 2023-01-02-12:54:12
 *   Modified: 2023-01-02-12:54:12
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGeneration;

public record struct EnumerationTypeDeclaration
{
    public string GeneratedCodeAttribute => Constants.GeneratedCodeAttribute;
    public string EnumerationTypeName { get; set; }
    public string EnumTypeName { get; set; }
    public string EnumerationTypeType { get; set; }
    public string Namespace { get; set; }
    public string EnumNamespace { get; set; }
    public IDictionary<string, EnumerationMemberDeclaration> Members { get; set; }
    public string XmlDoc { get; set; }
    public IDictionary<string, type> Attributes { get; set; }
    public bool IsRecordType => EnumerationTypeType.StartsWith("record");
}

public record struct EnumerationMemberDeclaration
{
    public string Name { get; set; }
    public int Value { get; set; }
    public string XmlDoc { get; set; }
    public IDictionary<string, EnumerationAttributeDeclaration> Attributes { get; set; }
}

public record struct EnumerationAttributeDeclaration
{
    public string Name { get; set; }
    public type Type { get; set; }
    public object? Value { get; set; }
    public string XmlDoc { get; set; }
}
