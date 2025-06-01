
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    public sealed class PreBuiltPartBuilder<TPart> where TPart : class
    {
        private readonly List<PreBuiltExportDefinition<TPart>> exportDefinitions = new List<PreBuiltExportDefinition<TPart>>();
        private readonly List<PreBuiltImportDefinition<TPart>> importDefinitions = new List<PreBuiltImportDefinition<TPart>>();
        private readonly Func<object[], TPart> factory;
        private readonly Dictionary<string, object?> metadata = new Dictionary<string, object?>();
        private int ctorImports = 0;

        public PreBuiltPartBuilder(Func<object[], TPart> factory)
        {
            this.factory = factory; 
        }

        public ComposablePartDefinition Build()
        {
            return new PreBuiltPartDefinition<TPart>(factory, exportDefinitions, importDefinitions, metadata, ctorImports);
        }

        public PreBuiltPartBuilder<TPart> AddExport<TExport>(Func<TPart, TExport> value)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(MetadataHelper.GetFullName<TExport>(), value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddExport<TExport>(string name, Func<TPart, TExport> value)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(name, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddExport<TExport>(Func<TPart, TExport> value, Dictionary<string,object?> metadata)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(MetadataHelper.GetFullName<TExport>(), value, metadata));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddExport<TExport>(string name, Func<TPart, TExport> value, Dictionary<string, object?> metadata)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(name, value, metadata));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddMetadata(string name, string value)
        {
            metadata.Add(name, value);
            return this;
        }

        // *************************** Member Imports ***************************

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(Action<TPart, TImport> value, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(string name, Action<TPart, TImport> value, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(Action<TPart, Lazy<TImport>> value, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(string name, Action<TPart, Lazy<TImport>> value, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport,TMetadata>(Action<TPart, Lazy<TImport, TMetadata>> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, metadataFactory, isValidMetadata));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport, TMetadata>(string name, Action<TPart, Lazy<TImport, TMetadata>> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyMetadataDefinition<TPart, TImport, TMetadata>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, metadataFactory, isValidMetadata));
            return this;
        }

        // *************************** Constructor imports ***************************

        public PreBuiltPartBuilder<TPart> AddConstructorImport<TImport>(bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImport<TImport>(string name, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, ctorImports));
            ctorImports++;
            return this;
        }
        public PreBuiltPartBuilder<TPart> AddConstructorImportLazy<TImport>(bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportLazy<TImport>(string name, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorLazyDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportLazy<TImport,TMetadata>(Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, ctorImports, metadataFactory, isValidMetadata));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportLazy<TImport, TMetadata>(string name, Func<IDictionary<string,object?>,TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorLazyMetadataDefinition<TPart, TImport, TMetadata>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, ctorImports, metadataFactory, isValidMetadata));
            ctorImports++;
            return this;
        }
    }
}