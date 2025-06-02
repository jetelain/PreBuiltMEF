using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberImport : ImportBase
    {
        public MemberImport(string name, ContractReference reference, bool allowDefault, bool allowRecomposition, ImportMode mode, ITypeSymbol? metadata, MemberInfos infos)
            : base(reference, allowDefault: allowDefault, allowRecomposition: allowRecomposition, mode, metadata)
        {
            Name = name;
            Infos = infos;
        }

        public string Name { get; }

        public MemberInfos Infos { get; }
    }
}
