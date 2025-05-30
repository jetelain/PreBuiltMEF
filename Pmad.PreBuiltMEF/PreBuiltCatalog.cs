using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltCatalog : ComposablePartCatalog
    {
        private readonly List<ComposablePartDefinition> parts;

        public PreBuiltCatalog(List<ComposablePartDefinition> parts)
        {
            this.parts = parts;
        }

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.

        public override IQueryable<ComposablePartDefinition> Parts => parts.AsQueryable(); // Not used, but required by the base class

#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            // TODO: Create a more efficient lookup mechanism (with an index/dictionary per ContractName)
            return base.GetExports(definition);
        }
    }
}