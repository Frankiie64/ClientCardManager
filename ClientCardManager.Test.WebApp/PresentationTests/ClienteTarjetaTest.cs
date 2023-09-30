using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using ClientCardManager.Presentation.WebApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCardManager.Test.WebApp.PresentationTests
{
    [TestFixture]
    public class ClienteTarjetaTest
    {
        protected TestServer server;
        public IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta> _service;
        public IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> _serviceCliente;
        public IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta> _serviceTarjeta;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _service = server.Host.Services.GetRequiredService<IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta>>();
            _serviceCliente = server.Host.Services.GetRequiredService<IServicioGenerico<SaveClienteVM, ClienteVM, Cliente>>();
            _serviceTarjeta = server.Host.Services.GetRequiredService<IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta>>();
        }
        [Order(0)]
        [Test]
        public async Task CrearPost_DeberiaRetornarUnResultadoExitoso()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);
            
            await _serviceCliente.Add(new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Ocupacion = "OcupacionCliente",
                Telefono = "809-743-5818"

            });

            var requestContent = new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                Numero = "0000-0000-0000-0000",
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            };


            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsTrue(ok);
            Assert.IsTrue(
            await _service.Exists(x =>
                 x.IdTipoTarjeta == requestContent.IdTipoTarjeta &&
                 x.Banco == requestContent.Banco &&
                 x.Numero == requestContent.Numero &&
                 x.MesVencimiento == requestContent.MesVencimiento &&
                 x.AnioVencimiento == requestContent.AnioVencimiento &&
                 x.IdCliente == requestContent.IdCliente
                )
            );
        }
    }
}
