using System;
using System.ComponentModel.Composition.Primitives;

namespace Pmad.PreBuiltMEF
{
    internal interface IPreBuildPartDefinition
    {
        Type TargetType { get; }

        ComposablePart CreatePart();

        IPreBuiltComposableDefinition ToComposableDefinition();
    }
}
