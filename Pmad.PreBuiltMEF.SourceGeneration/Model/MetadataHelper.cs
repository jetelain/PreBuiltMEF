using System.Collections.Generic;
using System.Text;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal static class MetadataHelper
    {
        internal static void AppendMetadata(StringBuilder sb, Dictionary<string, string> metadata)
        {
            sb.Append($"new System.Collections.Generic.Dictionary<string, object?>({metadata.Count+1}){{");
            bool isFirst = true;
            foreach (var kvp in metadata)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }
                else
                {
                    isFirst = false;
                }
                sb.Append($"{{{kvp.Key},{kvp.Value}}}");
            }
            sb.Append("}");
        }
    }
}
