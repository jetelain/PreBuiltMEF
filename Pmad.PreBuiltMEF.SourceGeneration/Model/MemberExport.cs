using System.Collections.Generic;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberExport : ExportBase
    {
        public MemberExport(string name, ContractReference reference, Dictionary<string,string> metadata)
            : base(reference, metadata)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
