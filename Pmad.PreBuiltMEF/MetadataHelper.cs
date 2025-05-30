using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace Pmad.PreBuiltMEF
{
    internal static class MetadataHelper
    {
        private const string ExportTypeIdentity = "ExportTypeIdentity";

        private class Holder<TExport>
        {
            public static readonly string FullName = typeof(TExport).FullName!;
            public static readonly IDictionary<string, object?> Value = new Dictionary<string, object?>() { { ExportTypeIdentity, FullName } };
        }
       
        internal static IDictionary<string, object?> GetDefaultMetadata<TExport>()
        {
            return Holder<TExport>.Value;
        }

        internal static Expression<Func<ExportDefinition, bool>> GetDefaultConstraint<TExport>(string contractName)
        {
            return new ContractBasedImportDefinition(contractName, Holder<TExport>.FullName, null, ImportCardinality.ExactlyOne, false, false, System.ComponentModel.Composition.CreationPolicy.Any).Constraint;
        }

        internal static string GetFullName<TExport>()
        {
            return Holder<TExport>.FullName;
        }

        internal static T? GetValue<T>(this IDictionary<string, object?> metadata, string key) where T : class
        {
            if (metadata.TryGetValue(key, out object? value) && value is T finalvalue)
            {
                return finalvalue;
            }
            return default;
        }

        internal static string? GetExportTypeIdentity(this IDictionary<string, object?> metadata)
        {
            return GetValue<string>(metadata, ExportTypeIdentity);
        }

        public static readonly Expression<Func<ExportDefinition, bool>> NoConstraint = e => true;

    }
}
