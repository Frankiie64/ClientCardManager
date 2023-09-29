using CientCardManager.Core.Application.Interfaces.Repositorios;
using ClientCardManager.Infrastructure.Persistence.Context;
using ClientCardManager.Infrastructure.Persistence.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientCardManager.Infrastructure.Persistence
{
    public static class Extension
    {
        public static void AgregarCapaPersisntencia(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("ClientCardManager"),
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                }));


            services.AddTransient(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
