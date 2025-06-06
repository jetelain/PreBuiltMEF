using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltPartDefinition<TPart> : ComposablePartDefinition, IPreBuildPartDefinition where TPart : class
    {
        private readonly Func<object[], TPart> factory;
        private readonly List<PreBuiltExportDefinition<TPart>> exportDefinitions;
        private readonly List<PreBuiltImportDefinition<TPart>> importDefinitions;
        private readonly Dictionary<string, object?> metadata;
        private readonly int ctorImports;

        public PreBuiltPartDefinition(Func<object[], TPart> factory, List<PreBuiltExportDefinition<TPart>> exportDefinitions, List<PreBuiltImportDefinition<TPart>> importDefinitions, Dictionary<string, object?> metadata, int ctorImports)
        {
            this.factory = factory;
            this.exportDefinitions = exportDefinitions;
            this.importDefinitions = importDefinitions;
            this.metadata = metadata;
            this.ctorImports = ctorImports;
        }

        public Func<object[], TPart> Factory => factory;

        public int CtorImports => ctorImports;

        public override IEnumerable<ExportDefinition> ExportDefinitions => exportDefinitions;

        public override IEnumerable<ImportDefinition> ImportDefinitions => importDefinitions;

        public override IDictionary<string, object?> Metadata => metadata;

        public Type TargetType => typeof(TPart);

        public override ComposablePart CreatePart()
        {
            return new PreBuiltComposablePart<TPart>(this);
        }

        public IPreBuiltComposableDefinition ToComposableDefinition()
        {
            return new PreBuiltComposableDefinition<TPart>(exportDefinitions, importDefinitions.Where(i => !i.IsPrerequisite).ToList());
        }
    }
}