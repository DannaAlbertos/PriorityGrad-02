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
            // Apunta a la carpeta 'Data' en la raíz del proyecto web
            string baseDir = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(baseDir, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _filePath = Path.Combine(dataFolder, "tareas.json");

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
            SalvarEnArchivo(tareas);
        }

        // NUEVO: Implementación del método Eliminar
        public void Eliminar(string materia)
        {
            var tareas = ObtenerTodas();
            var tareaAEliminar = tareas.FirstOrDefault(t => t.Materia == materia);

            if (tareaAEliminar != null)
            {
                tareas.Remove(tareaAEliminar);
                SalvarEnArchivo(tareas);
            }
        }

        // Método privado auxiliar para evitar repetir el código de serialización
        private void SalvarEnArchivo(List<Tarea> tareas)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, options);
            File.WriteAllText(_filePath, json);
        }
    }
}