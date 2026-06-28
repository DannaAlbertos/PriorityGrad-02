using Microsoft.AspNetCore.Mvc;
using PriorityGrad.domain.Interfaces;
using PriorityGrad.domain.Models;

namespace PriorityGrad.web.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepository _repository;

        // Inyección de dependencias a través del constructor
        public TareaController(ITareaRepository repository)
        {
            _repository = repository;
        }

        // GET: /Tarea/Index
        public IActionResult Index(string ordenarPor)
        {
            var tareas = _repository.ObtenerTodas();
            var listaOrdenada = AplicarOrdenamiento(tareas, ordenarPor);
            return View(listaOrdenada);
        }

        // GET: /Tarea/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Tarea/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tarea nuevaTarea)
        {
            // Validamos que el modelo cumpla con las reglas definidas en la clase Tarea
            if (ModelState.IsValid)
            {
                _repository.Guardar(nuevaTarea);
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, devolvemos la vista con los datos actuales
            return View(nuevaTarea);
        }

        // Lógica privada para el ordenamiento
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
    }
}