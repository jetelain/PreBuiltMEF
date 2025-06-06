using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltComposableDefinition<TPart> : IPreBuiltComposableDefinition where TPart : class
    {
        private readonly List<PreBuiltExportDefinition<TPart>> exportDefinitions;
        private readonly List<PreBuiltImportDefinition<TPart>> importDefinitions;

        public PreBuiltComposableDefinition(
            List<PreBuiltExportDefinition<TPart>> exportDefinitions,
            List<PreBuiltImportDefinition<TPart>> importDefinitions)
        {
            this.exportDefinitions = exportDefinitions;
            this.importDefinitions = importDefinitions;
        }

        public Type TargetType => typeof(TPart);

        private PreBuiltComposablePart<TPart> CreatePart(TPart part)
        {
            return new PreBuiltComposablePart<TPart>(new PreBuiltPartDefinition<TPart>(_ => part, exportDefinitions, importDefinitions, MetadataHelper.None, 0));
        }

        public ComposablePart CreatePart(object part)
        {
            return CreatePart((TPart)part);
        }
    }
}
