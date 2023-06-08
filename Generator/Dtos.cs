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

namespace Dgmjr.Enumerations.CodeGenerator
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;


    /* This code defines a C# record struct called `EnumerationTypeDeclaration` that represents a
    declaration of an enumeration type. It has several properties, including `Symbol` which represents
    the named type symbol of the enumeration, `EnumerationTypeName` which represents the name of the
    enumeration type, `Members` which is a dictionary of the enumeration members, and `XmlDoc` which
    represents the XML documentation for the enumeration type. It also has some methods, such as
    `IsRecordType` which returns true if the enumeration type is a record type, and
    `OverridableMemberModifiers` which returns the appropriate modifiers for overridable members
    depending on whether the enumeration type is a value type or not. */
    public struct EnumerationTypeDeclaration
    {
        public INamedTypeSymbol Symbol { get; set; }
        public string GeneratedCodeAttribute => Constants.GeneratedCodeAttribute;
        public string EnumerationTypeName { get; set; }
        public string EnumTypeName { get; set; }
        public string EnumerationTypeType { get; set; }
        public string Namespace { get; set; }
        public string EnumNamespace { get; set; }
        public IDictionary<string, EnumerationMemberDeclaration> Members { get; set; }
        public string XmlDoc { get; set; }
        public IDictionary<string, type> Attributes { get; set; }
        public bool IsRecordType => EnumerationTypeType.Contains("record");
        public bool IsClassType => EnumerationTypeType.Contains("class");
        private bool IsValueType => EnumerationTypeType.Contains("struct");
        public string OverridableMemberModifiers => IsValueType ? "" : " virtual ";
        public SyntaxTree? OtherStuff { get; set; }
        public string? OtherStuffString =>
            OtherStuff?.GetCompilationUnitRoot().NormalizeWhitespace().ToString();
    }

    public struct EnumerationMemberDeclaration
    {
        public ISymbol Symbol { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string XmlDoc { get; set; }
        public IDictionary<string, EnumerationAttributeDeclaration> Attributes { get; set; }
        private bool IsOverridable => Symbol.ContainingType.TypeKind == TypeKind.Class;
        private bool ShouldBeOverridden =>
            IsOverridable && (Symbol is IMethodSymbol methodSymbol) && methodSymbol.IsVirtual
            || (Symbol is IPropertySymbol propertySymbol) && propertySymbol.IsVirtual;

        public string OverridableMemberModifiers =>
            IsOverridable && ShouldBeOverridden ? " override " : "";
    }

    public struct EnumerationAttributeDeclaration
    {
        public string Name { get; set; }
        public type Type { get; set; }
        public object? Value { get; set; }
        public string XmlDoc { get; set; }
    }
}
