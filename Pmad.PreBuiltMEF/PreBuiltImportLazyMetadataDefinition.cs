using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace Pmad.PreBuiltMEF
{
    internal abstract class PreBuiltImportLazyMetadataDefinition<TPart, TImport, TMetadata> : PreBuiltImportDefinition<TPart, TImport>
        where TPart : class
    {
        protected readonly Func<IDictionary<string, object?>, TMetadata> metadataFactory;
        protected readonly Func<IDictionary<string, object?>, bool> isValidMetadata;

        public PreBuiltImportLazyMetadataDefinition(string contractName, 
            ImportCardinality cardinality, 
            bool isPrerequisite, 
            Func<IDictionary<string, object?>, TMetadata> metadataFactory,
            Func<IDictionary<string, object?>, bool> isValidMetadata)
            : base(contractName, cardinality, isPrerequisite)
        {
            this.metadataFactory = metadataFactory;
            this.isValidMetadata = isValidMetadata;
        }

        public override Expression<Func<ExportDefinition, bool>> Constraint
        {
            get
            {
                if (_constraint == null)
                {
                    var baseConstraint = MetadataHelper.GetDefaultConstraint<TImport>(ContractName);

                    _constraint = Expression.Lambda<Func<ExportDefinition, bool>>(
                        Expression.AndAlso(
                            baseConstraint.Body,
                            Expression.Invoke(Expression.Constant(isValidMetadata), Expression.Property(baseConstraint.Parameters[0], "Metadata"))
                        ),
                        baseConstraint.Parameters[0]
                    );

                }
                return _constraint;
            }
        }

        public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
        {
            return base.IsConstraintSatisfiedBy(exportDefinition) && isValidMetadata(exportDefinition.Metadata);
        }

        public override string ToString()
        {
            return $"Contract=='{ContractName}' and Metadata['ExportTypeIdentity']=='{MetadataHelper.GetFullName<TImport>()}' and Metadata is '{typeof(TMetadata).FullName}'";
        }
    }
}
