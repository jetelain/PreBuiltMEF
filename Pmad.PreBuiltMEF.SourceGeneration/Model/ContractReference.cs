using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class ContractReference
    {
        public ContractReference(string type, string? contractName = null)
        {
            ContractName = contractName;
            Type = type;
        }

        public string? ContractName { get; }

        public string Type { get; }


        internal static ContractReference Get(ITypeSymbol type, AttributeData? importOrExportAttribute)
        {
            if (importOrExportAttribute == null)
            {
                return new ContractReference(type.ToDisplayString(), null);
            }

            string? exportType = null;
            string? contractName = null;
            if (importOrExportAttribute.ConstructorArguments.Length == 1)
            {
                var arg = importOrExportAttribute.ConstructorArguments[0];
                if (arg.Kind == TypedConstantKind.Type && arg.Value is INamedTypeSymbol typeSymbol)
                {
                    exportType = typeSymbol.ToDisplayString();
                }
                else if (arg.Kind == TypedConstantKind.Primitive && arg.Value is string s)
                {
                    contractName = s;
                }
            }
            else if (importOrExportAttribute.ConstructorArguments.Length == 2)
            {
                // (string, Type)
                var argType = importOrExportAttribute.ConstructorArguments[1];
                var argContract = importOrExportAttribute.ConstructorArguments[0];
                if (argType.Kind == TypedConstantKind.Type && argType.Value is INamedTypeSymbol typeSymbol)
                {
                    exportType = typeSymbol.ToDisplayString();
                }
                if (argContract.Kind == TypedConstantKind.Primitive && argContract.Value is string s)
                {
                    contractName = s;
                }
            }

            // Si pas de type, on exporte la classe elle-même
            if (string.IsNullOrEmpty(exportType))
            {
                exportType = type.ToDisplayString();
            }

            return new ContractReference(exportType!, contractName);
        }
    }
}
