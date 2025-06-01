using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace Pmad.PreBuiltMEF
{
    internal abstract class PreBuiltImportDefinition<TPart> : ImportDefinition where TPart : class
    {
        protected PreBuiltImportDefinition(Expression<Func<ExportDefinition, bool>> constaint, string contractName, ImportCardinality cardinality, bool isPrerequisite, bool isRecomposable)
            : base(constaint, contractName, cardinality, isRecomposable: isRecomposable, isPrerequisite: isPrerequisite)
        {
        }

        internal abstract void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports);

    }
}