using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class SymbolReference
    {
        public SymbolReference(string fullReference, string fullName, string? genericParameters, string? genericConstraintsAsCSharp)
        {
            FullReference = fullReference;
            FullName = fullName;
            GenericParametersPrefixed = genericParameters;
            GenericConstraintsAsCSharp = genericConstraintsAsCSharp;
            MethodName = fullName.Replace('.', '_');
        }

        /// <summary>
        /// Full name of the symbol, with generic parameters if any.   
        /// </summary>
        public string FullReference { get; }

        /// <summary>
        /// Full name of the symbol, without generic parameters if any.   
        /// </summary>
        public string FullName { get; }

        public string? GenericParametersPrefixed { get; }

        public string? GenericConstraintsAsCSharp { get; }

        public string MethodName { get; }

        public static SymbolReference Create(INamedTypeSymbol symbol)
        {
            var fullReference = symbol.ToDisplayString();

            if (symbol.IsGenericType)
            {
                // Nom sans paramètres génériques
                var fullName = fullReference.Substring(0, fullReference.IndexOf('<'));

                // Noms des paramètres génériques
                var genericParameterNames = symbol.TypeParameters.Select(tp => tp.Name).ToArray();

                // Contraintes des paramètres génériques
                var genericConstraints = symbol.TypeParameters
                    .Select(tp =>
                    {
                        var constraints = new List<string>();
                        if (tp.HasReferenceTypeConstraint)
                            constraints.Add("class");
                        if (tp.HasValueTypeConstraint)
                            constraints.Add("struct");
                        if (tp.HasUnmanagedTypeConstraint)
                            constraints.Add("unmanaged");
                        if (tp.HasConstructorConstraint)
                            constraints.Add("new()");
                        constraints.AddRange(tp.ConstraintTypes.Select(ct => ct.ToDisplayString()));
                        return constraints.Count > 0
                            ? $"where {tp.Name} : {string.Join(", ", constraints)}"
                            : null;
                    })
                    .Where(c => c != null)
                    .ToList();

                return new SymbolReference(
                    fullReference,
                    fullName,
                    "," + string.Join(",", genericParameterNames),
                    string.Join(" ", genericConstraints));
            }

            return new SymbolReference(
                fullReference,
                fullReference,
                null,
                null);
        }
    }
}
