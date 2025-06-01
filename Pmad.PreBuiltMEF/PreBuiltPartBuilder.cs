
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

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(Action<TPart, TImport> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(string name, Action<TPart, TImport> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(Action<TPart, Lazy<TImport>> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(string name, Action<TPart, Lazy<TImport>> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport,TMetadata>(Action<TPart, Lazy<TImport, TMetadata>> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport, TMetadata>(string name, Action<TPart, Lazy<TImport, TMetadata>> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyMetadataDefinition<TPart, TImport, TMetadata>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }


        // *************************** Member Imports Many ***************************

        public PreBuiltPartBuilder<TPart> AddImportMany<TImport>(Action<TPart, TImport[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportMany<TImport>(string name, Action<TPart, TImport[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyDefinition<TPart, TImport>(name, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportMany<TImport>(Action<TPart, Lazy<TImport>[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportMany<TImport>(string name, Action<TPart, Lazy<TImport>[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyDefinition<TPart, TImport>(name, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportMany<TImport, TMetadata>(Action<TPart, Lazy<TImport, TMetadata>[]> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportMany<TImport, TMetadata>(string name, Action<TPart, Lazy<TImport, TMetadata>[]> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyMetadataDefinition<TPart, TImport, TMetadata>(name, value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
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


        // *************************** Constructor Many imports ***************************

        public PreBuiltPartBuilder<TPart> AddConstructorImportMany<TImport>(bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorManyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportMany<TImport>(string name, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorManyDefinition<TPart, TImport>(name, ctorImports));
            ctorImports++;
            return this;
        }
        public PreBuiltPartBuilder<TPart> AddConstructorImportManyLazy<TImport>(bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorManyLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportManyLazy<TImport>(string name, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorManyLazyDefinition<TPart, TImport>(name, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportManyLazy<TImport, TMetadata>(Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorManyLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), ctorImports, metadataFactory, isValidMetadata));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddConstructorImportManyLazy<TImport, TMetadata>(string name, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false)
        {
            importDefinitions.Add(new PreBuiltImportConstructorManyLazyMetadataDefinition<TPart, TImport, TMetadata>(name, ctorImports, metadataFactory, isValidMetadata));
            ctorImports++;
            return this;
        }
    }
}