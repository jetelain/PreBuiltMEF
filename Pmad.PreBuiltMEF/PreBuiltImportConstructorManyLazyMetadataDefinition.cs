using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportConstructorManyLazyMetadataDefinition<TPart, TImport, TMetadata> : PreBuiltImportLazyMetadataDefinition<TPart, TImport, TMetadata> where TPart : class
    {
        private readonly int index;

        public PreBuiltImportConstructorManyLazyMetadataDefinition(string name, int index, Func<IDictionary<string, object?>, TMetadata> metadataFactory, Func<IDictionary<string, object?>, bool> isValidMetadata)
          : base(name, ImportCardinality.ZeroOrOne, true, metadataFactory, isValidMetadata)
        {
            this.index = index;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            preCompiledComposablePart.SetCtorImport(index, exports.Select(export => new Lazy<TImport, TMetadata>(() => (TImport)export.Value!, metadataFactory(export.Metadata))).ToArray());
        }
    }
}