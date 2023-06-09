/*
 * Constants.cs
 *
 *   Created: 2023-01-11-03:08:07
 *   Modified: 2023-03-25-03:01:30
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

extern alias SUn;
using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security;
using static SUn::Scriban.Template;
using SUn::Scriban.Parsing;
using SUn::Scriban;

namespace Dgmjr.Enumerations.CodeGenerator;

internal static partial class Constants
{
    private const string ThisAssemblyTitle = ThisAssembly.Info.Title;
    private const string ThisAssemblyVersion = ThisAssembly.Info.FileVersion;
    private const string Header_cstemplate = "Header.cstemplate";

    private static readonly string Header = typeof(Constants).Assembly.ReadAssemblyResourceAllText(Header_cstemplate);

    // public static readonly Template HeaderTemplate = Parse(Header);

    public static string[] GenerateEnumerationTypeAttributes =>
        new[]
        {
            GenerateEnumerationRecordClassAttribute,
            GenerateEnumerationRecordStructAttribute,
            GenerateEnumerationClassAttribute,
            GenerateEnumerationStructAttribute,
            GenerateEnumerationRecordClassAttribute.Replace(nameof(Attribute), string.Empty),
            GenerateEnumerationRecordStructAttribute.Replace(nameof(Attribute), string.Empty),
            GenerateEnumerationClassAttribute.Replace(nameof(Attribute), string.Empty),
            GenerateEnumerationStructAttribute.Replace(nameof(Attribute), string.Empty)
        };

    public const string GenerateEnumerationRecordClassAttribute = nameof(
        GenerateEnumerationRecordClassAttribute
    );
    public const string GenerateEnumerationRecordStructAttribute = nameof(
        GenerateEnumerationRecordStructAttribute
    );
    public const string GenerateEnumerationClassAttribute = nameof(
        GenerateEnumerationClassAttribute
    );
    public const string GenerateEnumerationStructAttribute = nameof(
        GenerateEnumerationStructAttribute
    );

    public const string GenerateEnumerationTypesFileName = "GenerateEnumerationTypes.g.cs";

    public static string GeneratedCodeAttribute =>
        $"""global::System.CodeDom.Compiler.GeneratedCode("{ThisAssemblyTitle}", "{ThisAssemblyVersion}"), global::System.Runtime.CompilerServices.CompilerGenerated""";

    public static string GenerateEnumerationTypeAttributesDeclaration =>
        $$$"""""
        {{{Header}}}

        using System.CodeDom.Compiler;
        using System.Runtime.CompilerServices;

        #nullable enable
        [{{{GeneratedCodeAttribute}}}, AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
        internal abstract class GenerateEnumerationTypeAttribute : Attribute
        {
            public GenerateEnumerationTypeAttribute(string? EnumerationTypeName = null, string? Namespace =  null)
            {
            }
        }

        [{{{GeneratedCodeAttribute}}}, AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
        internal abstract class GenerateEnumerationRecordTypeAttribute : GenerateEnumerationTypeAttribute
        {
            public GenerateEnumerationRecordTypeAttribute(string? EnumerationTypeName = null, string? Namespace = null) : base(EnumerationTypeName, Namespace)
            {
            }
        }

        [{{{GeneratedCodeAttribute}}}, AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
        internal sealed class GenerateEnumerationRecordClassAttribute : GenerateEnumerationRecordTypeAttribute
        {
            public GenerateEnumerationRecordClassAttribute(string? EnumerationTypeName = null, string? Namespace = null) : base(EnumerationTypeName, Namespace)
            {
            }
        }

        [{{{GeneratedCodeAttribute}}}, AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
        internal sealed class GenerateEnumerationRecordStructAttribute : GenerateEnumerationRecordTypeAttribute
        {
            public GenerateEnumerationRecordStructAttribute(string? EnumerationTypeName = null, string? Namespace = null) : base(EnumerationTypeName, Namespace)
            {
            }
        }

        [{{{GeneratedCodeAttribute}}}, AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
        internal sealed class GenerateEnumerationClassAttribute : GenerateEnumerationTypeAttribute
        {
            public GenerateEnumerationClassAttribute(string? EnumerationTypeName = null, string? Namespace = null) : base(EnumerationTypeName, Namespace)
            {
            }
        }

        [{{{GeneratedCodeAttribute}}}, AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class)]
        internal sealed class GenerateEnumerationStructAttribute : GenerateEnumerationTypeAttribute
        {
            public GenerateEnumerationStructAttribute(string? EnumerationTypeName = null, string? Namespace = null) : base(EnumerationTypeName, Namespace)
            {
            }
        }
        """"";

    public const string IConvertible_cstemplate = "IConvertible.cstemplate";
    public const string EnumerationTypeDeclaration_cstemplate =
        "EnumerationTypeDeclaration.cstemplate";

    private static MemoryStream NotFoundStream => new("/* Not found */"u8.ToArray());

    public static readonly string IConvertibleImplementation = new StreamReader(
        typeof(Constants).Assembly.GetManifestResourceStream(IConvertible_cstemplate)
            ?? NotFoundStream
    ).ReadToEnd();

    public static readonly string EnumerationTypeDeclaration = new StreamReader(
        typeof(Constants).Assembly.GetManifestResourceStream(EnumerationTypeDeclaration_cstemplate)
            ?? NotFoundStream
    ).ReadToEnd();

    public const string IConvertibleImplementationFullPath = "IConvertible.cs";
    public static ParserOptions ParserOptions = new();
    public static LexerOptions LexerOptions = new();

    public static Template IConvertibleImplementationTemplate =>
        Parse(IConvertibleImplementation, IConvertible_cstemplate).CheckForErrors();

    // ThisAssembly.Resources.IConvertible.

    public const string EnumerationTypeDeclarationResourceName =
        "EnumerationTypeDeclaration.cstemplate";

    public static Template EnumerationTypeDeclarationTemplate =>
        Parse(EnumerationTypeDeclaration, EnumerationTypeDeclaration_cstemplate).CheckForErrors();

    public static Template CheckForErrors(this Template template)
    {
        if (template.HasErrors)
        {
            var errors = template.Messages.Where(msg => msg.Type == ParserMessageType.Error)
            .Select(msg =>
                    new ScriptRuntimeException(msg.Span, msg.Message)).ToArray();
            if (errors.Any())
            {
                throw new AggregateException("There was an error parsing the template", errors);
            }
        }
        return template;
    }

    public static class DisplayAttribute
    {
        public const string Name = nameof(Name);
        public const string ShortName = nameof(ShortName);
        public const string Description = nameof(Description);
        public const string GroupName = nameof(GroupName);
        public const string ResourceType = nameof(ResourceType);
        public const string Prompt = nameof(Prompt);
        public const string Order = nameof(Order);
        public const string AutoGenerateField = nameof(AutoGenerateField);
    }

    public class ScriptRuntimeException : Exception
    {
        public ScriptRuntimeException(SourceSpan span, string message) : base(message)
        {
            Span = span;
        }

        public SourceSpan Span { get; }
    }
}
