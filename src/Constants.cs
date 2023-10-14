/*
 * Constants.cs
 *
 *   Created: 2023-10-10-09:00:48
 *   Modified: 2023-10-12-08:28:26
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

extern alias Scrib;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Template = Scrib::Scriban.Template;

namespace Dgmjr.Enumerations.CodeGenerator
{
    public static class Constants
    {
        // Public const fields
        public const string _cs = ".cs";
        public const string @class = nameof(@class);
        public const string @global = nameof(@global);
        public const string @record = nameof(@record);
        public const string @struct = nameof(@struct);
        public const string global__ = $"{@global}::";
        public const string record_class = $"{@record} {@class}";
        public const string record_struct = $"{@record} {@struct}";
        public const string scriban = nameof(scriban);

        public const string AssemblyName = ThisAssembly.Project.AssemblyName;
        public const string AssemblyVersion = ThisAssembly.Info.Version;
        public const string AttributeFullName = AttributeNamespace + "." + AttributeName;
        public const string AttributeName = "Enumeration";
        public const string AttributeNamespace = "Dgmjr.Enumerations";
        public const string CompilerGeneratedAttributes =
            $"[System.Runtime.CompilerServices.CompilerGenerated, System.CodeDom.Compiler.GeneratedCode(\"{AssemblyName}\", \"{AssemblyVersion}\")]";
        public const string Description = nameof(Description);
        public const string DgmjrAbstractionsNamespace = "Dgmjr.Abstractions";
        public const string DisplayAttribute = nameof(DisplayAttribute);
        public const string DisplayName = nameof(DisplayName);
        public const string Enumeration = nameof(Enumeration);
        public const string GenerateEnumerationClassAttribute = nameof(
            GenerateEnumerationClassAttribute
        );
        public const string GenerateEnumerationRecordClassAttribute = nameof(
            GenerateEnumerationRecordClassAttribute
        );
        public const string GenerateEnumerationRecordStructAttribute = nameof(
            GenerateEnumerationRecordStructAttribute
        );
        public const string GenerateEnumerationStructAttribute = nameof(
            GenerateEnumerationStructAttribute
        );
        public const string GenerateEnumerationTypeAttributes = nameof(
            GenerateEnumerationTypeAttributes
        );
        public const string GroupName = nameof(GroupName);
        public const string GuidAttribute = nameof(GuidAttribute);
        public const string Id = nameof(Id);
        public const string Name = nameof(Name);
        public const string NestedEnumerationType = nameof(NestedEnumerationType);
        public const string Order = nameof(Order);
        public const string ShortName = nameof(ShortName);
        public const string SynonymsAttribute = nameof(SynonymsAttribute);
        public const string System = nameof(System);
        public const string UriAttribute = nameof(UriAttribute);
        public const string UriPattern = "urn:{0}:{1}:{2}";
        public const string UrlAttribute = nameof(UrlAttribute);
        public const string Value = nameof(Value);

        // Public static readonly fields
        public static readonly string[] AttributeClasses =
        {
            GenerateEnumerationClassAttribute,
            GenerateEnumerationRecordClassAttribute,
            GenerateEnumerationRecordStructAttribute,
            GenerateEnumerationStructAttribute
        };

        public static readonly Template HeaderTemplate = Template.Parse(Header);

        public static readonly string GenerateEnumerationTypeAttributeDeclarations =
            ParseCompilationUnit(
                    typeof(Constants).Assembly
                        .GetManifestResourceStream(
                            nameof(GenerateEnumerationTypeAttributeDeclarations) + _cs
                        )
                        .ReadToEnd()
                )
                .NormalizeWhitespace()
                .GetText()
                .ToString();

        public static readonly string EnumerationDeclaration =
            typeof(Constants).Assembly.ReadAssemblyResourceAllText($"{Enumeration}.{scriban}");

        public static readonly string IEnumerationDeclaration =
            typeof(Constants).Assembly.ReadAssemblyResourceAllText($"I{Enumeration}.{scriban}");

        public static readonly string NestedEnumerationTypeDeclaration =
            typeof(Constants).Assembly.ReadAssemblyResourceAllText(
                $"{NestedEnumerationType}.{scriban}"
            );

        public static readonly Template EnumerationDeclarationTemplate = Template.Parse(
            EnumerationDeclaration
        );

        public static readonly Template IEnumerationDeclarationTemplate = Template.Parse(
            IEnumerationDeclaration
        );

        public static readonly Template NestedEnumerationTypeDeclarationTemplate = Template.Parse(
            NestedEnumerationTypeDeclaration
        );

        // Header constant
        public const string Header = $$$"""
        /*
         * <auto-generated>
         *     This code was auto-generated by {{{AssemblyName}}}
         *     on {{ timestamp }}.
         *     Changes to this file may cause incorrect
         *     behavior and will be lost if the code is regenerated.
         * </auto-generated>
         */

        using static System.AttributeTargets;
        using static System.Text.RegularExpressions.RegexOptions;
        using System;
        using System.CodeDom.Compiler;
        using System.Collections.Generic;
        using System.Diagnostics.CodeAnalysis;
        using System.Globalization;
        using System.Linq;
        using System.Runtime.CompilerServices;
        using System.Text;
        using System.Text.RegularExpressions;
        #if NET7_0_OR_GREATER
        using StringSyntax = System.Diagnostics.CodeAnalysis.StringSyntaxAttribute;
        #endif
        #nullable enable
        """;

        public static string HeaderFilledIn(string filename) =>
            HeaderTemplate.Render(
                new
                {
                    Filename = filename,
                    Timestamp = DateTimeOffset.Now.ToString("yyyy-mm-ddTHH:mm:ss.ffffzzzZ")
                }
            );
    }
}
