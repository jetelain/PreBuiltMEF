using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class ImportBase : ImportOrExportBase
    {
        public ImportBase(ContractReference reference, bool allowDefault, ImportMode mode, ITypeSymbol? metadata) : base(reference)
        {
            AllowDefault = allowDefault;
            Mode = mode;
            Metadata = metadata;
        }

        public bool AllowDefault { get; }

        public ImportMode Mode { get; }

        public ITypeSymbol? Metadata { get; }

        public string GetGenericArgs()
        {
            if (Metadata != null)
            {
                return $"{Type},{Metadata.ToDisplayString()}";
            }
            return Type;
        }
    }
}
