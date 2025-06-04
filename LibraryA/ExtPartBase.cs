using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryA
{
    public abstract class ExtPartBase
    {

        [Import]
        public IExternalExportB PublicImport { get; set; }

        [Import]
        internal IExternalExportA HiddenImport { get; set; }

    }
}
