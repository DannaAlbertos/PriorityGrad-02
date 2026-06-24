using Microsoft.AspNetCore.Mvc;
using PriorityGrad.web.Models;
using System.Collections.Generic;
using System.Linq;

namespace PriorityGrad.web.Controllers
{
    public class TareaController : Controller
    {
        // Lista estática para simular la base de datos
        private static List<Tarea> _tareas = new List<Tarea>();

        // GET: Tarea
        public IActionResult Index(string ordenarPor)
        {
            // Usamos .ToList() para obtener una copia de la lista en memoria 
            // y sobre ella aplicar el ordenamiento
            var listaOrdenada = _tareas.ToList();

            switch (ordenarPor)
            {
                case "valor":
                    listaOrdenada = listaOrdenada.OrderByDescending(t => t.Valor)
                                                 .ThenByDescending(t => t.Dificultad)
                                                 .ToList();
                    break;
                case "dificultad":
                    listaOrdenada = listaOrdenada.OrderByDescending(t => t.Dificultad)
                                                 .ThenByDescending(t => t.Valor)
                                                 .ToList();
                    break;
                case "fecha":
                    listaOrdenada = listaOrdenada.OrderBy(t => t.Fecha).ToList();
                    break;
                default:
                    listaOrdenada = listaOrdenada.OrderBy(t => t.Materia).ToList();
                    break;
            }

            return View(listaOrdenada);
        }

        // GET: Tarea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tarea/Create
        [HttpPost]
        public IActionResult Create(Tarea nuevaTarea)
        {
            if (ModelState.IsValid)
            {
                _tareas.Add(nuevaTarea);
                return RedirectToAction(nameof(Index));
            }
            return View(nuevaTarea);
        }
    }
}