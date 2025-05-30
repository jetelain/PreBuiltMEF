using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace Pmad.PreBuiltMEF
{
    internal abstract class PreBuiltImportDefinition<TPart> : ImportDefinition where TPart : class
    {
        public PreBuiltImportDefinition()
        {
        }

        public PreBuiltImportDefinition(Expression<Func<ExportDefinition, bool>> constaint, string contractName, ImportCardinality cardinality)
            : base(constaint, contractName, cardinality, false, true)
        {
        }

        internal abstract void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports);

    }
}