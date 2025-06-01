using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportConstructorManyDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart, TImport> where TPart : class
    {
        private readonly int index;

        public PreBuiltImportConstructorManyDefinition(string name, int index)
          : base(name, ImportCardinality.ZeroOrMore, true)
        {
            this.index = index;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            preCompiledComposablePart.SetCtorImport(index, exports.Select(export => (TImport)export.Value!).ToArray());
        }
    }
}