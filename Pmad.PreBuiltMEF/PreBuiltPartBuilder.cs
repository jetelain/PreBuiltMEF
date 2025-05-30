
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

        public PreBuiltPartBuilder<TPart> AddMetadata(string name, string value)
        {
            metadata.Add(name, value);
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(Action<TPart, TImport> value)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), ImportCardinality.ExactlyOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImport<TImport>(string name, Action<TPart, TImport> value)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(name, ImportCardinality.ExactlyOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddOptionalImport<TImport>(Action<TPart, TImport> value)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), ImportCardinality.ZeroOrOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddOptionalImport<TImport>(string name, Action<TPart, TImport> value)
        {
            importDefinitions.Add(new PreBuiltImportMemberDefinition<TPart, TImport>(name, ImportCardinality.ZeroOrOne, value));
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportFromConstructor<TImport>()
        {
            importDefinitions.Add(new PreBuiltImportConstructorDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), ImportCardinality.ExactlyOne, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddImportFromConstructor<TImport>(string name)
        {
            importDefinitions.Add(new PreBuiltImportConstructorDefinition<TPart, TImport>(name, ImportCardinality.ExactlyOne, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddOptionalImportFromConstructor<TImport>()
        {
            importDefinitions.Add(new PreBuiltImportConstructorDefinition<TPart, TImport>(MetadataHelper.GetFullName<TImport>(), ImportCardinality.ZeroOrOne, ctorImports));
            ctorImports++;
            return this;
        }

        public PreBuiltPartBuilder<TPart> AddOptionalImportFromConstructor<TImport>(string name)
        {
            importDefinitions.Add(new PreBuiltImportConstructorDefinition<TPart, TImport>(name, ImportCardinality.ZeroOrOne, ctorImports));
            ctorImports++;
            return this;
        }

        

    }
}