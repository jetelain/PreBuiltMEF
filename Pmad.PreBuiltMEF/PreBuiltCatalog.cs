using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltCatalog : ComposablePartCatalog
    {
        private readonly List<ComposablePartDefinition> parts;
        private readonly Dictionary<string, List<Tuple<ComposablePartDefinition, ExportDefinition>>> index;
        public PreBuiltCatalog(List<ComposablePartDefinition> parts)
        {
            this.parts = parts;
            this.index = parts.SelectMany(p => p.ExportDefinitions.Select(e => new Tuple<ComposablePartDefinition, ExportDefinition>(p, e)))
                              .GroupBy(ed => ed.Item2.ContractName)
                              .ToDictionary(g => g.Key, g => g.ToList());
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
            if (index.TryGetValue(definition.ContractName, out var exports))
            {
                return exports.Where(e => definition.IsConstraintSatisfiedBy(e.Item2));
            }
            return Enumerable.Empty<Tuple<ComposablePartDefinition, ExportDefinition>>();
        }
    }
}