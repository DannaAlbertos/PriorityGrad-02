using System.Text.Json;
using PriorityGrad.domain.Models;
using PriorityGrad.domain.Interfaces;

namespace PriorityGrad.infrastructure.Repositories
{
    public class JsonRepository : ITareaRepository
    {
        private readonly string _filePath = "tareas.json";

        public List<Tarea> ObtenerTodas()
        {
            if (!File.Exists(_filePath)) return new List<Tarea>();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Tarea>>(json) ?? new List<Tarea>();
        }

        public void Guardar(Tarea tarea)
        {
            var tareas = ObtenerTodas();
            tareas.Add(tarea);

            string json = JsonSerializer.Serialize(tareas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}