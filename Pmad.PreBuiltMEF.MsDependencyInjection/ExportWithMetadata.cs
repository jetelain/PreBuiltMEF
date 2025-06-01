using System;
using System.Collections.Generic;

namespace Pmad.PreBuiltMEF.MsDependencyInjection
{
    internal sealed class ExportWithMetadata<TExport>
    {
        public ExportWithMetadata(Func<IServiceProvider, TExport> factory, Dictionary<string, object> metadata)
        {
            Factory = factory;
            Metadata = metadata;
        }

        public Func<IServiceProvider, TExport> Factory { get; }

        public Dictionary<string, object> Metadata { get; }
    }
}
