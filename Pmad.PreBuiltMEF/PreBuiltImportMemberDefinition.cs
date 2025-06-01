using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportMemberDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart,TImport>
        where TPart : class
    {
        private readonly Action<TPart, TImport> setValue;

        public PreBuiltImportMemberDefinition(string name, ImportCardinality cardinality, Action<TPart, TImport> value, bool isRecomposable = false)
            : base(name, cardinality, isPrerequisite: false, isRecomposable: isRecomposable)
        {
            this.setValue = value;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            var export = exports.FirstOrDefault();
            if (export != null)
            {
                var value = (TImport)export.Value!;
                preCompiledComposablePart.AddImportAction(part => setValue(part, value));
            }
        }
    }
}