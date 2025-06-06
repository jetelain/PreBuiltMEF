using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pmad.PreBuiltMEF
{
    public sealed class PreBuiltCompositionHelper
    {
        private readonly IEnumerable<ComposablePartDefinition> nonDiscoverable;
        private readonly Dictionary<Type, IPreBuildPartDefinition> nonDiscoverableIndex;
        private readonly Dictionary<Type, IPreBuiltComposableDefinition> composableIndex;

        internal PreBuiltCompositionHelper(IEnumerable<ComposablePartDefinition> nonDiscoverable, IEnumerable<IPreBuiltComposableDefinition> composable)
        {
            this.nonDiscoverable = nonDiscoverable;
            this.nonDiscoverableIndex = nonDiscoverable.Cast<IPreBuildPartDefinition>().ToDictionary(p => p.TargetType, p => p);
            this.composableIndex = composable.ToDictionary(p => p.TargetType, p => p);
        }

        public IEnumerable<ComposablePartDefinition> NonDiscoverableParts => nonDiscoverable;

        public bool TryCreateAndComposePart(CompositionContainer container, Type partType, out object? value)
        {
            if (TryCreatePart(partType, out var composablePart))
            {
                container.Compose(new CompositionBatch(new[] { composablePart! }, Enumerable.Empty<ComposablePart>()));
                value = ((IPreBuiltComposablePart)composablePart!).Instance!;
                return true;
            }
            value = null;
            return false;
        }

        public bool TryCreateAndComposePart<TPart>(CompositionContainer container, out TPart? value) where TPart : class
        {
            if (TryCreatePart(typeof(TPart), out var composablePart))
            {
                container.Compose(new CompositionBatch(new[] { composablePart! }, Enumerable.Empty<ComposablePart>()));
                value = (TPart)((PreBuiltComposablePart<TPart>)composablePart!).Instance!;
                return true;
            }
            value = null;
            return false;
        }

        public bool TryComposePart(CompositionContainer container, object part)
        {
            if (TryCreatePart(part, out var composablePart))
            {
                container.Compose(new CompositionBatch(new[] { composablePart! }, Enumerable.Empty<ComposablePart>()));
                return true;
            }
            return false;
        }

        public bool TryCreateAndSatisfyImportsOnce(CompositionContainer container, Type partType, out object? value)
        {
            if (TryCreatePart(partType, out var composablePart))
            {
                container.SatisfyImportsOnce(composablePart!);
                value = ((IPreBuiltComposablePart)composablePart!).Instance!;
                return true;
            }
            value = null;
            return false;
        }

        public bool TryCreateAndSatisfyImportsOnce<TPart>(CompositionContainer container, out TPart? value) where TPart : class
        {
            if (TryCreatePart(typeof(TPart), out var composablePart))
            {
                container.SatisfyImportsOnce(composablePart);
                value = (TPart)((PreBuiltComposablePart<TPart>)composablePart!).Instance!;
                return true;
            }
            value = null;
            return false;
        }

        public bool TrySatisfyImportsOnce(CompositionContainer container, object part)
        {
            if (TryCreatePart(part, out var composablePart))
            {
                container.SatisfyImportsOnce(composablePart);
                return true;
            }
            return false;
        }

        public bool TryCreatePart<TPart>(
#if NET8_0_OR_GREATER
            [NotNullWhen(true)]
#endif
            out ComposablePart? composablePart) where TPart : class
        {
            if (nonDiscoverableIndex.TryGetValue(typeof(TPart), out var definition))
            {
                composablePart = definition.CreatePart();
                return true;
            }
            composablePart = null;
            return false;
        }

        public bool TryCreatePart(Type type,
#if NET8_0_OR_GREATER
            [NotNullWhen(true)]
#endif
            out ComposablePart? composablePart)
        {
            if (nonDiscoverableIndex.TryGetValue(type, out var definition))
            {
                composablePart = definition.CreatePart();
                return true;
            }
#if NET8_0_OR_GREATER
            if (RuntimeFeature.IsDynamicCodeSupported)
            {
#endif
                composablePart = AttributedModelServices.CreatePartDefinition(type, null).CreatePart();
                return true;
#if NET8_0_OR_GREATER
            }
            composablePart = null;
            return false;
#endif
        }

        public bool TryCreatePart(
            object part,
#if NET8_0_OR_GREATER
            [NotNullWhen(true)]
#endif
            out ComposablePart? composablePart)
        {
            var type = part.GetType();
            if (nonDiscoverableIndex.TryGetValue(type, out var definition))
            {
                composablePart = definition.ToComposableDefinition().CreatePart(part);
                return true;
            }
            if (composableIndex.TryGetValue(type, out var composableDefinition))
            {
                composablePart = composableDefinition.CreatePart(part);
                return true;
            }
#if NET8_0_OR_GREATER
            if (RuntimeFeature.IsDynamicCodeSupported)
            {
#endif
                composablePart = AttributedModelServices.CreatePart(part);
                return true;
#if NET8_0_OR_GREATER
            }
            composablePart = null;
            return false;
#endif
        }
    }
}
