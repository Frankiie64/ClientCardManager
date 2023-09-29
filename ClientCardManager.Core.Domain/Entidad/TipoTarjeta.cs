using ClientCardManager.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCardManager.Core.Domain.Entidad
{
    public class TipoTarjeta : EntidadBaseAuditoria
    {
        public string Nombre { get; set; }
        public ICollection<ClienteTarjeta> Tarjetas { get; set; }
    }
}
