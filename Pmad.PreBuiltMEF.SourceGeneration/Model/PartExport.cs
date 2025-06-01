using System.Collections.Generic;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class PartExport : ExportBase
    {
        public PartExport(ContractReference reference, Dictionary<string, string> metadata)
            : base(reference, metadata)
        {
        }
    }
}
