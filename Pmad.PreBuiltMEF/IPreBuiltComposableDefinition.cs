using System;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    public interface IPreBuiltComposableDefinition
    {
        Type TargetType { get; }

        ComposablePart CreatePart(object part);
    }
}