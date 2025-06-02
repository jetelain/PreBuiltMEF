using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    public abstract class PreBuiltPartBuilderBase<TPart> where TPart : class
    {
        private protected readonly List<PreBuiltExportDefinition<TPart>> exportDefinitions = new List<PreBuiltExportDefinition<TPart>>();
        private protected readonly List<PreBuiltImportDefinition<TPart>> importDefinitions = new List<PreBuiltImportDefinition<TPart>>();

        public PreBuiltPartBuilderBase<TPart> AddExport<TExport>(Func<TPart, TExport> value)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(MetadataHelper.GetFullName<TExport>(), value));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddExport<TExport>(string name, Func<TPart, TExport> value)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(name, value));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddExport<TExport>(Func<TPart, TExport> value, Dictionary<string, object?> metadata)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(MetadataHelper.GetFullName<TExport>(), value, metadata));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddExport<TExport>(string name, Func<TPart, TExport> value, Dictionary<string, object?> metadata)
        {
            exportDefinitions.Add(new PreBuiltExportDefinition<TPart, TExport>(name, value, metadata));
            return this;
        }


        // *************************** Member Imports ***************************

        public PreBuiltPartBuilderBase<TPart> AddImport<TImport>(Action<TPart, TImport> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImport<TImport>(string name, Action<TPart, TImport> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImport<TImport>(Action<TPart, Lazy<TImport>> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImport<TImport>(string name, Action<TPart, Lazy<TImport>> value, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyDefinition<TPart, TImport>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImport<TImport, TMetadata>(Action<TPart, Lazy<TImport, TMetadata>> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImport<TImport, TMetadata>(string name, Action<TPart, Lazy<TImport, TMetadata>> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowDefault = false, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberLazyMetadataDefinition<TPart, TImport, TMetadata>(name, allowDefault ? ImportCardinality.ZeroOrOne : ImportCardinality.ExactlyOne, value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }


        // *************************** Member Imports Many ***************************

        public PreBuiltPartBuilderBase<TPart> AddImportMany<TImport>(Action<TPart, TImport[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImportMany<TImport>(string name, Action<TPart, TImport[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyDefinition<TPart, TImport>(name, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImportMany<TImport>(Action<TPart, Lazy<TImport>[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImportMany<TImport>(string name, Action<TPart, Lazy<TImport>[]> value, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyDefinition<TPart, TImport>(name, value, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImportMany<TImport, TMetadata>(Action<TPart, Lazy<TImport, TMetadata>[]> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyMetadataDefinition<TPart, TImport, TMetadata>(MetadataHelper.GetFullName<TImport>(), value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> AddImportMany<TImport, TMetadata>(string name, Action<TPart, Lazy<TImport, TMetadata>[]> value, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata, bool allowRecomposition = false)
        {
            importDefinitions.Add(new PreBuiltImportMemberManyLazyMetadataDefinition<TPart, TImport, TMetadata>(name, value, metadataFactory, isValidMetadata, isRecomposable: allowRecomposition));
            return this;
        }

        public PreBuiltPartBuilderBase<TPart> Apply(Action<PreBuiltPartBuilderBase<TPart>> action)
        {
            action(this);
            return this;
        }
    }
}
