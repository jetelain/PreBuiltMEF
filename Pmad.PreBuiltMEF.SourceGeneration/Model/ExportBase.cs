using System.Collections.Generic;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal abstract class ExportBase : ImportOrExportBase
    {
        public ExportBase(ContractReference reference, Dictionary<string, string> metadata)
            : base(reference)
        {
            Metadata = metadata;
        }

        public Dictionary<string, string> Metadata { get; }
    }
}
