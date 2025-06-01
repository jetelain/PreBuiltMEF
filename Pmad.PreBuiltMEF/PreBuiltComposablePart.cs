using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    internal sealed class PreBuiltComposablePart<TPart> : ComposablePart where TPart : class
    {
        private readonly PreBuiltPartDefinition<TPart> definition;
        private object?[]? ctorArray;
        private Action<TPart>? importActions;
        private TPart? instance;

        public PreBuiltComposablePart(PreBuiltPartDefinition<TPart> definition)
        {
            this.definition = definition;
            if (definition.CtorImports != 0)
            {
                this.ctorArray = new object[definition.CtorImports];
            }
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions => definition.ExportDefinitions;

        public override IEnumerable<ImportDefinition> ImportDefinitions => definition.ImportDefinitions;

        public override object? GetExportedValue(ExportDefinition definition)
        {
            return ((PreBuiltExportDefinition<TPart>)definition).GetExport(instance!);
        }

        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            ((PreBuiltImportDefinition<TPart>)definition).SetImport(this, exports);
        }

        public override void Activate()
        {
            instance = definition.Factory(ctorArray!);
            importActions?.Invoke(instance);
            ctorArray = null;
            importActions = null;
            (instance as IPartImportsSatisfiedNotification)?.OnImportsSatisfied();
        }

        internal void AddImportAction(Action<TPart> importAction)
        {
            if (instance != null)
            {
                importAction(instance);
                return;
            }
            importActions += importAction;
        }

        internal void SetCtorImport(int index, object? export)
        {
            ctorArray![index] = export;
        }
    }
}