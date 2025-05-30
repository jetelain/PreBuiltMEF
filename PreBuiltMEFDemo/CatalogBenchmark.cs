using System.ComponentModel.Composition.Primitives;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace PreBuiltMEFDemo
{
    [MemoryDiagnoser]
    [MediumRunJob]
    public class CatalogBenchmark
    {
        [Benchmark(Baseline = true)]
        public List<ComposablePartDefinition> Reflection() => Program.CreateCatalogReflection().ToList();

        [Benchmark]
        public List<ComposablePartDefinition> Prebuilt() => Program.CreateCatalogPrebuilt().ToList();

        [Benchmark]
        public List<ServiceDescriptor> ServiceCollection() => Program.CreateServiceCollection().ToList();
    }
}
