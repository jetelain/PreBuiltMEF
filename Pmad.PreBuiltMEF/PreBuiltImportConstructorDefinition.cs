using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportConstructorDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart, TImport> where TPart : class
    {
        private readonly int index;

        public PreBuiltImportConstructorDefinition(string name, ImportCardinality cardinality, int index)
          : base(name, cardinality)
        {
            this.index = index;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            var export = exports.FirstOrDefault();
            if (export != null)
            {
                preCompiledComposablePart.SetCtorImport(index, (TImport)export.Value!);
            }
            else
            {
                preCompiledComposablePart.SetCtorImport(index, default!);
            }
        }
    }
}