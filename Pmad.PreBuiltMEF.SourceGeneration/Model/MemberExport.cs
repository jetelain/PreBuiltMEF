namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberExport : ImportOrExportBase
    {
        public MemberExport(string name, ContractReference reference)
            : base(reference)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
