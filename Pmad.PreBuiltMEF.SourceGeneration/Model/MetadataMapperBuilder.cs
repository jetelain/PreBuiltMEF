using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MetadataMapperBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();

        private readonly HashSet<string> _names = new HashSet<string>();

        public string GetOrCreate(ITypeSymbol typeSymbol)
        {
            var name = typeSymbol.ToDisplayString().Replace('.','_');
            if (_names.Add(name))
            {
                _builder.AppendLine($"  private sealed class {name}Impl : {typeSymbol.ToDisplayString()}");
                _builder.AppendLine("  {");
                _builder.AppendLine("    private readonly System.Collections.Generic.IDictionary<string, object?> metadata;");
                _builder.AppendLine($"    internal {name}Impl(System.Collections.Generic.IDictionary<string, object?> metadata)");
                _builder.AppendLine("    {");
                _builder.AppendLine("      this.metadata = metadata;");
                _builder.AppendLine("    }");
                foreach(var property in typeSymbol.GetMembers().OfType<IPropertySymbol>())
                {
                    _builder.AppendLine($"    public {property.Type.ToDisplayString()} {property.Name} => metadata.GetMetadataValue<{property.Type.ToDisplayString()}>(\"{property.Name}\");");
                }
                _builder.Append("    public static bool IsValid(System.Collections.Generic.IDictionary<string, object?> metadata)");
                var first = true;
                foreach (var property in typeSymbol.GetMembers().OfType<IPropertySymbol>())
                {
                    _builder.AppendLine();
                    _builder.Append("      ");
                    if (first)
                    {
                        first = false;
                        _builder.Append("=> ");
                    }
                    else
                    {
                        _builder.Append(" &&");
                    }
                    _builder.Append($"metadata.HasMetadata<{property.Type.ToDisplayString()}>(\"{property.Name}\")");
                }
                _builder.AppendLine(";");


                _builder.AppendLine($"    public static {typeSymbol.ToDisplayString()} Create (System.Collections.Generic.IDictionary<string, object?> metadata)");
                _builder.AppendLine($"      => new {name}Impl(metadata);");
                _builder.AppendLine("  }");
            }
            return name + "Impl";
        }


        public void AppendTo(StringBuilder target)
        {
            target.Append(_builder);
        }
    }
}
