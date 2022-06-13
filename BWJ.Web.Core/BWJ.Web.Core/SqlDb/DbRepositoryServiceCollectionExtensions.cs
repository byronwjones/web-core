using BWJ.Web.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BWJ.Web.Core.SqlDb
{
    public static class DbRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDatabaseRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDbQueryService, DbQueryService>();

            var types = typeof(DbRepositoryServiceCollectionExtensions).Assembly.GetExportedTypes();
            var repos = types.Where(t => t.IsSubclassOfGenericClassDefinition(typeof(DbRepositoryBase<>)));

            foreach(var repository in repos)
            {
                var iface = repository.GetInterface($"I{repository.Name}", ignoreCase: true);

                if(iface is not null)
                {
                    services.AddSingleton(iface, repository);
                }
            }

            return services;
        }
    }
}
