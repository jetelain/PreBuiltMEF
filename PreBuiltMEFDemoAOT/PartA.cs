using System.ComponentModel.Composition;

namespace PreBuiltMEFDemoAOT
{
    [Export(typeof(IExportA))]
    [Export("NamedA",typeof(IExportA))]
    [PartMetadata("Metadata1", "MetadataA")]
    internal class PartA : IExportA, IExportA1
    {
        [Export(typeof(IExportA1))]
        public IExportA1 Propperty1 => this;

        [Export("NamedA1", typeof(IExportA1))]
        public IExportA1 Propperty2 => this;
    }
}
