using Microsoft.AspNetCore.Mvc;
using PriorityGrad.domain.Interfaces; // Interfaz en tu dominio
using PriorityGrad.domain.Models;     // Modelos en tu dominio

namespace PriorityGrad.web.Controllers
{
    public class TareaController : Controller
    {
        // El controlador ahora depende de la INTERFAZ, no de la implementación (JsonRepository)
        private readonly ITareaRepository _repository;

        public TareaController(ITareaRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(string ordenarPor)
        {
            // 1. Obtener datos a través del puerto
            var tareas = _repository.ObtenerTodas();

            // 2. Lógica de ordenamiento (puedes mantenerla aquí o moverla a un "Servicio de Dominio")
            var listaOrdenada = AplicarOrdenamiento(tareas, ordenarPor);

            return View(listaOrdenada);
        }

        private List<Tarea> AplicarOrdenamiento(List<Tarea> lista, string criterio)
        {
            return criterio switch
            {
                "valor" => lista.OrderByDescending(t => t.Valor).ThenByDescending(t => t.Dificultad).ToList(),
                "dificultad" => lista.OrderByDescending(t => t.Dificultad).ThenByDescending(t => t.Valor).ToList(),
                "fecha" => lista.OrderBy(t => t.Fecha).ToList(),
                _ => lista.OrderBy(t => t.Materia).ToList()
            };
        }

        [HttpPost]
        public IActionResult Create(Tarea nuevaTarea)
        {
            if (ModelState.IsValid)
            {
                _repository.Guardar(nuevaTarea); // Persistencia vía puerto
                return RedirectToAction(nameof(Index));
            }
            return View(nuevaTarea);
        }
    }
}