using BWJ.Web.Core.ApiRequests;
using BWJ.Web.Core.Attributes;
using BWJ.Web.Core.Sentinels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BWJ.Web.Core.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServicesInAssembly<TFromAssembly>(this IServiceCollection collection)
        {
            var assm = typeof(TFromAssembly).Assembly;
            var allServices = assm.GetExportedTypes().Where(t => t.IsAbstract == false);

            var namespaceRegistries = assm.GetTypes()
                .Where(t => t.GetCustomAttribute<ApplicationServiceNamespaceSentinelAttribute>() is not null)
                .OrderByDescending(t => t.Namespace);
            foreach (var ns in namespaceRegistries)
            {
                RegisterServicesByNamespace(
                    collection,
                    ns.Namespace,
                    ns.GetCustomAttribute<ApplicationServiceNamespaceSentinelAttribute>().Type,
                    allServices);
            }

            var individualRegistries = allServices
                .Where(s => s.IsClass && s.GetCustomAttribute<ApplicationServiceAttribute>() is not null);
            foreach (var impType in individualRegistries)
            {
                var serviceType = impType.GetInterface($"I{impType.Name}");
                var registerAs = impType.GetCustomAttribute<ApplicationServiceAttribute>().Type;

                if (serviceType is not null &&
                        collection.Any(s => s.ServiceType == serviceType) == false)
                {
                    RegisterService(collection, registerAs, serviceType, impType);
                }
            }

            return collection;
        }
        public static IServiceCollection RegisterApiHandlers(this IServiceCollection services)
        {
            services.AddScoped<IApiHandlerMediator, ApiHandlerMediator>();

            var handlers = typeof(ServiceCollectionExtensions)
                .Assembly.GetExportedTypes()
                .Where(t => t.IsSubclassOfGenericClassDefinition(typeof(ApiHandler<,>)));

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.BaseType, handler);
            }

            return services;
        }

        private static void RegisterServicesByNamespace(
            IServiceCollection serviceCollection,
            string nameSpace,
            ApplicationServiceLifetime registerServicesAs,
            IEnumerable<Type> allServices)
        {
            var implementations = allServices
                .Where(s => s.IsClass && s.Namespace.IndexOf(nameSpace) == 0);

            foreach (var impType in implementations)
            {
                var serviceType = impType.GetInterface($"I{impType.Name}");
                if (serviceType is not null &&
                        impType.GetCustomAttribute<ApplicationServiceAttribute>() is null &&
                        serviceCollection.Any(s => s.ServiceType == serviceType) == false)
                {
                    RegisterService(serviceCollection, registerServicesAs, serviceType, impType);
                }
            }
        }

        private static void RegisterService(
            IServiceCollection services,
            ApplicationServiceLifetime registerAs,
            Type serviceType, Type implementationType)
        {
            switch (registerAs)
            {
                case ApplicationServiceLifetime.Scope:
                    services.AddScoped(serviceType, implementationType);
                    break;
                case ApplicationServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, implementationType);
                    break;
                case ApplicationServiceLifetime.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
            }
        }
    }
}
