using Microsoft.AspNetCore.Mvc;
using PriorityGrad.domain.Interfaces;
using System.Runtime.InteropServices;

namespace PriorityGrad.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ITareaRepository _repository;

        public TareasController(ITareaRepository repository)
        {
            _repository = repository;
        }

        // GET: api/tareas
        [HttpGet]
        public IActionResult ObtenerTodas() => Ok(_repository.ObtenerTodas());

        // GET: api/tareas/dificultadtarea/{id}
        [HttpGet("dificultadtarea/{id}")]
        public IActionResult ObtenerDificultad(int id)
        {
            var tareas = _repository.ObtenerTodas();
            if (id < 0 || id >= tareas.Count) return NotFound("Tarea no encontrada");
            return Ok(tareas[id].Dificultad);
        }

        // GET: api/tareas/fechatarea/{id}
        [HttpGet("fechatarea/{id}")]
        public IActionResult ObtenerFecha(int id)
        {
            var tareas = _repository.ObtenerTodas();
            if (id < 0 || id >= tareas.Count) return NotFound("Tarea no encontrada");
            return Ok(tareas[id].Fecha);
        }

        // GET: api/tareas/valortarea/{id}
        [HttpGet("valortarea/{id}")]
        public IActionResult ObtenerValor(int id)
        {
            var tareas = _repository.ObtenerTodas();
            if (id < 0 || id >= tareas.Count) return NotFound("Tarea no encontrada");
            return Ok(tareas[id].Valor);
        }
    }
}