using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace Pmad.PreBuiltMEF
{
    public static class MetadataHelper
    {
        private const string ExportTypeIdentity = "ExportTypeIdentity";

        internal static readonly Dictionary<string, object?> None = new Dictionary<string, object?>(0);

        private class Holder<TExport>
        {
            public static readonly string FullName = typeof(TExport).FullName!;
            public static readonly IDictionary<string, object?> Value = new Dictionary<string, object?>() { { ExportTypeIdentity, FullName } };
        }
       
        internal static IDictionary<string, object?> GetDefaultMetadata<TExport>()
        {
            return Holder<TExport>.Value;
        }

        internal static IDictionary<string, object?> GetDefaultMetadata<TExport>(Dictionary<string, object?> metadata)
        {
            metadata.Add(ExportTypeIdentity, Holder<TExport>.FullName);
            return metadata;
        }

        internal static Expression<Func<ExportDefinition, bool>> GetDefaultConstraint<TExport>(string contractName)
        {
            return new ContractBasedImportDefinition(contractName, Holder<TExport>.FullName, null, ImportCardinality.ExactlyOne, false, false, System.ComponentModel.Composition.CreationPolicy.Any).Constraint;
        }

        internal static string GetFullName<TExport>()
        {
            return Holder<TExport>.FullName;
        }

        public static T GetMetadataValue<T>(this IDictionary<string, object?> metadata, string key)
        {
            if (metadata.TryGetValue(key, out object? value) && value is T finalvalue)
            {
                return finalvalue;
            }
            return default!;
        }

        public static bool HasMetadata<T>(this IDictionary<string, object?> metadata, string key)
        {
            return metadata.TryGetValue(key, out object? value) && value is T;
        }

        internal static string? GetExportTypeIdentity(this IDictionary<string, object?> metadata)
        {
            return GetMetadataValue<string>(metadata, ExportTypeIdentity);
        }

        internal static readonly Expression<Func<ExportDefinition, bool>> NoConstraint = e => true;

    }
}
