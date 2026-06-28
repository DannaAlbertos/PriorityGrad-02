using System.Text.Json;
using PriorityGrad.domain.Models;
using PriorityGrad.domain.Interfaces;
using System.IO;

namespace PriorityGrad.infrastructure.Repositories
{
    public class JsonRepository : ITareaRepository
    {
        private readonly string _filePath;

        // Constructor con soporte para ruta personalizada
        public JsonRepository(string? dataPath = null)
        {
            if (string.IsNullOrEmpty(dataPath))
            {
                // Ruta por defecto si no se pasa nada
                _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tareas.json");
            }
            else
            {
                // Ruta personalizada pasada desde el Program.cs
                _filePath = Path.Combine(dataPath, "tareas.json");
            }

            // Aseguramos que la carpeta exista
            string? directory = Path.GetDirectoryName(_filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Si el archivo no existe, lo inicializamos vacío
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public List<Tarea> ObtenerTodas()
        {
            if (!File.Exists(_filePath)) return new List<Tarea>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Tarea>>(json) ?? new List<Tarea>();
            }
            catch
            {
                return new List<Tarea>();
            }
        }

        public void Guardar(Tarea tarea)
        {
            var tareas = ObtenerTodas();
            tareas.Add(tarea);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, options);

            File.WriteAllText(_filePath, json);
        }
    }
}