using System.ComponentModel.Composition.Primitives;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace PreBuiltMEFDemoAOT
{
    [MemoryDiagnoser]
    [MediumRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NativeAot80)]
    [MediumRunJob]
    public class CatalogBenchmark
    {
        [Benchmark]
        public List<ComposablePartDefinition> Prebuilt() => Program.CreateCatalogPrebuilt().ToList();

        [Benchmark]
        public List<ServiceDescriptor> ServiceCollection() => Program.CreateServiceCollection().ToList();
    }
}
