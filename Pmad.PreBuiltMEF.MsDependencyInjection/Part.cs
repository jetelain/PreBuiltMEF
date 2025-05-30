namespace Pmad.PreBuiltMEF.MsDependencyInjection
{
    internal sealed class Part<TPart>
    {
        public Part(TPart part)
        {
            Value = part;
        }

        public TPart Value { get; }
    }
}