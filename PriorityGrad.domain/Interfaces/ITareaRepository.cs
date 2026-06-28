using PriorityGrad.domain.Models;

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
        /// Elimina una tarea basándose en su nombre (Materia).
        /// </summary>
        void Eliminar(string materia);
    }
}