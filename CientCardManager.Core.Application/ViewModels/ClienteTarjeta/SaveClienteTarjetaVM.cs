using System.ComponentModel.DataAnnotations;

namespace CientCardManager.Core.Application.ViewModels.ClienteTarjeta
{
    public class SaveClienteTarjetaVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Es obligatorio seleccionar un tipo de tarjeta.")]
        public int IdTipoTarjeta { get; set; }
        [Required(ErrorMessage = "Es obligatorio seleccionar el cliente a quien le pertenece esta tarjeta.")]
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "El campo banco es obligatorio.")]
        public string Banco { get; set; }
        [Required(ErrorMessage = "El campo numero es obligatorio")]
        public string Numero { get; set; }
        [Required(ErrorMessage = "El campo mes vencimiento es obligatorio")]
        public int MesVencimiento { get; set; }
        [Required(ErrorMessage = "El campo año vencimiento es obligatorio")]
        public int AnioVencimiento { get; set; }
        public string NombreCliente { get; set; }
    }
}
