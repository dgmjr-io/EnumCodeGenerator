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
        public const string Header = $$$"""
        /*
         * <auto-generated>
         *     {{ filename }}
         *     Created: {{ timestamp }}
         *
         *     This code was generated by {{{AssemblyName}}} version {{{AssemblyVersion}}}
         *
         *     Changes to this file may cause incorrect behavior and will be lost if
         *     the code is regenerated.
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

        public const string Enumeration = nameof(Enumeration);
        public const string NestedEnumerationType = nameof(NestedEnumerationType);
#pragma warning disable IDE1006
        public const string @record = nameof(@record);
        public const string @struct = nameof(@struct);
        public const string @class = nameof(@class);
        public const string record_struct = $"{@record} {@struct}";
        public const string record_class = $"{@record} {@class}";
        public const string scriban = nameof(scriban);
        public const string @global = nameof(@global);
        public const string global__ = $"{@global}::";
        public const string System = nameof(System);
#pragma warning restore

        public const string CompilerGeneratedAttributes =
            $"[System.Runtime.CompilerServices.CompilerGenerated, System.CodeDom.Compiler.GeneratedCode(\"{AssemblyName}\", \"{AssemblyVersion}\")]";

        public const string AttributeName = "Enumeration";
        public const string AttributeNamespace = "Dgmjr.Enumerations";
        public const string AttributeFullName = AttributeNamespace + "." + AttributeName;
        public const string GenerateEnumerationRecordStructAttribute =
            global__ + nameof(GenerateEnumerationRecordStructAttribute);
        public const string GenerateEnumerationRecordClassAttribute =
            global__ + nameof(GenerateEnumerationRecordClassAttribute);
        public const string GenerateEnumerationStructAttribute =
            global__ + nameof(GenerateEnumerationStructAttribute);
        public const string GenerateEnumerationClassAttribute =
            global__ + nameof(GenerateEnumerationClassAttribute);

        public static readonly string[] AttributeClasses = new[]
        {
            GenerateEnumerationClassAttribute,
            GenerateEnumerationRecordClassAttribute,
            GenerateEnumerationRecordStructAttribute,
            GenerateEnumerationStructAttribute
        };

        public const string GenerateEnumerationTypeAttributes = nameof(
            GenerateEnumerationTypeAttributes
        );

        public static readonly Template HeaderTemplate = Template.Parse(Header);

        public static string HeaderFilledIn(string filename) =>
            HeaderTemplate.Render(
                new
                {
                    filename,
                    toolName = AssemblyName,
                    toolVersion = AssemblyVersion,
                    timestamp = DateTimeOffset.Now,
                    CompilerGeneratedAttributes
                }
            );

        public static readonly string GenerateEnumerationTypeAttributeDeclarations =
            ParseCompilationUnit(
                    typeof(Constants).Assembly
                        .GetManifestResourceStream(
                            nameof(GenerateEnumerationTypeAttributeDeclarations) + ".cs"
                        )
                        .ReadToEnd()
                )
                .NormalizeWhitespace()
                .GetText()
                .ToString();

        public const string AssemblyName = ThisAssembly.Project.AssemblyName;
        public const string AssemblyVersion = ThisAssembly.Info.Version;
        public const string Value = nameof(Value);
        public const string DisplayName = nameof(DisplayName);
        public const string GroupName = nameof(GroupName);
        public const string ShortName = nameof(ShortName);
        public const string Name = nameof(Name);
        public const string Order = nameof(Order);
        public const string Id = nameof(Id);
        public const string Description = nameof(Description);
        public const string GuidAttribute = nameof(GuidAttribute);
        public const string UrlAttribute = nameof(UrlAttribute);
        public const string SynonymsAttribute = nameof(SynonymsAttribute);
        public const string DisplayAttribute = nameof(DisplayAttribute);
        public const string UriPattern = "urn:{0}:{1}:{2}";
        public const string DgmjrAbstractionsNamespace = "Dgmjr.Abstractions";

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
    }
}
