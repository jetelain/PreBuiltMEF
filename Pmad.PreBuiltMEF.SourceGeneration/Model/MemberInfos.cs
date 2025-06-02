using Microsoft.CodeAnalysis;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    internal class MemberInfos
    {
        public MemberInfos(Accessibility accessibility, string? containingAssembly = null, string? containingType = null)
        {
            ContainingAssembly = containingAssembly;
            ContainingType = containingType;
            IsPublic = accessibility == Accessibility.Public;
        }

        public string? ContainingAssembly { get; }

        public string? ContainingType { get; }

        public bool IsPublic { get; }

        public bool IsInherited => !string.IsNullOrEmpty(ContainingType);

        public bool IsExternal => !string.IsNullOrEmpty(ContainingAssembly);

    }
}
