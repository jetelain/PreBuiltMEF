using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportConstructorLazyDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart, TImport> where TPart : class
    {
        private readonly int index;

        public PreBuiltImportConstructorLazyDefinition(string name, ImportCardinality cardinality, int index)
          : base(name, cardinality, true)
        {
            this.index = index;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            var export = exports.FirstOrDefault();
            if (export != null)
            {
                preCompiledComposablePart.SetCtorImport(index, new Lazy<TImport>(() => (TImport)export.Value!));
            }
            else
            {
                preCompiledComposablePart.SetCtorImport(index, null);
            }
        }
    }
}