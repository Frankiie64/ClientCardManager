using System.ComponentModel.DataAnnotations;

namespace CientCardManager.Core.Application.ViewModels.Cliente
{
    public class SaveClienteVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo apellido es obligatorio.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El campo numero de contacto es obligatorio.")]
        public string Telefono { get; set; }
        
        public string? Ocupacion { get; set; }
    }
}
