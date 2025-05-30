using System.ComponentModel.Composition.Primitives;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace PreBuiltMEFDemoAOT
{
    [MemoryDiagnoser]
    [MediumRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NativeAot80)]
    [MediumRunJob]
    public class ComposeBenchmark
    {
        private readonly ComposablePartCatalog prebuilt = Program.CreateCatalogPrebuilt();

        private readonly ServiceCollection msdi = Program.CreateServiceCollection();

        [Benchmark]
        public object Prebuilt() => Program.CreateContainerAndGetPartB(prebuilt); 
        
        [Benchmark]
        public object ServiceCollection() => Program.CreateContainerAndGetPartB(msdi);
    }
}
