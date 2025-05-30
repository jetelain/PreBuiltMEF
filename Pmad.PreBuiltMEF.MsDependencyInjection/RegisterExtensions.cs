using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
