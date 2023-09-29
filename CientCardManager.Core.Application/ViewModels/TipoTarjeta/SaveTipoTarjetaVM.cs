using System.ComponentModel.DataAnnotations;

namespace CientCardManager.Core.Application.ViewModels.TipoTarjeta
{
    public class SaveTipoTarjetaVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo nombre es obligatorio")]
        public string Nombre { get; set; }
    }
}
