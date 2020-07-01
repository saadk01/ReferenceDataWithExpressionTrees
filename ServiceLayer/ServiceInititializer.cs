using DataLayer;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLayer
{
    public static class ServiceInititializer
    {
        public static void InitializeProjectServices(this IServiceCollection services)
        {
            services.AddSingleton<IDataBank, DataBank>();
            services.AddSingleton<IRepositoryService, RepositoryService>();
            services.AddSingleton<IReferenceDataService, ReferenceDataService>();
        }
    }
}
