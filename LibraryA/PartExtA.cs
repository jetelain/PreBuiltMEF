using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryA
{
    [Export(typeof(IExternalExportA))]
    internal class PartExtA : IExternalExportA
    {
    }
}
