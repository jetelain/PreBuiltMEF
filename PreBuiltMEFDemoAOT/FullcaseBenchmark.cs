using BenchmarkDotNet.Attributes;

namespace PreBuiltMEFDemoAOT
{
    [MemoryDiagnoser]
    [MediumRunJob]
    public class FullcaseBenchmark
    {
        [Benchmark]
        public object Prebuilt() => Program.CreateContainerAndGetPartB(Program.CreateCatalogPrebuilt());

        [Benchmark]
        public object ServiceCollection() => Program.CreateContainerAndGetPartB(Program.CreateServiceCollection());
    }
}
