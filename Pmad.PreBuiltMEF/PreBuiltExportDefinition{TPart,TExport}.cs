using System;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltExportDefinition<TPart, TExport> : PreBuiltExportDefinition<TPart>, IPreBuiltExport<TExport>
    {
        private readonly Func<TPart, TExport> value;

        public PreBuiltExportDefinition(string name, Func<TPart, TExport> value)
            : base(name, MetadataHelper.GetDefaultMetadata<TExport>())
        {
            this.value = value;
        }

        public override object? GetExport(TPart part)
        {
            return value(part);
        }
    }
}
