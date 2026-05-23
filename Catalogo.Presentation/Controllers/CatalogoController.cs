using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;
        private readonly string _rutaCalificaciones;

        public CatalogoController(ItemService service)
        {
            _service = service;
            _rutaCalificaciones = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "calificaciones.json"
            );
        }

        public IActionResult Index(string? genero)
        {
            var items = string.IsNullOrEmpty(genero)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorGenero(genero);

            ViewBag.Generos = _service.ObtenerGeneros();
            ViewBag.GeneroActual = genero;
            ViewBag.Calificaciones = LeerCalificaciones();

            return View(items);
        }

        [HttpPost]
        public IActionResult Calificar(string videojuego, int puntuacion, string comentario)
        {
            var usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Auth");
            }

            var calificaciones = LeerCalificaciones();

            calificaciones.Add(new CalificacionViewModel
            {
                Usuario = usuario,
                Videojuego = videojuego,
                Puntuacion = puntuacion,
                Comentario = comentario
            });

            GuardarCalificaciones(calificaciones);

            return RedirectToAction("Index");
        }

        private List<CalificacionViewModel> LeerCalificaciones()
        {
            if (!System.IO.File.Exists(_rutaCalificaciones))
            {
                return new List<CalificacionViewModel>();
            }

            var json = System.IO.File.ReadAllText(_rutaCalificaciones);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<CalificacionViewModel>();
            }

            return JsonSerializer.Deserialize<List<CalificacionViewModel>>(json)
                   ?? new List<CalificacionViewModel>();
        }

        private void GuardarCalificaciones(List<CalificacionViewModel> calificaciones)
        {
            var json = JsonSerializer.Serialize(
                calificaciones,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            System.IO.File.WriteAllText(_rutaCalificaciones, json);
        }

        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }
    }

    public class CalificacionViewModel
    {
        public string Usuario { get; set; }
        public string Videojuego { get; set; }
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }
    }
}