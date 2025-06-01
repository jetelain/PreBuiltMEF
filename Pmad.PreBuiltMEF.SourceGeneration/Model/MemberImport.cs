using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberImport : ImportBase
    {
        public MemberImport(string name, ContractReference reference, bool allowDefault, bool allowRecomposition, ImportMode mode, ITypeSymbol? metadata)
            : base(reference, allowDefault: allowDefault, allowRecomposition: allowRecomposition, mode, metadata)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
