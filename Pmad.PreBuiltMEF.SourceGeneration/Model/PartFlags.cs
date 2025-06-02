using System;

namespace Pmad.PreBuiltMEF.SourceGeneration.Model
{
    [Flags]
    internal enum PartFlags
    {
        None = 0,
        Public = 1,
        Discoverable = 2,
        Sealed = 4,
        EmptyConstructor = 8,
        GenericTypeDefinition = 16,
        Abstract = 32,
    }
}
