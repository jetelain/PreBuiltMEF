
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    public sealed class PreBuiltPartBuilder<TPart> : PreBuiltPartBuilderBase<TPart> where TPart : class
    {
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


        public PreBuiltPartBuilder<TPart> AddMetadata(string name, string value)
        {
            metadata.Add(name, value);
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