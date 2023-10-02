using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ClientCardManager.Presentation.WebApp.Controllers
{
    public class ClienteTarjetaController : Controller
    {
        private readonly IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta> _service;
        private readonly IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta> _serviceTipoTarjeta;
        private readonly IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> _serviceCliente;

        public ClienteTarjetaController(IServicioGenerico<SaveClienteTarjetaVM, ClienteTarjetaVM, ClienteTarjeta> service, IServicioGenerico<SaveTipoTarjetaVM, 
            TipoTarjetaVM, TipoTarjeta> serviceTipoTarjeta, IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> serviceCliente)
        {
            _service = service;
            _serviceTipoTarjeta = serviceTipoTarjeta;
            _serviceCliente = serviceCliente;
        }
        public async Task<IActionResult> Index(int id = 0)
        {
            List<ClienteVM> cliente = new List<ClienteVM>()
            {
                new ClienteVM {
                    Id = 0,
                    Nombre = "Selecciona un cliente",
                    Apellido = ""
                }
            };

           cliente.AddRange(await _serviceCliente.GetAll());

            ViewBag.Clientes  = new SelectList(cliente.Select(S => new {
                value = S.Id,
                name = $"{S.Nombre} {S.Apellido}"
            }), "value", "name");
           
            return View(new ClienteTarjetaVM() { IdCliente = id});
            
        }
        [HttpGet]
        public async Task<IActionResult> Crear(int id)
        {
            var tiposTarjetas = await _serviceTipoTarjeta.GetAll();
            ViewBag.TipoTarjeta = new SelectList(tiposTarjetas.Select(S => new {
                value = S.Id,
                name = S.Nombre
            }), "value", "name");

            var response = await _serviceCliente.GetById(id);

            return View("Save", new SaveClienteTarjetaVM() { 
                NombreCliente = $"{response.Nombre} {response.Apellido}",
                MesVencimiento = DateTime.Now.AddMonths(1).Month,
                AnioVencimiento = DateTime.Now.AddMonths(1).Year,
                IdCliente = id
            });
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var tiposTarjetas = await _serviceTipoTarjeta.GetAll();
            ViewBag.TipoTarjeta = new SelectList(tiposTarjetas.Select(S => new {
                value = S.Id,
                name = S.Nombre
            }), "value", "name");

            var response = await _service.GetById(id,x=>x.Cliente);

            var sv = _service.MapepVmToSv(response);
            sv.NombreCliente = $"{response.Cliente.Nombre} {response.Cliente.Apellido}";            

            return View("Save",  sv);
        }

        [HttpPost]
        public async Task<JsonResult> Crear(SaveClienteTarjetaVM request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Where(x => x.Value.Errors.Any())
                                                 .Select(x => new
                                                 {
                                                     PropertyName = x.Key,
                                                     ErrorMessage = x.Value.Errors.First().ErrorMessage
                                                 })
                                                 .ToList();

                    if (errorList.Any())
                    {
                        var firstError = errorList.First();
                        var propertyName = firstError.PropertyName;
                        var errorMessage = firstError.ErrorMessage;

                        if (propertyName != "FechaHasta")
                        {
                            var error = new
                            {
                                ok = false,
                                titulo = $"El campo {propertyName} es invalido",
                                msj = errorMessage,
                                idCliente = request.IdCliente
                            };
                            return Json(error);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(request.Banco))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error banco esta vacio.",
                        msj = "El campo banco no es válido.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (request.Banco.Count() > 30)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Banco",
                        msj = "El campo Banco es demasiado largo.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (string.IsNullOrWhiteSpace(request.Numero))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error número esta vacio.",
                        msj = "El campo número no es válido.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                request.Banco = request.Banco.ToUpper();

                if (!Regex.IsMatch(request.Numero, @"^\d{4}-\d{4}-\d{4}-\d{4}$"))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error Número de tarjeta.",
                        msj = "Este número de tarjeta no es válido.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (request.AnioVencimiento < DateTime.Now.Year)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error Fecha Vencimiento.",
                        msj = "Esta fecha ya expiro",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (request.MesVencimiento > 12 || request.MesVencimiento <= 0 || (request.MesVencimiento <= DateTime.Now.Month && request.AnioVencimiento == DateTime.Now.Year))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error Fecha Vencimiento.",
                        msj = "Esta fecha ya expiro",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }


                if (await _service.Exists(x=>x.Numero == request.Numero))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Número de tarjeta ya existe.",
                        msj = "Este número de tarjeta ya existe.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (!await _serviceCliente.Exists(x => x.Id == request.IdCliente))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error este cliente no existe",
                        msj = "El cliente es inválido.",
                        idCliente = request.IdCliente

                    };

                    return Json(error);
                }

                if (!await _serviceTipoTarjeta.Exists(x => x.Id == request.IdTipoTarjeta))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error este tipo de tarjeta no existe",
                        msj = "Tipo tarjeta es inválido.",
                        idCliente = request.IdCliente

                    };

                    return Json(error);
                }

                if (await _serviceCliente.Exists(x => x.Id == request.Id && x.Activo == false))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error este cliente esta incativo",
                        msj = "El cliente no puede ser actualizado, mientra este inactivo.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                bool result = await _service.Add(request);

                if (!result)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error del servidor",
                        msj = "Por favor, ponerse en contacto con soporte tecnico.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                var response = new
                {
                    ok = true,
                    titulo = "Correcto!!",
                    msj = "Todo ha salido correcto.",
                    idCliente = request.IdCliente
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    ok = false,
                    titulo = "Error del servidor",
                    msj = $"{ex.InnerException.Message}",
                    idCliente = request.IdCliente
                };

                return Json(error);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Editar(SaveClienteTarjetaVM request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Where(x => x.Value.Errors.Any())
                                                 .Select(x => new
                                                 {
                                                     PropertyName = x.Key,
                                                     ErrorMessage = x.Value.Errors.First().ErrorMessage
                                                 })
                                                 .ToList();

                    if (errorList.Any())
                    {
                        var firstError = errorList.First();
                        var propertyName = firstError.PropertyName;
                        var errorMessage = firstError.ErrorMessage;

                        if (propertyName != "FechaHasta")
                        {
                            var error = new
                            {
                                ok = false,
                                titulo = $"El campo {propertyName} es invalido",
                                msj = errorMessage,
                                idCliente = request.IdCliente
                            };
                            return Json(error);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(request.Banco))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error banco esta vacio.",
                        msj = "El campo banco no es válido.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }


                if (request.Banco.Count() > 30)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Banco",
                        msj = "El campo Banco es demasiado largo.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (string.IsNullOrWhiteSpace(request.Numero))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error número esta vacio.",
                        msj = "El campo número no es válido.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                request.Banco = request.Banco.ToUpper();

                if (!Regex.IsMatch(request.Numero, @"^\d{4}-\d{4}-\d{4}-\d{4}$"))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error Número de tarjeta.",
                        msj = "Este número de tarjeta no es válido.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (request.AnioVencimiento < DateTime.Now.Year)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error Fecha Vencimiento.",
                        msj = "Esta fecha ya expiro",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (request.MesVencimiento > 12 || request.MesVencimiento <= 0 || (request.MesVencimiento <= DateTime.Now.Month && request.AnioVencimiento == DateTime.Now.Year ))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error Fecha Vencimiento.",
                        msj = "Esta fecha ya expiro",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

               

                if (await _service.Exists(x => x.Numero == request.Numero && x.Id != request.Id))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error",
                        msj = "Este número de tarjeta ya existe.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                if (!await _serviceCliente.Exists(x => x.Id == request.Id))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error este cliente no existe",
                        msj = "El cliente es inválido.",
                        idCliente = request.IdCliente

                    };

                    return Json(error);
                }

                if (!await _serviceTipoTarjeta.Exists(x => x.Id == request.IdTipoTarjeta))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error este tipo de tarjeta no existe",
                        msj = "Tipo tarjeta es inválido.",
                        idCliente = request.IdCliente

                    };

                    return Json(error);
                }

                if (!await _service.Exists(x => x.Id == request.Id))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error esta asociacón de tarjeta no existe",
                        msj = "Asociacón de tarjeta no existe.",
                        idCliente = request.IdCliente

                    };

                    return Json(error);
                }

                if (await _serviceCliente.Exists(x => x.Id == request.Id && x.Activo == false))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error este cliente esta incativo",
                        msj = "El cliente no puede ser actualizado, mientra este inactivo.",
                        idCliente = request.IdCliente

                    };

                    return Json(error);
                }

                bool result = await _service.Update(request);

                if (!result)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error del servidor",
                        msj = "Por favor, ponerse en contacto con soporte tecnico.",
                        idCliente = request.IdCliente
                    };

                    return Json(error);
                }

                var response = new
                {
                    ok = true,
                    titulo = "Correcto!!",
                    msj = "Todo ha salido correcto.",
                    idCliente = request.IdCliente
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    ok = false,
                    titulo = "Error del servidor",
                    msj = $"{ex.InnerException.Message}",
                    idCliente = request.IdCliente
                };

                return Json(error);
            }
        }
        public async Task<JsonResult> ObtenerClienteTarjetas(int idCliente = 0)
        {
            if (idCliente == 0)
            {
                return Json(new { data = new List<ClienteTarjetaVM>() });
            }
            var result = await _service.GetList(x=> x.IdCliente == idCliente,x=>x.Include(y=>y.Cliente).Include(y=>y.Tarjeta));

            var response = result.Select(x => new
            {
                x.Id,
                nombreCompleto = $"{x.Cliente.Nombre} {x.Cliente.Apellido}",
                tipoTarjeta = x.Tarjeta.Nombre,
                x.Numero,
                x.Banco,
            }).ToList();

            return Json(new { data = response });
        }
    }
}
