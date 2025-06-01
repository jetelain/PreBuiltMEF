using System;
using System.Collections.Generic;
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
            throw new NotImplementedException("RegisterExport with metadata is not implemented yet.");
            //return services.AddSingleton<TExport>(sp => factory(sp.GetPart<TPart>()));
        }

        public static IServiceCollection RegisterExport<TPart, TExport>(this IServiceCollection services, string name, Func<TPart, TExport> factory, Dictionary<string, object> metadata)
            where TExport : class
        {
            throw new NotImplementedException("RegisterExport with metadata is not implemented yet.");
            //return services.AddKeyedSingleton<TExport>(name, (sp, _) => factory(sp.GetPart<TPart>()));
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
            throw new NotImplementedException("ImportLazy with metadata is not implemented yet.");
            //return new Lazy<TExport, TMetadata>(() => serviceProvider.GetRequiredService<TExport>(), default);
        }

        public static Lazy<TExport, TMetadata> ImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, string name, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            throw new NotImplementedException("ImportLazy with metadata is not implemented yet.");
            //return new Lazy<TExport, TMetadata>(() => serviceProvider.GetRequiredKeyedService<TExport>(name), default);
        }

        public static Lazy<TExport, TMetadata> OptionalImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            throw new NotImplementedException("ImportLazy with metadata is not implemented yet.");
            //return new Lazy<TExport, TMetadata>(() => serviceProvider.GetService<TExport>(), default);
        }

        public static Lazy<TExport, TMetadata> OptionalImportLazy<TExport, TMetadata>(this IServiceProvider serviceProvider, string name, Func<IDictionary<string, object>, TMetadata> metadataFactory, Func<IDictionary<string, object>, bool> isValidMetadata)
        {
            throw new NotImplementedException("ImportLazy with metadata is not implemented yet.");
            //return new Lazy<TExport, TMetadata>(() => serviceProvider.GetKeyedService<TExport>(name), default);
        }

    }
}
