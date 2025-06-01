using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportConstructorLazyMetadataDefinition<TPart, TImport, TMetadata> : PreBuiltImportLazyMetadataDefinition<TPart, TImport, TMetadata> where TPart : class
    {
        private readonly int index;

        public PreBuiltImportConstructorLazyMetadataDefinition(string name, ImportCardinality cardinality, int index, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata)
          : base(name, cardinality, isPrerequisite: true, isRecomposable: false, metadataFactory, isValidMetadata)
        {
            this.index = index;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            var export = exports.FirstOrDefault();
            if (export != null)
            {
                preCompiledComposablePart.SetCtorImport(index, new Lazy<TImport, TMetadata>(() => (TImport)export.Value!, metadataFactory(export.Metadata)));
            }
            else
            {
                preCompiledComposablePart.SetCtorImport(index, null);
            }
        }
    }
}