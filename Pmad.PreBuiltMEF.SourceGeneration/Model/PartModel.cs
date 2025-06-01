using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class PartModel
    {
        public PartModel(string type, Dictionary<string, string> metadata, List<PartConstructorParameter>? importingConstructorParameters, List<PartExport> partExports, List<MemberImport> propertyImports, List<MemberExport> propertyExports)
        {
            Type = type;
            Metadata = metadata;
            ImportingConstructorParameters = importingConstructorParameters;
            PartExports = partExports;
            MemberImports = propertyImports;
            MemberExports = propertyExports;
        }

        public string Type { get; }

        public Dictionary<string, string> Metadata { get; }

        public bool HasImportingConstructor => ImportingConstructorParameters != null;

        public List<PartConstructorParameter>? ImportingConstructorParameters { get; }

        public List<PartExport> PartExports { get; }

        public List<MemberImport> MemberImports { get; }

        public List<MemberExport> MemberExports { get; }

        public static bool IsTargeted(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax classDeclaration)
            {
                // Exclude static and abstract classes
                return !classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword) || m.IsKind(SyntaxKind.AbstractKeyword));
            }
            return false;
        }

        public static PartModel? Create(INamedTypeSymbol symbol)
        {
            var importingConstructorArguments = GetImportingConstructorParameters(symbol);
            var exports = GetPartExports(symbol);

            var propexports = new List<MemberExport>();
            var propimports = new List<MemberImport>();
            GetProperties(symbol, propexports, propimports);
            GetFields(symbol, propexports, propimports);

            if (importingConstructorArguments != null || exports.Count > 0 || propexports.Count > 0 || propimports.Count > 0)
            {
                var metadata = GetMetadata(symbol);
                return new PartModel(symbol.ToDisplayString(), metadata, importingConstructorArguments, exports, propimports, propexports);
            }

            return null;
        }

        private static void GetProperties(INamedTypeSymbol symbol, List<MemberExport> propexports, List<MemberImport> propimports)
        {
            foreach (var prop in symbol.GetMembers().OfType<IPropertySymbol>().Where(p => !p.IsStatic))
            {
                foreach (var exportAttr in prop.GetAttributes().Where(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ExportAttribute"))
                {
                    propexports.Add(new MemberExport(prop.Name, ContractReference.Get(prop.Type, exportAttr), GetExportMetadata(prop)));
                }

                foreach (var importAttr in prop.GetAttributes().Where(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportAttribute"))
                {
                    var mode = GetImportMode(prop.Type, out var type, out var metadata);
                    propimports.Add(new MemberImport(prop.Name, ContractReference.Get(type, importAttr), allowDefault: IsAllowDefault(importAttr), allowRecomposition: IsAllowRecomposition(importAttr), mode, metadata));
                }

                foreach (var importManyAttr in prop.GetAttributes().Where(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportManyAttribute"))
                {
                    var mode = GetImportManyMode(prop.Type, out var type, out var metadata);
                    if (mode >= ImportMode.Many)
                    {
                        propimports.Add(new MemberImport(prop.Name, ContractReference.Get(type, importManyAttr), allowDefault: false, allowRecomposition: IsAllowRecomposition(importManyAttr), mode, metadata));
                    }
                }
            }
        }

        public static ImportMode GetImportMode(ITypeSymbol propertyType, out ITypeSymbol type, out ITypeSymbol? metadata)
        {
            if (propertyType is INamedTypeSymbol namedType)
            {
                if (namedType.ConstructedFrom?.ToDisplayString() == "System.Lazy<T>" 
                    && namedType.TypeArguments.Length == 1)
                {
                    type = namedType.TypeArguments[0];
                     metadata = null;
                    return ImportMode.Lazy;
                }
                if (namedType.ConstructedFrom?.ToDisplayString() == "System.Lazy<T, TMetadata>"
                    && namedType.TypeArguments.Length == 2)
                {
                    type = namedType.TypeArguments[0];
                    metadata = namedType.TypeArguments[1];
                    return ImportMode.Lazy;
                }
            }
            metadata = null;
            type = propertyType;
            return ImportMode.Normal;
        }

        public static ImportMode GetImportManyMode(ITypeSymbol propertyType, out ITypeSymbol type, out ITypeSymbol? metadata)
        {
            if (propertyType is INamedTypeSymbol namedType)
            {
                if (namedType.TypeKind == TypeKind.Array 
                    && namedType.TypeArguments.Length == 1)
                {
                    var itemMode = GetImportMode(namedType.TypeArguments[0], out type, out metadata);
                    if (itemMode == ImportMode.Lazy)
                    {
                        return ImportMode.ManyLazy;
                    }
                    return ImportMode.Many;
                }
                if (namedType.ConstructedFrom?.ToDisplayString() == "System.Collections.Generic.IEnumerable<T>"
                    && namedType.TypeArguments.Length == 1)
                {
                    var itemMode = GetImportMode(namedType.TypeArguments[0], out type, out metadata);
                    if (itemMode == ImportMode.Lazy)
                    {
                        return ImportMode.ManyLazy;
                    }
                    return ImportMode.Many;
                }
            }
            metadata = null;
            type = propertyType;
            return ImportMode.Normal;
        }

        private static bool IsAllowDefault(AttributeData? attr)
        {
            if (attr == null)
            {
                return false;
            }
            foreach (var namedArg in attr.NamedArguments)
            {
                if (namedArg.Key == "AllowDefault" && namedArg.Value.Value is bool allowDefault)
                {
                    return allowDefault;
                }
            }
            return false;
        }

        private static void GetFields(INamedTypeSymbol symbol, List<MemberExport> propexports, List<MemberImport> propimports)
        {
            foreach (var prop in symbol.GetMembers().OfType<IFieldSymbol>().Where(p => !p.IsStatic))
            {
                foreach (var exportAttr in prop.GetAttributes().Where(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ExportAttribute"))
                {
                    propexports.Add(new MemberExport(prop.Name, ContractReference.Get(prop.Type, exportAttr), GetExportMetadata(prop)));
                }

                foreach (var importAttr in prop.GetAttributes().Where(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportAttribute"))
                {
                    var mode = GetImportMode(prop.Type, out var type, out var metadata);
                    propimports.Add(new MemberImport(prop.Name, ContractReference.Get(type, importAttr), allowDefault: IsAllowDefault(importAttr), allowRecomposition: IsAllowRecomposition(importAttr), mode, metadata));
                }

                foreach (var importManyAttr in prop.GetAttributes().Where(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportManyAttribute"))
                {
                    var mode = GetImportManyMode(prop.Type, out var type, out var metadata);
                    if (mode >= ImportMode.Many)
                    {
                        propimports.Add(new MemberImport(prop.Name, ContractReference.Get(type, importManyAttr), allowDefault: false, allowRecomposition: IsAllowRecomposition(importManyAttr), mode, metadata));
                    }
                }
            }
        }

        private static bool IsAllowRecomposition(AttributeData? attr)
        {
            if (attr == null)
            {
                return false;
            }
            foreach (var namedArg in attr.NamedArguments)
            {
                if (namedArg.Key == "AllowRecomposition" && namedArg.Value.Value is bool allowDefault)
                {
                    return allowDefault;
                }
            }
            return false;
        }

        private static Dictionary<string, string> GetMetadata(INamedTypeSymbol symbol)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            foreach (var partMetadataAttribute in symbol.GetAttributes().Where(attr =>
                                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.PartMetadataAttribute"))
            {
                if (partMetadataAttribute.ConstructorArguments.Length == 2)
                {
                    metadata.Add(partMetadataAttribute.ConstructorArguments[0].ToCSharpString(), partMetadataAttribute.ConstructorArguments[1].ToCSharpString());
                }
            }

            return metadata;
        }
        private static Dictionary<string, string> GetExportMetadata(ISymbol symbol)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            foreach (var partMetadataAttribute in symbol.GetAttributes().Where(attr =>
                                    attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ExportMetadataAttribute"))
            {
                if (partMetadataAttribute.ConstructorArguments.Length == 2)
                {
                    metadata.Add(partMetadataAttribute.ConstructorArguments[0].ToCSharpString(), partMetadataAttribute.ConstructorArguments[1].ToCSharpString());
                }
            }

            //foreach (var exportMetadataAttribute in symbol.GetAttributes().Where(attr =>
            //                        attr.AttributeClass?.GetAttributes()
            //                        .Any(c => c.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.MetadataAttributeAttribute") ?? false))
            //{
            //    // ISSUE: some attributs may have custom logic to extract metadata

            //    // Simple case: Properties set in the attribute (assume property has no logic)
            //    foreach (var namedArg in exportMetadataAttribute.NamedArguments
            //        .Where(namedArg => namedArg.Value.Value != null && namedArg.Value.Value is string))
            //    {
            //        metadata[namedArg.Key] = namedArg.Value.ToCSharpString();
            //    }
            //}

            return metadata;
        }

        private static List<PartExport> GetPartExports(INamedTypeSymbol symbol)
        {
            var metadata = GetExportMetadata(symbol);

            List<PartExport> exports = new List<PartExport>();
            foreach (var exportAttr in symbol.GetAttributes().Where(attr =>
                attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ExportAttribute"))
            {
                exports.Add(new PartExport(ContractReference.Get(symbol, exportAttr), metadata));
            }
            return exports;
        }

        private static List<PartConstructorParameter>? GetImportingConstructorParameters(INamedTypeSymbol symbol)
        {
            var importingCtor = symbol.Constructors
                        .FirstOrDefault(ctor => ctor.GetAttributes()
                            .Any(attr => attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportingConstructorAttribute"));

            if (importingCtor != null)
            {
                var importingConstructorArguments = new List<PartConstructorParameter>();
                foreach (var param in importingCtor.Parameters)
                {
                    var importAttribute = param.GetAttributes().FirstOrDefault(attr =>
                        attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportAttribute");

                    var importManyAttribute = param.GetAttributes().FirstOrDefault(attr =>
                        attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.Composition.ImportManyAttribute");

                    if (importManyAttribute != null)
                    {
                        var mode = GetImportManyMode(param.Type, out var typeMany, out var metadataMany);
                        if (mode >= ImportMode.Many)
                        {
                            importingConstructorArguments.Add(new PartConstructorParameter(param.Type.ToDisplayString(), ContractReference.Get(typeMany, importManyAttribute), false, mode, metadataMany));
                        }
                        continue;
                    }

                    var lazy = GetImportMode(param.Type, out var type, out var metadata);
                    importingConstructorArguments.Add(new PartConstructorParameter(param.Type.ToDisplayString(), ContractReference.Get(type, importAttribute), IsAllowDefault(importAttribute), lazy, metadata));
                }
                return importingConstructorArguments;
            }
            return null;
        }

    }
}
