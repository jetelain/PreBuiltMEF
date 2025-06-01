using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltImportMemberManyDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart,TImport>
        where TPart : class
    {
        private readonly Action<TPart, TImport[]> setValue;

        public PreBuiltImportMemberManyDefinition(string name, Action<TPart, TImport[]> value)
            : base(name, ImportCardinality.ZeroOrMore, false)
        {
            this.setValue = value;
        }

        internal override void SetImport(PreBuiltComposablePart<TPart> preCompiledComposablePart, IEnumerable<Export> exports)
        {
            var value = exports.Select(export => (TImport)export.Value!).ToArray();
            preCompiledComposablePart.AddImportAction(part => setValue(part, value));
        }
    }
}