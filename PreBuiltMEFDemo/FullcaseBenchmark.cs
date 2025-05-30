using BenchmarkDotNet.Attributes;

namespace PreBuiltMEFDemo
{
    [MemoryDiagnoser]
    [MediumRunJob]
    public class FullcaseBenchmark
    {
        [Benchmark(Baseline =true)]
        public object Reflection() => Program.CreateContainerAndGetPartB(Program.CreateCatalogReflection());

        [Benchmark]
        public object Prebuilt() => Program.CreateContainerAndGetPartB(Program.CreateCatalogPrebuilt());

        [Benchmark]
        public object ServiceCollection() => Program.CreateContainerAndGetPartB(Program.CreateServiceCollection());
    }
}
