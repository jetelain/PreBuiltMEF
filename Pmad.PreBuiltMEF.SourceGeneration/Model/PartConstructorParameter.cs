namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class PartConstructorParameter : ImportBase
    {
        public PartConstructorParameter(string type, ContractReference reference, bool allowDefault) 
            : base(reference, allowDefault)
        {
            ParamType = type;
        }

        public string ParamType { get; }
    }
}
