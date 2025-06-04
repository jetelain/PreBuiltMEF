using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Pmad.PreBuiltMEF.MsDependencyInjection;
using PreBuiltMEFDemo.Samples;

namespace PreBuiltMEFDemoAOT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var catalog2 = CreateCatalogPrebuilt();
            //var container2 = new CompositionContainer(catalog2);
            //var b2 = container2.GetExportedValue<IExportB>();

            //BenchmarkRunner.Run<CatalogBenchmark>(null, args);
            BenchmarkRunner.Run<ComposeBenchmark>(null, args);
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
            LibraryA._PreBuiltMEF.RegisterAllParts(builder);

            return builder.Build();
        }

        public static ServiceCollection CreateServiceCollection()
        {
            var builder = new ServiceCollection();
            _PreBuiltMsDI.RegisterAllParts(builder);
            LibraryA._PreBuiltMsDI.RegisterAllParts(builder);
            return builder;
        }

        public static IExportB CreateContainerAndGetPartB(ServiceCollection collection)
        {
            var container1 = collection.BuildServiceProvider();
            return container1.Import<IExportB>();
        }
    }
}
