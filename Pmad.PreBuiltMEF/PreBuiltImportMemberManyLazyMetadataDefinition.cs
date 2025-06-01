using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportMemberManyLazyMetadataDefinition<TPart, TImport, TMetadata> : PreBuiltImportLazyMetadataDefinition<TPart,TImport, TMetadata>
        where TPart : class
    {
        private readonly Action<TPart, Lazy<TImport, TMetadata>[]> setValue;

        public PreBuiltImportMemberManyLazyMetadataDefinition(string name, 
            Action<TPart, Lazy<TImport,TMetadata>[]> value,
            Func<IDictionary<string, object?>, TMetadata> metadataFactory,
            Func<IDictionary<string, object?>, bool> isValidMetadata)
            : base(name, ImportCardinality.ZeroOrMore, false, metadataFactory, isValidMetadata)
        {
            this.setValue = value;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            preCompiledComposablePart.AddImportAction(part => setValue(part, exports.Select(export => new Lazy<TImport, TMetadata>(() => (TImport)export.Value!, metadataFactory(export.Metadata))).ToArray()));
        }
    }
}