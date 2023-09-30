using CientCardManager.Core.Application.Interfaces.Servicios;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientCardManager.Presentation.WebApp.Controllers
{
    public class TipoTarjetaController : Controller
    {
        private readonly IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta> _service;
        public TipoTarjetaController(IServicioGenerico<SaveTipoTarjetaVM, TipoTarjetaVM, TipoTarjeta> service)
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
            return View("Save", new SaveTipoTarjetaVM());
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _service.GetByIdSv(id);
            return View("Save", response);
        }
        [HttpPost]
        public async Task<JsonResult> Crear(SaveTipoTarjetaVM request)
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
                        titulo = "Campo invalido",
                        msj = "El campo Nombre es invalido",
                    };

                    return Json(error);
                }

                request.Nombre = request.Nombre.ToUpper();

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
        public async Task<JsonResult> Editar(SaveTipoTarjetaVM request)
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
                        titulo = "Campo invalido",
                        msj = "El campo Nombre es invalido",
                    };

                    return Json(error);
                }

                request.Nombre = request.Nombre.ToUpper();
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
        public async Task<JsonResult> ObtenerTipoTarjetas()
        {
            var result = await _service.GetList(null, x => x.Include(y => y.Tarjetas));

            var response = result.Select(x => new
            {
                x.Id,
                x.Nombre,
                ultimaModificacion = x.UltimaModificacion.HasValue ? x.UltimaModificacion.Value.ToString("dd/MM/yyy") : x.Creado.ToString("dd/MM/yyy"),
                tarjetas = x.Tarjetas.Count(),                
            }).ToList();

            return Json(new { data = response });
        }
    }
}
