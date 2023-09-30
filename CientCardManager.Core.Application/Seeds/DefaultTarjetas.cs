using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;

namespace CientCardManager.Core.Application.Seeds
{
    public  class DefaultTarjetas
    {
        public static async Task AgregarTarjetas(IServicioGenerico<SaveTipoTarjetaVM,TipoTarjetaVM,TipoTarjeta> _servicio)
        {
            try
            {
                var valores = Enum.GetValues(typeof(EnumTiposTarjetas));

                foreach (var item in valores)
                {
                    var result = await _servicio.FindWhere(x => x.Nombre.Trim() == item.ToString().Trim(),null);

                    if (result == null)
                    {
                        await _servicio.Add(new SaveTipoTarjetaVM
                        {
                            Nombre = item.ToString()
                        });
                    }
                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
			}
        }
    }
}
