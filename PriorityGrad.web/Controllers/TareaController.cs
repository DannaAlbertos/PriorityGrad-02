using Microsoft.AspNetCore.Mvc;
using PriorityGrad.domain.Interfaces;
using PriorityGrad.domain.Models;

namespace PriorityGrad.web.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepository _repository;

        public TareaController(ITareaRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(string ordenarPor)
        {
            var tareas = _repository.ObtenerTodas();
            var listaOrdenada = AplicarOrdenamiento(tareas, ordenarPor);
            return View(listaOrdenada);
        }

        // NUEVO: Vista de Tareas Vencidas
        public IActionResult Vencidas()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            // Filtramos tareas cuya fecha sea menor o igual al día de ayer
            var tareasVencidas = _repository.ObtenerTodas()
                                            .Where(t => t.Fecha <= hoy.AddDays(-1))
                                            .ToList();
            return View(tareasVencidas);
        }

        // NUEVO: Acción para eliminar (llamada desde el formulario en la vista)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(string materia)
        {
            _repository.Eliminar(materia);
            return RedirectToAction(nameof(Vencidas));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tarea nuevaTarea)
        {
            if (!ModelState.IsValid)
            {
                return View(nuevaTarea);
            }

            _repository.Guardar(nuevaTarea);
            return RedirectToAction(nameof(Index));
        }

        private List<Tarea> AplicarOrdenamiento(List<Tarea> lista, string criterio)
        {
            if (lista == null) return new List<Tarea>();

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