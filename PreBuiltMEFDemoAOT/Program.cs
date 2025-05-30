using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Pmad.PreBuiltMEF.MsDependencyInjection;

namespace PreBuiltMEFDemoAOT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var catalog2 = CreateCatalogPrebuilt();
            //var container2 = new CompositionContainer(catalog2);
            //var b2 = container2.GetExportedValue<IExportB>();

            var config = DefaultConfig.Instance
                .AddJob(Job.Default.WithRuntime(NativeAotRuntime.Net80));
            //BenchmarkRunner.Run<FullcaseBenchmark>(config, args);
            BenchmarkRunner.Run<ComposeBenchmark>(config, args);
        }

        public static IExportB CreateContainerAndGetPartB(ComposablePartCatalog catalog1)
        {
            var container1 = new CompositionContainer(catalog1);
            return container1.GetExportedValue<IExportB>()!;
        }

        public static ComposablePartCatalog CreateCatalogPrebuilt()
        {
            var builder = new Pmad.PreBuiltMEF.PreBuiltCatalogBuilder();
            _PreBuiltMEF.RegisterAllParts(builder);
            return builder.Build();
        }
        public static ServiceCollection CreateServiceCollection()
        {
            var builder = new ServiceCollection();

            _PreBuiltMsDI.RegisterAllParts(builder);

            return builder;
        }

        public static IExportB CreateContainerAndGetPartB(ServiceCollection collection)
        {
            var container1 = collection.BuildServiceProvider();
            return container1.Import<IExportB>();
        }
    }
}
