using System.Collections.Generic;

namespace Pmad.PreBuiltMEF.MsDependencyInjection
{
    public static class MetadataHelper
    {
        public static T GetMetadataValue<T>(this IDictionary<string, object> metadata, string key)
        {
            if (metadata.TryGetValue(key, out object value) && value is T finalValue)
            {
                return finalValue;
            }
            return default;
        }
        public static bool HasMetadata<T>(this IDictionary<string, object> metadata, string key)
        {
            return metadata.TryGetValue(key, out object value) && value is T;
        }
    }
}
