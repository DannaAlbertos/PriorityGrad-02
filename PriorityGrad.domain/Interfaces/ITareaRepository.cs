using PriorityGrad.domain.Models;

namespace PriorityGrad.domain.Interfaces
{
    public interface ITareaRepository
    {
        List<Tarea> ObtenerTodas();
        void Guardar(Tarea tarea);
    }
}