using System;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace Pmad.PreBuiltMEF
{
    internal abstract class PreBuiltImportDefinition<TPart, TImport> : PreBuiltImportDefinition<TPart> where TPart : class
    {
        private Expression<Func<ExportDefinition, bool>>? _constraint;

        protected PreBuiltImportDefinition(string contractName, ImportCardinality cardinality = ImportCardinality.ExactlyOne) 
            : base(MetadataHelper.NoConstraint, contractName, cardinality)
        {
        }

        public override Expression<Func<ExportDefinition, bool>> Constraint
        {
            get
            {
                if (_constraint == null)
                {
                    // Constraint is not used on a standard case, and is costly to generate, so defer it 
                    _constraint = MetadataHelper.GetDefaultConstraint<TImport>(ContractName);
                }
                return _constraint;
            }
        }

        public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
        {
            if (!string.Equals(exportDefinition.ContractName, ContractName))
            {
                return false;
            }
            if (exportDefinition is IPreBuiltExport<TImport>)
            {
                return true;
            }
            return exportDefinition.Metadata.GetExportTypeIdentity() == MetadataHelper.GetFullName<TImport>();
        }

        public override string ToString()
        {
            return $"Contract=='{ContractName}' and Metadata['ExportTypeIdentity']=='{MetadataHelper.GetFullName<TImport>()}'";
        }

    }
}
