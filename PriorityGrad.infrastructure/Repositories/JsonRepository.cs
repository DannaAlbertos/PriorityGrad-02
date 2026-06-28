using System.Text.Json;
using PriorityGrad.domain.Models;
using PriorityGrad.domain.Interfaces;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PriorityGrad.infrastructure.Repositories
{
    public class JsonRepository : ITareaRepository
    {
        private readonly string _filePath;

        public JsonRepository()
        {
            string baseDir = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(baseDir, "Data");

            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

            _filePath = Path.Combine(dataFolder, "tareas.json");

            if (!File.Exists(_filePath)) File.WriteAllText(_filePath, "[]");
        }

        public List<Tarea> ObtenerTodas()
        {
            if (!File.Exists(_filePath)) return new List<Tarea>();
            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Tarea>>(json) ?? new List<Tarea>();
            }
            catch { return new List<Tarea>(); }
        }

        public void Guardar(Tarea tarea)
        {
            var tareas = ObtenerTodas();
            // Asignamos un ID único basado en el máximo actual + 1
            tarea.Id = tareas.Any() ? tareas.Max(t => t.Id) + 1 : 1;
            tareas.Add(tarea);
            SalvarEnArchivo(tareas);
        }

        public void Actualizar(Tarea tareaActualizada)
        {
            var tareas = ObtenerTodas();
            var index = tareas.FindIndex(t => t.Id == tareaActualizada.Id);

            if (index != -1)
            {
                tareas[index] = tareaActualizada;
                SalvarEnArchivo(tareas);
            }
        }

        public void Eliminar(int id)
        {
            var tareas = ObtenerTodas();
            var tareaAEliminar = tareas.FirstOrDefault(t => t.Id == id);

            if (tareaAEliminar != null)
            {
                tareas.Remove(tareaAEliminar);
                SalvarEnArchivo(tareas);
            }
        }

        private void SalvarEnArchivo(List<Tarea> tareas)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, options);
            File.WriteAllText(_filePath, json);
        }
    }
}