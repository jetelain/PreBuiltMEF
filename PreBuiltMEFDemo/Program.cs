using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Pmad.PreBuiltMEF.MsDependencyInjection;

namespace PreBuiltMEFDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var x = MetadataHelper.GetDefaultConstraint<IExportA>("TEST");

            //var catalog1 = CreateCatalogReflection();
            //var container1 = new CompositionContainer(catalog1);
            //var b1 = container1.GetExportedValue<IExportB>();

            //var catalog2 = CreateCatalogPrebuilt();
            //var container2 = new CompositionContainer(catalog2);
            //var b2 = container2.GetExportedValue<IExportB>();

            //BenchmarkRunner.Run<CatalogBenchmark>(null, args);
            BenchmarkRunner.Run<ComposeBenchmark>(null, args);
            //BenchmarkRunner.Run<FullcaseBenchmark>(null, args);

            //for (int i = 0; i < 10000000; ++i)
            //{
            //    CreateCatalogPrebuilt();
            //}
        }

        public static IExportB CreateContainerAndGetPartB(ComposablePartCatalog catalog1)
        {
            var container1 = new CompositionContainer(catalog1);
            return container1.GetExportedValue<IExportB>()!;
        }

        public static ComposablePartCatalog CreateCatalogPrebuilt()
        {
            var builder = new Pmad.PreBuiltMEF.PreBuiltCatalogBuilder();

            //builder.AddPart<PartA>()
            //    .AddMetadata("Metadata1", "MetadataA")
            //    .AddExport<IExportA>(part => part)
            //    .AddExport<IExportA>("NamedA", part => part)
            //    .AddExport<IExportA1>(part => part.Propperty1)
            //    .AddExport<IExportA1>("NamedA1", part => part.Propperty2);

            //builder.AddPart<PartB>(scope => new PartB(scope.Import<IExportA>(), scope.Import<IExportA1>("NamedA1")))
            //    .AddConstructorImport<IExportA>()
            //    .AddConstructorImport<IExportA1>("NamedA1")
            //    .AddMetadata("Metadata1", "MetadataB")
            //    .AddExport<IExportB>(part => part)
            //    .AddImport<IExportA>("NamedA", (part, value) => part.A = value)
            //    .AddImport<IExportA1>((part, value) => part.A1 = value);

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

        public static ComposablePartCatalog CreateCatalogReflection()
        {
            return new AssemblyCatalog(typeof(Program).Assembly);
        }
    }
}
