using PriorityGrad.domain.Models;
using System.Collections.Generic;

namespace PriorityGrad.domain.Interfaces
{
    public interface ITareaRepository
    {
        /// <summary>
        /// Obtiene la lista completa de tareas.
        /// </summary>
        List<Tarea> ObtenerTodas();

        /// <summary>
        /// Guarda una nueva tarea en el repositorio.
        /// </summary>
        void Guardar(Tarea tarea);

        /// <summary>
        /// Actualiza una tarea existente usando su Id.
        /// </summary>
        void Actualizar(Tarea tarea);

        /// <summary>
        /// Elimina una tarea basándose en su Id único.
        /// </summary>
        void Eliminar(int id);
    }
}