namespace ClientCardManager.Core.Domain.Common
{
    public abstract class EntidadBaseAuditoria
    {
        public virtual int Id { get; set; }
        public DateTime Creado{ get; set; }
        public DateTime? UltimaModificacion { get; set; }
    }
}
