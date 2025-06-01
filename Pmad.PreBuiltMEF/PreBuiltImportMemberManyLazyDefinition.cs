using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportMemberManyLazyDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart,TImport>
        where TPart : class
    {
        private readonly Action<TPart, Lazy<TImport>[]> setValue;

        public PreBuiltImportMemberManyLazyDefinition(string name, Action<TPart, Lazy<TImport>[]> value, bool isRecomposable = false)
            : base(name, ImportCardinality.ZeroOrOne, isPrerequisite: false, isRecomposable: isRecomposable)
        {
            this.setValue = value;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            preCompiledComposablePart.AddImportAction(part => setValue(part, exports.Select(export => new Lazy<TImport>(() => (TImport)export.Value!)).ToArray()));
        }
    }
}