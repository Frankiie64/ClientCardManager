using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.Servicios;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CientCardManager.Core.Application
{
    public static class Extension
    {
        public static void AgregarCapaServicio(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IServicioGenerico<,,>), typeof(ServicioGenerico<,,>));

        }
    }
}