namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class ImportBase : ImportOrExportBase
    {
        public ImportBase(ContractReference reference, bool allowDefault) : base(reference)
        {
            AllowDefault = allowDefault;
        }

        public bool AllowDefault { get; }
    }
}
