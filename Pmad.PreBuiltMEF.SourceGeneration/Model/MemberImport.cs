using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberImport : ImportBase
    {
        public MemberImport(string name, ContractReference reference, bool allowDefault, ImportMode mode, ITypeSymbol? metadata)
            : base(reference, allowDefault, mode, metadata)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
