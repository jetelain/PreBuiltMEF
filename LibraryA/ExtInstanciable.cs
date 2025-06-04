using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;

namespace LibraryA
{
    public sealed class ExtInstanciable
    {
        [Import]
        internal IExternalExportA ImportA { get; set; }

        [Import]
        internal IExternalExportB ImportB { get; set; }
    }
}
