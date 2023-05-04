/*
 * EnumerationTypeGenerator.cs
 *
 *   Created: 2023-01-11-03:08:07
 *   Modified: 2023-03-25-02:56:18
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGenerator;

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Generator]
public class EnumerationTypeGenerator : IIncrementalGenerator
{
    private const string Id = nameof(Id);
    private const string Display = nameof(Display);
    private const string Value = nameof(Value);
    private const string UriAttribute = nameof(UriAttribute);
    private const string GuidKeyName = "Guid";
    private const string GuidAttribute = nameof(GuidAttribute);
    private const string Prompt = nameof(Prompt);
    private const string DefaultUrnPattern = "urn:{0}:{1}:{1}";

    private static bool Include(SyntaxNode node, CancellationToken cancellationToken) =>
        node is EnumDeclarationSyntax enumDeclaration
        && enumDeclaration.AttributeLists.Any(
            list =>
                list.Attributes.Any(
                    attribute =>
                        Constants.GenerateEnumerationTypeAttributes.Any(
                            attributeName => attribute.Name.ToString() == attributeName
                        )
                )
        );

    private static EnumerationTypeDeclaration? Transform(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken
    )
    {
        var attribute = context.Attributes.FirstOrDefault(
            attribute =>
                Constants.GenerateEnumerationTypeAttributes.Any(
                    attributeName => attribute.AttributeClass.Name == attributeName
                )
        );
        if (attribute is null)
        {
            return null;
        }

        var enumTypeSymbol = context.TargetSymbol as INamedTypeSymbol;

        var fallbackEnumerationTypeName = enumTypeSymbol.Name.Replace(nameof(Enum), string.Empty);

        var enumerationTypeName =
            attribute.ConstructorArguments.FirstOrDefault().Value ?? fallbackEnumerationTypeName;
        if (enumerationTypeName is not string enumerationTypeNameString)
        {
            return null;
        }

        var @namespace =
            attribute.ConstructorArguments.Skip(1).FirstOrDefault().Value
            ?? enumTypeSymbol.ContainingNamespace.ToDisplayString();
        if (@namespace is not string namespaceString)
        {
            return null;
        }

        var membersDictionary = new Dictionary<string, EnumerationMemberDeclaration>();
        foreach (var enumValue in enumTypeSymbol.GetMembers().OfType<IFieldSymbol>())
        {
            var attributesDictionary = new Dictionary<string, EnumerationAttributeDeclaration>
            {
                [Id] = new EnumerationAttributeDeclaration
                {
                    Name = Id,
                    Type = typeof(int),
                    Value = enumValue.ConstantValue
                },
                [nameof(DisplayAttribute.Name)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(DisplayAttribute.Name),
                    Type = typeof(string),
                    Value = enumValue.Name
                },
                [nameof(DisplayAttribute.Description)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(DisplayAttribute.Description),
                    Type = typeof(string),
                    Value = /*enumValue.GetDocumentationCommentXml().SelectXpath("//summary", false).FirstOrDefault()?.Value ?? */
                    enumValue.Name,
                    XmlDoc =
                        enumValue
                            .GetDocumentationCommentXml()
                            .SelectXpath("//summary", false)
                            .FirstOrDefault()
                            ?.Value
                            + enumValue
                                .GetDocumentationCommentXml()
                                .SelectXpath("//remarks", false)
                                .FirstOrDefault()
                                ?.Value
                            + enumValue
                                .GetDocumentationCommentXml()
                                .SelectXpath("//value", false)
                                .FirstOrDefault()
                                ?.Value
                        ?? enumValue.Name
                },
                [Display + nameof(DisplayAttribute.Name)] = new EnumerationAttributeDeclaration
                {
                    Name = Display + nameof(DisplayAttribute.Name),
                    Type = typeof(string),
                    Value = enumValue.Name
                },
                [nameof(DisplayAttribute.ShortName)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(DisplayAttribute.ShortName),
                    Type = typeof(string),
                    Value = enumValue.Name
                },
                [nameof(DisplayAttribute.GroupName)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(DisplayAttribute.GroupName),
                    Type = typeof(string),
                    Value = enumTypeSymbol.Name
                },
                [nameof(DisplayAttribute.Order)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(DisplayAttribute.Order),
                    Type = typeof(int),
                    Value = enumValue.ConstantValue
                },
                [nameof(Uri)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(Uri),
                    Type = typeof(string),
                    Value = null
                },
                [nameof(GuidKeyName)] = new EnumerationAttributeDeclaration
                {
                    Name = nameof(Guid),
                    Type = typeof(string),
                    Value = null
                }
            };

            foreach (var attributeData in enumValue.GetAttributes())
            {
                var displayAttribute =
                    attributeData.AttributeClass.Name == nameof(DisplayAttribute);
                if (displayAttribute)
                {
                    var name = attributeData.NamedArguments.FirstOrDefault(
                        namedArgument => namedArgument.Key == nameof(DisplayAttribute.Name)
                    );
                    var nameString = name.Value.Value as string;
                    attributesDictionary[nameof(DisplayAttribute.Name)] =
                        new EnumerationAttributeDeclaration
                        {
                            Name = nameof(DisplayAttribute.Name),
                            Type = typeof(string),
                            Value = nameString ?? enumValue.Name
                        };

                    var description = attributeData.NamedArguments.FirstOrDefault(
                        namedArgument => namedArgument.Key == nameof(DisplayAttribute.Description)
                    );
                    var descriptionString = name.Value.Value as string;
                    attributesDictionary[nameof(DisplayAttribute.Description)] =
                        new EnumerationAttributeDeclaration
                        {
                            Name = nameof(DisplayAttribute.Description),
                            Type = typeof(string),
                            Value =
                                descriptionString
                                ?? /*enumValue.GetDocumentationCommentXml().SelectXpath("//summary", false).FirstOrDefault()?.Value ??*/
                                name.Value.Value
                                ?? enumValue.Name
                        };

                    var order = attributeData.NamedArguments.FirstOrDefault(
                        namedArgument => namedArgument.Key == nameof(DisplayAttribute.Order)
                    );
                    var orderInt =
                        order.Value.Value as int? ?? enumValue.ConstantValue as int? ?? 0;
                    {
                        attributesDictionary[nameof(DisplayAttribute.Order)] =
                            new EnumerationAttributeDeclaration
                            {
                                Name = nameof(DisplayAttribute.Order),
                                Type = typeof(int),
                                Value = orderInt
                            };
                    }

                    var groupName = attributeData.NamedArguments.FirstOrDefault(
                        namedArgument => namedArgument.Key == nameof(DisplayAttribute.GroupName)
                    );
                    var groupNameString = groupName.Value.Value as string;
                    attributesDictionary[nameof(DisplayAttribute.GroupName)] =
                        new EnumerationAttributeDeclaration
                        {
                            Name = nameof(DisplayAttribute.GroupName),
                            Type = typeof(string),
                            Value = groupNameString ?? enumerationTypeName
                        };

                    var prompt = attributeData.NamedArguments.FirstOrDefault(
                        namedArgument => namedArgument.Key == nameof(DisplayAttribute.Prompt)
                    );
                    var promptString = prompt.Value.Value as string;
                    attributesDictionary[Prompt] = new EnumerationAttributeDeclaration
                    {
                        Name = Prompt,
                        Type = typeof(string),
                        Value = promptString ?? enumerationTypeName
                    };

                    var shortName = attributeData.NamedArguments.FirstOrDefault(
                        namedArgument => namedArgument.Key == nameof(DisplayAttribute.ShortName)
                    );
                    var shortNameString = shortName.Value.Value as string;
                    attributesDictionary[nameof(DisplayAttribute.ShortName)] =
                        new EnumerationAttributeDeclaration
                        {
                            Name = nameof(DisplayAttribute.ShortName),
                            Type = typeof(string),
                            Value = shortNameString ?? enumValue.Name
                        };
                }

                var uriAttribute = attributeData.AttributeClass.Name == nameof(UriAttribute);
                if (uriAttribute)
                {
                    var uri = attributeData.ConstructorArguments.FirstOrDefault().Value;
                    var uriString = uri as string;
                    attributesDictionary[nameof(Uri)] = new EnumerationAttributeDeclaration
                    {
                        Name = nameof(Uri),
                        Type = typeof(string),
                        Value =
                            uriString
                            ?? Format(
                                DefaultUrnPattern,
                                namespaceString.Replace(".", ":"),
                                enumerationTypeNameString.Replace(".", ":"),
                                enumValue.Name
                            )
                    };
                }

                var guidAttribute = attributeData.AttributeClass.Name == nameof(GuidAttribute);
                if (guidAttribute)
                {
                    var guid = attributeData.ConstructorArguments.FirstOrDefault().Value;
                    var guidString = guid as string;
                    attributesDictionary[nameof(Guid)] = new EnumerationAttributeDeclaration
                    {
                        Name = nameof(Guid),
                        Type = typeof(string),
                        Value = guidString ?? null
                    };
                }

                // attributesDictionary[Value] = new EnumerationAttributeDeclaration
                // {
                //     Name = Value,
                //     Type = new NamedEnumType(enumTypeSymbol.ContainingNamespace.ToDisplayString(), enumTypeSymbol.Name),
                //     Value = $"({enumTypeSymbol.ContainingNamespace.ToDisplayString()}.{enumTypeSymbol.Name}){enumValue.ConstantValue}"
                // };
            }

            membersDictionary.Add(
                enumValue.Name,
                new EnumerationMemberDeclaration
                {
                    Name = enumValue.Name,
                    Value = (int?)enumValue.ConstantValue ?? 0,
                    Attributes = attributesDictionary,
                    XmlDoc = $"""
                /// <summary>{(enumValue.GetDocumentationCommentXml().SelectXpath("//summary", false)?.FirstOrDefault()?.Value ?? attributesDictionary[nameof(DisplayAttribute.Name)].Value)?.ToString()?.Replace(Environment.NewLine, Environment.NewLine + "/// ")}</summary>
                /// <remarks>{(enumValue.GetDocumentationCommentXml().SelectXpath("//remarks", false)?.FirstOrDefault()?.Value ?? attributesDictionary[nameof(DisplayAttribute.Description)].Value)?.ToString()?.Replace(Environment.NewLine, Environment.NewLine + "/// ")}</remarks>
                /// <seealso cref="global::{enumTypeSymbol.ContainingNamespace.ToDisplayString()}.@{enumTypeSymbol.Name}.@{enumValue.Name}">{enumTypeSymbol.Name}</seealso>
                """
                }
            );
        }

        var xmlComment = "";
        try
        {
            var doc = System.Xml.Linq.XDocument.Parse(enumTypeSymbol.GetDocumentationCommentXml());
            xmlComment = Join(
                Environment.NewLine,
                doc.XPathSelectElements("//member/*").Select(xe => xe.ToString()).ToArray()
            );
        }
        catch (Exception)
        {
            // ignored
        }

        return new EnumerationTypeDeclaration
        {
            EnumerationTypeName = enumerationTypeNameString,
            EnumTypeName = enumTypeSymbol.Name,
            Namespace = namespaceString ?? enumTypeSymbol.ContainingNamespace.ToDisplayString(),
            EnumNamespace = enumTypeSymbol.ContainingNamespace.ToDisplayString(),
            Members = membersDictionary,
            EnumerationTypeType =
                attribute.AttributeClass.Name == Constants.GenerateEnumerationClassAttribute
                    ? "class"
                    : attribute.AttributeClass.Name == Constants.GenerateEnumerationStructAttribute
                        ? "struct"
                        : attribute.AttributeClass.Name
                        == Constants.GenerateEnumerationRecordClassAttribute
                            ? "record class"
                            : attribute.AttributeClass.Name
                            == Constants.GenerateEnumerationRecordStructAttribute
                                ? "record struct"
                                : "class",
            XmlDoc = $"""
                /**
                {xmlComment}
                */
                """,
            Attributes = membersDictionary
                .SelectMany(x => x.Value.Attributes.ToDictionary(a => a.Key, a => a.Value.Type))
                .Distinct()
                .ToDictionary(x => x.Key, x => x.Value)
        };
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        foreach (var attributeName in Constants.GenerateEnumerationTypeAttributes)
        {
            var syntaxProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(attributeName, Include, Transform)
                .Collect();
            context.RegisterSourceOutput(syntaxProvider, Generate);
        }
        context.RegisterPostInitializationOutput(
            (context) =>
                context.AddSource(
                    Constants.GenerateEnumerationTypesFileName,
                    Constants.GenerateEnumerationTypeAttributesDeclaration
                )
        );
    }

    private static void Generate(
        SourceProductionContext context,
        ImmutableArray<EnumerationTypeDeclaration?> values
    )
    {
        foreach (var enumerationType in values)
        {
            if (enumerationType is null)
            {
                continue;
            }

            var template = Constants.EnumerationTypeDeclarationTemplate;
            var result = template.Render(enumerationType);
            context.AddSource($"{enumerationType.Value.EnumerationTypeName}.g.cs", result);
        }
    }

    public class NamedEnumType : type
    {
        public NamedEnumType(string @namespace, string name)
        {
            _namespace = @namespace;
            _mame = name;
        }

        public override Assembly Assembly => typeof(NamedEnumType).Assembly;

        public override string AssemblyQualifiedName => FullName + ", " + Assembly.FullName;

        public override type BaseType => typeof(Enum);

        public override string FullName => throw new NotImplementedException();

        public override Guid GUID => throw new NotImplementedException();

        public override Module Module => throw new NotImplementedException();

        private string _namespace = "";
        public override string Namespace => _namespace;

        public override type UnderlyingSystemType => typeof(Enum).UnderlyingSystemType;

        private string _mame = "";
        public override string Name => _mame;

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override object[] GetCustomAttributes(bool inherit) =>
            throw new NotImplementedException();

        public override object[] GetCustomAttributes(type attributeType, bool inherit) =>
            throw new NotImplementedException();

        public override type GetElementType() => throw new NotImplementedException();

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override EventInfo[] GetEvents(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override FieldInfo GetField(string name, BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override FieldInfo[] GetFields(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override type GetInterface(string name, bool ignoreCase) =>
            throw new NotImplementedException();

        public override type[] GetInterfaces() => throw new NotImplementedException();

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override type GetNestedType(string name, BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override type[] GetNestedTypes(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) =>
            throw new NotImplementedException();

        public override object InvokeMember(
            string name,
            BindingFlags invokeAttr,
            Binder binder,
            object target,
            object[] args,
            ParameterModifier[] modifiers,
            CultureInfo culture,
            string[] namedParameters
        ) => throw new NotImplementedException();

        public override bool IsDefined(type attributeType, bool inherit) =>
            throw new NotImplementedException();

        protected override TypeAttributes GetAttributeFlagsImpl() =>
            throw new NotImplementedException();

        protected override ConstructorInfo GetConstructorImpl(
            BindingFlags bindingAttr,
            Binder binder,
            CallingConventions callConvention,
            type[] types,
            ParameterModifier[] modifiers
        ) => throw new NotImplementedException();

        protected override MethodInfo GetMethodImpl(
            string name,
            BindingFlags bindingAttr,
            Binder binder,
            CallingConventions callConvention,
            type[] types,
            ParameterModifier[] modifiers
        ) => throw new NotImplementedException();

        protected override PropertyInfo GetPropertyImpl(
            string name,
            BindingFlags bindingAttr,
            Binder binder,
            type returnType,
            type[] types,
            ParameterModifier[] modifiers
        ) => throw new NotImplementedException();

        protected override bool HasElementTypeImpl() => throw new NotImplementedException();

        protected override bool IsArrayImpl() => throw new NotImplementedException();

        protected override bool IsByRefImpl() => throw new NotImplementedException();

        protected override bool IsCOMObjectImpl() => throw new NotImplementedException();

        protected override bool IsPointerImpl() => throw new NotImplementedException();

        protected override bool IsPrimitiveImpl() => throw new NotImplementedException();
    }
}
