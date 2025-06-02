
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Pmad.PreBuiltMEF
{
    public sealed class PreBuiltCatalogBuilder
    {
        private readonly List<Func<ComposablePartDefinition>> parts = new List<Func<ComposablePartDefinition>>();
        private readonly List<Func<ComposablePartDefinition>> nonDiscoverableParts = new List<Func<ComposablePartDefinition>>();

        public PreBuiltCatalogBuilder()
        {
        }

        public PreBuiltPartBuilder<T> AddPart<T>() where T : class, new()
        {
            return AddPart<T>(_ => new T());
        }

        public PreBuiltPartBuilder<T> AddPart<T>(Func<object[], T> factory) where T : class
        {
            var builder = new PreBuiltPartBuilder<T>(factory);
            parts.Add(builder.Build);
            return builder;
        }


        public PreBuiltPartBuilder<T> AddNonDiscoverablePart<T>() where T : class, new()
        {
            return AddNonDiscoverablePart<T>(_ => new T());
        }

        public PreBuiltPartBuilder<T> AddNonDiscoverablePart<T>(Func<object[], T> factory) where T : class
        {
            var builder = new PreBuiltPartBuilder<T>(factory);
            nonDiscoverableParts.Add(builder.Build);
            return builder;
        }

        public PreBuiltPartBuilderBase<T> AddComposable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public ComposablePartCatalog Build()
        {
            return new PreBuiltCatalog(parts.Select(p => p()).ToList());
        }
    }
}