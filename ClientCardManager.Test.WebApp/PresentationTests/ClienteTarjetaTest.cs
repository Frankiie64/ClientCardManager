using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using ClientCardManager.Infrastructure.Persistence.Context;
using ClientCardManager.Presentation.WebApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        private ApplicationDbContext _context;
        public IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta> _service;
        public IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> _serviceCliente;
        public IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta> _serviceTarjeta;

        [SetUp]
        public void SetUp()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _service = server.Host.Services.GetRequiredService<IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta>>();
            _serviceCliente = server.Host.Services.GetRequiredService<IServicioGenerico<SaveClienteVM, ClienteVM, Cliente>>();
            _serviceTarjeta = server.Host.Services.GetRequiredService<IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta>>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ConnectionTest")
                .Options;

            _context = new ApplicationDbContext(options);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        [Order(0)]
        [Test]
        public async Task CrearPost_NoExisteCliente_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

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

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
        [Order(1)]
        [Test]
        public async Task CrearPost_SinNumero_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

            var requestContent = new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
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

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
        [Order(2)]
        [Test]
        public async Task CrearPost_SinMes_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

            var requestContent = new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                Numero = "0000-0000-0000-0000",
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            };

            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
        [Order(3)]
        [Test]
        public async Task CrearPost_SinAnio_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

            var requestContent = new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                Numero = "0000-0000-0000-0000",
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            };

            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
        [Order(4)]
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

        [Order(5)]
        [Test]
        public async Task EditarPost_SinNumero_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);
            
            var requestContent = new SaveClienteTarjetaVM
            {
                Id = 1,
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            };

            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
        [Order(6)]
        [Test]
        public async Task EditarPost_SinMes_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

            var requestContent = new SaveClienteTarjetaVM
            {
                Id = 1,
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                Numero = "0000-0001-0000-1000",
                IdCliente = 1,
                IdTipoTarjeta = 1,
            };

            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
        [Order(7)]
        [Test]
        public async Task EditarPost_SinAnio_DeberiaRetornarUnResultadoErroneo()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

            var requestContent = new SaveClienteTarjetaVM
            {
                Id = 1,
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                Numero = "0000-0001-0000-1000",
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            };

            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsFalse(
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
       
        [Order(8)]
        [Test]
        public async Task EditarPost_DeberiaRetornarUnResultadoExitoso()
        {
            // Arrange 
            ClienteTarjetaController controller = new ClienteTarjetaController(_service, _serviceTarjeta, _serviceCliente);

            var requestContent = new SaveClienteTarjetaVM();

            requestContent.Id = 1;
            requestContent.Banco = "BancoClienteTarjetaEditado";
            requestContent.Numero = "0000-0000-0000-0012";
            requestContent.AnioVencimiento = DateTime.Now.AddMonths(2).Year;
            requestContent.MesVencimiento = DateTime.Now.AddMonths(2).Month;
            requestContent.IdCliente = 1;
            requestContent.IdTipoTarjeta = 2;

            // Act
            var response = await controller.Editar(requestContent);
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
        [Order(8)]
        [Test]
        public async Task CrearPost_NumeroExiste_DeberiaRetornarUnResultadoErroneo()
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

            await _service.Add(new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                Numero = "0000-0000-0000-0000",
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            });

            var requestContent = new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjetaExisteTarjeta",
                Banco = "BancoClienteTarjetaExisteTarjeta",
                Numero = "0000-0000-0000-0000",
                AnioVencimiento = DateTime.Now.AddMonths(22).Year,
                MesVencimiento = DateTime.Now.AddMonths(22).Month,
                IdCliente = 1,
                IdTipoTarjeta = 2,
            };

            // Act
            var response = await controller.Crear(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert
            Assert.IsFalse(ok);
            Assert.IsFalse(
            await _service.Exists(x =>
                 x.IdTipoTarjeta == requestContent.IdTipoTarjeta &&
                 x.Banco == requestContent.Banco &&
                 x.Numero == requestContent.Numero &&
                 x.MesVencimiento == requestContent.MesVencimiento &&
                 x.AnioVencimiento == requestContent.AnioVencimiento &&
                 x.IdCliente == requestContent.IdCliente
                )
            );
            Assert.IsTrue(await _service.Exists(x => x.Numero == requestContent.Numero));
        }
        [Order(9)]
        [Test]
        public async Task EditarPost_NumeroExiste_DeberiaRetornarUnResultadoErroneo()
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

            await _service.Add(new SaveClienteTarjetaVM
            {
                NombreCliente = "NombreClienteTarjeta",
                Banco = "BancoClienteTarjeta",
                Numero = "0000-0000-0000-0000",
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                IdCliente = 1,
                IdTipoTarjeta = 1,
            });

            var requestContent = new SaveClienteTarjetaVM
            {
                Id = 1,
                NombreCliente = "NombreClienteTarjetaExisteTarjeta",
                Banco = "BancoClienteTarjetaExisteTarjeta",
                AnioVencimiento = DateTime.Now.AddMonths(22).Year,
                MesVencimiento = DateTime.Now.AddMonths(22).Month,
                Numero = "0000-0000-0000-0000",
                IdCliente = 1,
                IdTipoTarjeta = 2,
            };

            // Act
            var response = await controller.Editar(requestContent);
            var resultData = (dynamic)response.Value;
            var ok = (bool)resultData.GetType().GetProperty("ok").GetValue(resultData);

            // Assert

            Assert.IsFalse(ok);
            Assert.IsFalse(
           await _service.Exists(x =>
                x.IdTipoTarjeta == requestContent.IdTipoTarjeta &&
                x.Banco == requestContent.Banco &&
                x.Numero == requestContent.Numero &&
                x.MesVencimiento == requestContent.MesVencimiento &&
                x.AnioVencimiento == requestContent.AnioVencimiento &&
                x.IdCliente == requestContent.IdCliente
               )
           );
            Assert.IsTrue(await _service.Exists(x => x.Numero == requestContent.Numero));

        }
    }
}
