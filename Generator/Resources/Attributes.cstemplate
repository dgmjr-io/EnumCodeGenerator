/*
 * Attributes.cs
 *
 *   Created: 2023-05-06-11:09:53
 *   Modified: 2023-05-06-11:09:53
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

#if !GENERATED_ENUMERATION_TYPE_ATTRIBUTES
#define GENERATED_ENUMERATION_TYPE_ATTRIBUTES

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System;

#nullable enable
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
internal abstract class GenerateEnumerationTypeAttribute : Attribute
{
    public GenerateEnumerationTypeAttribute(
        string? EnumerationTypeName = null,
        string? Namespace = null
    ) { }
}

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
internal abstract class GenerateEnumerationRecordTypeAttribute : GenerateEnumerationTypeAttribute
{
    public GenerateEnumerationRecordTypeAttribute(
        string? EnumerationTypeName = null,
        string? Namespace = null
    ) : base(EnumerationTypeName, Namespace) { }
}

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
internal sealed class GenerateEnumerationRecordClassAttribute
    : GenerateEnumerationRecordTypeAttribute
{
    public GenerateEnumerationRecordClassAttribute(
        string? EnumerationTypeName = null,
        string? Namespace = null
    ) : base(EnumerationTypeName, Namespace) { }
}

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
internal sealed class GenerateEnumerationRecordStructAttribute
    : GenerateEnumerationRecordTypeAttribute
{
    public GenerateEnumerationRecordStructAttribute(
        string? EnumerationTypeName = null,
        string? Namespace = null
    ) : base(EnumerationTypeName, Namespace) { }
}

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
internal sealed class GenerateEnumerationClassAttribute : GenerateEnumerationTypeAttribute
{
    public GenerateEnumerationClassAttribute(
        string? EnumerationTypeName = null,
        string? Namespace = null
    ) : base(EnumerationTypeName, Namespace) { }
}

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
internal sealed class GenerateEnumerationStructAttribute : GenerateEnumerationTypeAttribute
{
    public GenerateEnumerationStructAttribute(
        string? EnumerationTypeName = null,
        string? Namespace = null
    ) : base(EnumerationTypeName, Namespace) { }
}

#endif
