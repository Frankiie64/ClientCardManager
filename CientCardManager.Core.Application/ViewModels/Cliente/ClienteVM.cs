using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;

namespace CientCardManager.Core.Application.ViewModels.Cliente
{
    public class ClienteVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Ocupacion { get; set; }
        public DateTime Creado { get; set; }
        public ICollection<ClienteTarjetaVM> Tarjetas { get; set; }
    }
}
