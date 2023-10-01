using ClientCardManager.Core.Domain.Common;

namespace ClientCardManager.Core.Domain.Entidad
{
    public class Cliente : EntidadBaseAuditoria
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Ocupacion { get; set; }
        public bool Activo { get; set; } = true;
        public ICollection<ClienteTarjeta> Tarjetas { get; set; }

    }
}
