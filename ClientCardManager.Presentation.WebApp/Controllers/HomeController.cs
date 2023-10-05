using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using Microsoft.AspNetCore.Mvc;

namespace ClientCardManager.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta> _serviceClienteTarjeta;
        private readonly IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> _serviceCliente;

        public HomeController(IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta> serviceClienteTarjeta, 
            IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> serviceCliente)
        {
            _serviceClienteTarjeta = serviceClienteTarjeta;
            _serviceCliente = serviceCliente;
        }
        public async Task<IActionResult> Index()
        {
            var tarjetas = await _serviceClienteTarjeta.GetList(x => x.Creado.Date == DateTime.Now.Date);
            var clientes = await _serviceCliente.GetAll();

            ViewData["clientesTotal"] = clientes.Count();
            ViewData["clientesTotalHoy"] = clientes.Count(x => x.Creado.Date == DateTime.Now.Date);
            ViewData["tarjetasTotalHoy"] = tarjetas.Count();
            return View();
        }
    }
}