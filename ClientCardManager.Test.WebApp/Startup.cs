using CientCardManager.Core.Application;
using CientCardManager.Core.Application.Interfaces.Repositorios;
using ClientCardManager.Infrastructure.Persistence.Context;
using ClientCardManager.Infrastructure.Persistence.Repositorios;
using ClientCardManager.Presentation.WebApp.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientCardManager.Test.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ConnectionTest"));
            services.AddTransient(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
            services.AgregarCapaServicio();
        }

        public void Configure(IServiceProvider services) 
        {
            services.cargarTarjetas();
        }
    }
}
