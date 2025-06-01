using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Pmad.PreBuiltMEF.MsDependencyInjection
{
    public static class RegisterExtensions
    {
        public static IServiceCollection AddPart<TPart>(this IServiceCollection services, Func<IServiceProvider,TPart> factory)
        {
            return services.AddSingleton<Part<TPart>>(sp => new Part<TPart>(factory(sp)));
        }

        public static IServiceCollection RegisterExport<TPart,TExport>(this IServiceCollection services, Func<TPart, TExport> factory)
            where TExport : class
        {
            return services.AddSingleton<TExport>(sp => factory(sp.GetPart<TPart>()));
        }

        public static IServiceCollection RegisterExport<TPart, TExport>(this IServiceCollection services, string name, Func<TPart, TExport> factory)
            where TExport : class
        {
            return services.AddKeyedSingleton<TExport>(name, (sp , _) => factory(sp.GetPart<TPart>()));
        }

        public static IServiceCollection RegisterExport<TPart, TExport>(this IServiceCollection services, Func<TPart, TExport> factory, Dictionary<string, object> metadata)
            where TExport : class
        {
            services.AddSingleton<ExportWithMetadata<TExport>>(new ExportWithMetadata<TExport>(sp => factory(sp.GetPart<TPart>()), metadata));
            return services.AddSingleton<TExport>(sp => factory(sp.GetPart<TPart>()));
        }

        public static IServiceCollection RegisterExport<TPart, TExport>(this IServiceCollection services, string name, Func<TPart, TExport> factory, Dictionary<string, object> metadata)
            where TExport : class
        {
            services.AddKeyedSingleton<ExportWithMetadata<TExport>>(name, new ExportWithMetadata<TExport>(sp => factory(sp.GetPart<TPart>()), metadata));
            return services.AddKeyedSingleton<TExport>(name, (sp, _) => factory(sp.GetPart<TPart>()));
        }

        public static TPart GetPart<TPart>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<Part<TPart>>().Value;
        }

        public static TExport Import<TExport>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<TExport>();
        }

        public static TExport Import<TExport>(this IServiceProvider serviceProvider, string name)
        {
            return serviceProvider.GetRequiredKeyedService<TExport>(name);
        }

        public static TExport OptionalImport<TExport>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<TExport>();
        }

        public static TExport OptionalImport<TExport>(this IServiceProvider serviceProvider, string name)
        {
            return serviceProvider.GetKeyedService<TExport>(name);
        }

        public static Lazy<TExport> ImportLazy<TExport>(this IServiceProvider serviceProvider)
        {
            return new Lazy<TExport>(() => serviceProvider.GetRequiredService<TExport>());
        }

        public static Lazy<TExport> ImportLazy<TExport>(this IServiceProvider serviceProvider, string name)
        {
            return new Lazy<TExport>(() => serviceProvider.GetRequiredKeyedService<TExport>(name));
        }

        public static Lazy<TExport> OptionalImportLazy<TExport>(this IServiceProvider serviceProvider)
        {
            return new Lazy<TExport>(() => serviceProvider.GetService<TExport>());
        }

        public static Lazy<TExport> OptionalImportLazy<TExport>(this IServiceProvider serviceProvider, string name)
        {
            return new Lazy<TExport>(() => serviceProvider.GetKeyedService<TExport>(name));
        }

        public static Lazy<TExport,TMetadata> ImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            var instance = serviceProvider.GetServices<ExportWithMetadata<TExport>>().Single(e => isValidMetadata(e.Metadata));
            return new Lazy<TExport, TMetadata>(() => instance.Factory(serviceProvider), metadataFactory(instance.Metadata));
        }

        public static Lazy<TExport, TMetadata> ImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, string name, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            var instance = serviceProvider.GetKeyedServices<ExportWithMetadata<TExport>>(name).Single(e => isValidMetadata(e.Metadata));
            return new Lazy<TExport, TMetadata>(() => instance.Factory(serviceProvider), metadataFactory(instance.Metadata));
        }

        public static Lazy<TExport, TMetadata> OptionalImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            var instance = serviceProvider.GetServices<ExportWithMetadata<TExport>>().SingleOrDefault(e => isValidMetadata(e.Metadata));
            if (instance != null)
            {
                return new Lazy<TExport, TMetadata>(() => instance.Factory(serviceProvider), metadataFactory(instance.Metadata));
            }
            return null;
        }

        public static Lazy<TExport, TMetadata> OptionalImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, string name, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            var instance = serviceProvider.GetKeyedServices<ExportWithMetadata<TExport>>(name).Single(e => isValidMetadata(e.Metadata));
            if (instance != null)
            {
                return new Lazy<TExport, TMetadata>(() => instance.Factory(serviceProvider), metadataFactory(instance.Metadata));
            }
            return null;
        }

    }
}
