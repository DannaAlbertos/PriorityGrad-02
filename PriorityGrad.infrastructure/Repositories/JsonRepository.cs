using System.Text.Json;
using PriorityGrad.domain.Models;
using PriorityGrad.domain.Interfaces;
using System.IO;

namespace PriorityGrad.infrastructure.Repositories
{
    public class JsonRepository : ITareaRepository
    {
        private readonly string _filePath;

        public JsonRepository()
        {
            // AppDomain.CurrentDomain.BaseDirectory apunta a la carpeta donde corre la app (ej. bin/Debug/net...)
            // 'Data' es la carpeta donde colocaste tu archivo json
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tareas.json");

            // Aseguramos que la carpeta Data exista para evitar errores
            string directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

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

            // Serializamos la lista completa. WriteIndented = true hace que el JSON sea legible.
            string json = JsonSerializer.Serialize(tareas, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }
    }
}