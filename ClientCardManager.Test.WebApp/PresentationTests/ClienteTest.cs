using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using ClientCardManager.Presentation.WebApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace ClientCardManager.Test.WebApp.PresentationTests
{
    [TestFixture]
    public class ClienteTest
    {
        protected TestServer server;
        public IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _service = server.Host.Services.GetRequiredService<IServicioGenerico<SaveClienteVM, ClienteVM, Cliente>>();
        }

        [Order(0)]
        [Test]
        public async Task CrearPost_DeberiaRetornarUnResultadoExitoso()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Ocupacion = "OcupacionCliente",
                Telefono = "809-743-5818"
                
            };


            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsTrue(ok);
            Assert.IsTrue(
                await _service.Exists(x =>
                    x.Nombre == requestContent.Nombre &&
                    x.Apellido == requestContent.Apellido &&
                    x.Ocupacion == requestContent.Ocupacion &&
                    x.Telefono == requestContent.Telefono
                )
            );

        }
        [Order(1)]
        [Test]
        public async Task CrearPost_PropiedadNull_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {};


            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsFalse(ok);

        }
        [Order(2)]
        [Test]
        public async Task EditarPost_DeberiaRetornarUnResultadoExitoso()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Ocupacion = "OcupacionCliente",
                Telefono = "809-743-5818"

            };
            await controller.Crear(requestContent);

            requestContent.Id = 1;
            requestContent.Nombre = "NombreEditado";
            requestContent.Apellido = "ApellidoEditado";
            requestContent.Telefono = "809-743-5819";
            requestContent.Ocupacion = "OcupacionEditada";

            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);
            var model = await _service.GetById(requestContent.Id);

            // Assert
            if (!ok)
            {
                Assert.Fail("La operación de edición no tuvo éxito.");
            }

            Assert.IsTrue(requestContent.Id == model.Id);
            Assert.IsTrue(
                await _service.Exists(x =>
                    x.Nombre == requestContent.Nombre &&
                    x.Apellido == requestContent.Apellido &&
                    x.Ocupacion == requestContent.Ocupacion &&
                    x.Telefono == requestContent.Telefono
                )
            );

        }
        [Order(3)]
        [Test]
        public async Task EditarPost_IdNoExiste_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Ocupacion = "OcupacionCliente",
                Telefono = "809-743-5818"

            };
            await controller.Crear(requestContent);

            requestContent.Id = 0;
            requestContent.Nombre = "NombreEditado";
            requestContent.Apellido = "ApellidoEditado";
            requestContent.Telefono = "809-743-5819";
            requestContent.Ocupacion = "OcupacionEditada";

            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
          

        }
        [Order(4)]
        [Test]
        public async Task EditarPost_PropiedadNull_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Ocupacion = "OcupacionCliente",
                Telefono = "809-743-5818"

            };
            await controller.Crear(requestContent);


            // Act
            var response = await controller.Editar(new SaveClienteVM { Id = 1});
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);

        }
        [Order(4)]
        [Test]
        public async Task EditarPost_PropiedadNullOcupacion_DeberiaRetornarUnResultadoExistoso()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Telefono = "809-743-5818"

            };
            


            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsTrue(ok);
            Assert.IsTrue(
                await _service.Exists(x =>
                    x.Nombre == requestContent.Nombre &&
                    x.Apellido == requestContent.Apellido &&
                    x.Ocupacion == requestContent.Ocupacion &&
                    x.Telefono == requestContent.Telefono
                )
            );

        }
        [Order(6)]
        [Test]
        public async Task EditarPost_PropiedadNullOcupacion_DeberiaRetornarUnResultadoExisto()
        {
            // Arrange 
            ClienteController controller = new ClienteController(_service);

            var requestContent = new SaveClienteVM
            {
                Nombre = "NombreCliente",
                Apellido = "ApellidoCliente",
                Telefono = "809-743-5818"

            };
            await controller.Crear(requestContent);

            // Act
            var response = await controller.Editar(new SaveClienteVM { Id = 1 });
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsTrue(
                await _service.Exists(x =>
                    x.Nombre == requestContent.Nombre &&
                    x.Apellido == requestContent.Apellido &&
                    x.Ocupacion == requestContent.Ocupacion &&
                    x.Telefono == requestContent.Telefono
                )
            );

        }

    }
}
