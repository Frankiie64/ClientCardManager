using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.Cliente;
using ClientCardManager.Core.Domain.Entidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ClientCardManager.Presentation.WebApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IServicioGenerico<SaveClienteVM,ClienteVM,Cliente> _service;
        public ClienteController(IServicioGenerico<SaveClienteVM, ClienteVM, Cliente> service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View("Save",new SaveClienteVM());
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _service.GetByIdSv(id);
            return View("Save", response);
        }
        [HttpPost]
        public async Task<JsonResult> Crear(SaveClienteVM request)
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
                                msj = errorMessage
                            };
                            return Json(error);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(request.Nombre))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Nombre",
                        msj = "El campo Nombre es inválido.",
                    };

                    return Json(error);
                }

                if (string.IsNullOrWhiteSpace(request.Apellido))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Apellido",
                        msj = "El campo Apellido es inválido.",
                    };

                    return Json(error);
                }
              
                if (!Regex.IsMatch(request.Telefono, @"^\d{3}-\d{3}-\d{4}$"))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Teléfono",
                        msj = "El campo teléfono es inválido.",
                    };

                    return Json(error);
                }

                request.Nombre = request.Nombre.ToUpper();
                request.Apellido = request.Apellido.ToUpper();

                if (!string.IsNullOrWhiteSpace(request.Ocupacion))                
                    request.Ocupacion = request.Ocupacion.ToUpper();
                
                bool result = await _service.Add(request);

                if (!result)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error del servidor",
                        msj = "Por favor, ponerse en contacto con soporte tecnico.",
                    };

                    return Json(error);
                }

                var response = new
                {
                    ok = true,
                    titulo = "Correcto!!",
                    msj = "Todo ha salido correcto.",
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
                };

                return Json(error);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Editar(SaveClienteVM request)
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
                                msj = errorMessage
                            };
                            return Json(error);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(request.Nombre))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Nombre",
                        msj = "El campo Nombre es inválido.",
                    };

                    return Json(error);
                }

                if (string.IsNullOrWhiteSpace(request.Apellido))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Apellido",
                        msj = "El campo Apellido es inválido.",
                    };

                    return Json(error);
                }

               

                if (!Regex.IsMatch(request.Telefono, @"^\d{3}-\d{3}-\d{4}$"))
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error campo Teléfono",
                        msj = "El campo teléfono es inválido.",
                    };

                    return Json(error);
                }

                request.Nombre = request.Nombre.ToUpper();
                request.Apellido = request.Apellido.ToUpper();

                if (!string.IsNullOrWhiteSpace(request.Ocupacion))                
                    request.Ocupacion = request.Ocupacion.ToUpper();
                

                bool result = await _service.Update(request);

                if (!result)
                {
                    var error = new
                    {
                        ok = false,
                        titulo = "Error del servidor",
                        msj = "Por favor, ponerse en contacto con soporte tecnico.",
                    };

                    return Json(error);
                }

                var response = new
                {
                    ok = true,
                    titulo = "Correcto!!",
                    msj = "Todo ha salido correcto.",
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
                };

                return Json(error);
            }
        }
        public async Task<JsonResult> ObtenerClientes()
        {
            var result = await _service.GetList(null,x=>x.Include(y=>y.Tarjetas));

            var response = result.Select(x => new
            {
                x.Id,
                x.Nombre,
                x.Apellido,
                x.Telefono,
                ocupacion = string.IsNullOrWhiteSpace(x.Ocupacion) ? "NO" : x.Ocupacion,
                tarjetas = x.Tarjetas.Count()
            }).ToList();
          
            return Json(new { data = response });            
        }
    }
}
