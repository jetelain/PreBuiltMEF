using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class PartConstructorParameter : ImportBase
    {
        public PartConstructorParameter(string type, ContractReference reference, bool allowDefault, ImportMode mode, ITypeSymbol? metadata) 
            : base(reference, allowDefault, mode, metadata)
        {
            ParamType = type;
        }

        public string ParamType { get; }
    }
}
