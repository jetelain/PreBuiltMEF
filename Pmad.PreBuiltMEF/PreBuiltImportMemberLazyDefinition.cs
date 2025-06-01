using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportMemberLazyDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart,TImport>
        where TPart : class
    {
        private readonly Action<TPart, Lazy<TImport>> setValue;

        public PreBuiltImportMemberLazyDefinition(string name, ImportCardinality cardinality, Action<TPart, Lazy<TImport>> value)
            : base(name, cardinality, false)
        {
            this.setValue = value;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            var export = exports.FirstOrDefault();
            if (export != null)
            {
                preCompiledComposablePart.AddImportAction(part => setValue(part, new Lazy<TImport>(() => (TImport)export.Value!)));
            }
        }
    }
}