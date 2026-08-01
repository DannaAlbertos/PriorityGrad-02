using Microsoft.AspNetCore.Mvc;
using PriorityGrad.domain.Interfaces;
using PriorityGrad.domain.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace PriorityGrad.web.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepository _repository;
        public TareaController(ITareaRepository repository) => _repository = repository;

        public IActionResult Index(string ordenarPor) => View(AplicarOrdenamiento(_repository.ObtenerTodas(), ordenarPor));

        public IActionResult Vencidas()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            return View(_repository.ObtenerTodas().Where(t => t.Fecha < hoy).ToList());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var tarea = _repository.ObtenerTodas().FirstOrDefault(t => t.Id == id);
            return tarea == null ? NotFound() : View(tarea);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tarea nuevaTarea)
        {
            // Sincronización automática: si ya existe otra tarea con la misma materia, heredamos su color
            if (!string.IsNullOrEmpty(nuevaTarea.Materia))
            {
                var tareaExistente = _repository.ObtenerTodas()
                    .FirstOrDefault(t => t.Materia.Equals(nuevaTarea.Materia, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(t.ColorHex));

                if (tareaExistente != null)
                {
                    nuevaTarea.ColorHex = tareaExistente.ColorHex;
                    nuevaTarea.ColorTexto = tareaExistente.ColorTexto;
                }
            }

            if (!ModelState.IsValid) return View(nuevaTarea);
            _repository.Guardar(nuevaTarea);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tarea = _repository.ObtenerTodas().FirstOrDefault(t => t.Id == id);
            return tarea == null ? NotFound() : View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tarea tareaEditada, string? returnUrl)
        {
            if (!ModelState.IsValid) return View(tareaEditada);
            _repository.Actualizar(tareaEditada);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id, string? returnUrl)
        {
            _repository.Eliminar(id);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        private List<Tarea> AplicarOrdenamiento(List<Tarea> lista, string criterio) => criterio switch
        {
            "valor" => lista.OrderByDescending(t => t.Valor).ThenByDescending(t => t.Dificultad).ToList(),
            "dificultad" => lista.OrderByDescending(t => t.Dificultad).ThenByDescending(t => t.Valor).ToList(),
            "fecha" => lista.OrderBy(t => t.Fecha).ToList(),
            _ => lista.OrderBy(t => t.Materia).ToList()
        };
    }
}