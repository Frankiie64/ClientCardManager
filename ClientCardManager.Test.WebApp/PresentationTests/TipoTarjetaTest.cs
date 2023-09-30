using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using ClientCardManager.Presentation.WebApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace ClientCardManager.Test.WebApp.PresentationTests
{
    [TestFixture]
    public class TipoTarjetaTest
    {
        protected TestServer server;
        public IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta> _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _service = server.Host.Services.GetRequiredService<IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta>>();
        }
        [Order(0)]
        [Test]
        public async Task ObtenerTodo_DeberiaRetornarUnResultadoSiempre()
        {

            // Act
            var response = await _service.GetAll();

            // Assert
            Assert.IsTrue(response.Any(x=>x.Nombre == EnumTiposTarjetas.CREDITO.ToString()));
            Assert.IsTrue(response.Any(x => x.Nombre == EnumTiposTarjetas.DEBITO.ToString()));

        }
        [Order(1)]
        [Test]
        public async Task CrearPost_DeberiaRetornarUnResultadoExitoso()
        {
            // Arrange 
            TipoTarjetaController controller = new TipoTarjetaController(_service);

            var requestContent = new SaveTipoTarjetaVM
            {
                Nombre = "TipoTarjeta",               
            };

            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsTrue(ok);
            Assert.IsTrue(await _service.Exists(x=>x.Nombre == requestContent.Nombre));
        }
        [Order(2)]
        [Test]
        public async Task CrearPost_PropiedadNull_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            TipoTarjetaController controller = new TipoTarjetaController(_service);

            var requestContent = new SaveTipoTarjetaVM
            { };

            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsFalse(ok);

        }
        [Order(3)]
        [Test]
        public async Task EditarPost_DeberiaRetornarUnResultadoExitoso()
        {
            // Arrange 
            TipoTarjetaController controller = new TipoTarjetaController(_service);

            var requestContent = new SaveTipoTarjetaVM
            {
                Nombre = "TipoTarjeta",
            };
            
            await controller.Crear(requestContent);
            requestContent.Id = 1;
            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsTrue(ok);
            Assert.IsTrue(await _service.Exists(x => x.Nombre == requestContent.Nombre));
        }
        [Order(4)]
        [Test]
        public async Task EditarPost_PropiedadNull_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            TipoTarjetaController controller = new TipoTarjetaController(_service);

            var requestContent = new SaveTipoTarjetaVM
            {
                Nombre = "TipoTarjeta",
            };

            await controller.Crear(requestContent);

            // Act
            var response = await controller.Editar(new SaveTipoTarjetaVM() { Id = 1});
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsFalse(ok);

        }
    }
}
