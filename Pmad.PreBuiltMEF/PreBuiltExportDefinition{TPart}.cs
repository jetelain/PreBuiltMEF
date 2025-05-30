using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    internal abstract class PreBuiltExportDefinition<TPart> : ExportDefinition
    {
        protected PreBuiltExportDefinition(string contractName, IDictionary<string, object?>? metadata)
            : base(contractName, metadata)
        {
        }

        public abstract object? GetExport(TPart part);
    }
}