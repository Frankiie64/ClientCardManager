using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;

namespace CientCardManager.Core.Application.ViewModels.ClienteTarjeta
{
    public class ClienteTarjetaVM
    {
        public int Id { get; set; }
        public int IdTipoTarjeta { get; set; }
        public TipoTarjetaVM Tarjeta { get; set; }
        public int IdCliente { get; set; }
        public ClienteVM Cliente { get; set; }
        public string Banco { get; set; }
        public string Numero { get; set; }
        public int MesVencimiento { get; set; }
        public int AnioVencimiento { get; set; }
    }
}
