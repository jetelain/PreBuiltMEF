using System.ComponentModel.Composition.Primitives;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;

namespace PreBuiltMEFDemo
{
    [MemoryDiagnoser]
    [MediumRunJob]
    public class ComposeBenchmark
    {
        private readonly ComposablePartCatalog reflection = Program.CreateCatalogReflection();

        private readonly ComposablePartCatalog prebuilt = Program.CreateCatalogPrebuilt();

        private readonly ServiceCollection msdi = Program.CreateServiceCollection();

        [Benchmark(Baseline =true)]
        public object Reflection() => Program.CreateContainerAndGetPartB(reflection);

        [Benchmark]
        public object Prebuilt() => Program.CreateContainerAndGetPartB(prebuilt);

        [Benchmark]
        public object ServiceCollection() => Program.CreateContainerAndGetPartB(msdi);
    }
}
