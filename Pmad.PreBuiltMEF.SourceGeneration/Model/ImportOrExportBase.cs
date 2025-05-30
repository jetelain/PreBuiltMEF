namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal abstract class ImportOrExportBase
    {
        private readonly ContractReference reference;

        public ImportOrExportBase(ContractReference reference)
        {
            this.reference = reference;
        }

        public string? ContractName => reference.ContractName;

        public string Type => reference.Type;
    }
}
