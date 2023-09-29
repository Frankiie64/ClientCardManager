using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;

namespace CientCardManager.Core.Application.ViewModels.TipoTarjeta
{
    public class TipoTarjetaVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? UltimaModificacion { get; set; }
        public ICollection<ClienteTarjetaVM> Tarjetas { get; set; }
    }
}
