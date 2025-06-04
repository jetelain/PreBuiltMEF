using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;

namespace LibraryA
{
    [Export(typeof(IExternalExportB))]
    public class PartExtB : IExternalExportB
    {
        [Import]
        internal IExternalExportA ImportA { get; set; }
    }
}
