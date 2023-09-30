using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.Seeds;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;

namespace ClientCardManager.Presentation.WebApp.Extensions
{
    public static class CargarDatos
    {
        public static async Task cargarTarjetas(this IServiceProvider app)
        {
            using (var scope = app.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var servicesTipoTarjeta = services.GetRequiredService<IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta>>();

                   await DefaultTarjetas.AgregarTarjetas(servicesTipoTarjeta);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
