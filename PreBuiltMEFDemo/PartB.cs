using System.ComponentModel.Composition;

namespace PreBuiltMEFDemo
{
    [Export(typeof(IExportB))]
    [PartMetadata("Metadata1", "MetadataB")]
    internal class PartB : IExportB
    {

        [ImportingConstructor]
        public PartB(IExportA a, [Import("NamedA1")] IExportA1 a1) {

        }

        [Import("NamedA")]
        public IExportA? A { get; set; }

        [Import]
        public IExportA1? A1 { get; set; }

        [Import("Missing", AllowDefault = true)]
        public IExportA? Missing { get; set; }
    }

}
