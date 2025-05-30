namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberImport : ImportBase
    {
        public MemberImport(string name, ContractReference reference, bool allowDefault)
            : base(reference, allowDefault)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
