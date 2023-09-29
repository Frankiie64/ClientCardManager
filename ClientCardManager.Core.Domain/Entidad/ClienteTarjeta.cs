using ClientCardManager.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCardManager.Core.Domain.Entidad
{
    public class ClienteTarjeta : EntidadBaseAuditoria
    {
        public int IdTipoTarjeta { get; set; }
        public TipoTarjeta Tarjeta { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
        public string Banco { get; set; }
        public string Numero { get; set; }
        public int MesVencimiento { get; set; }
        public int AnioVencimiento { get; set; }
    }
}
