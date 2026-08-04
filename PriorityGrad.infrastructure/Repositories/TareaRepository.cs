using Microsoft.EntityFrameworkCore;
using PriorityGrad.domain;
using PriorityGrad.domain.Interfaces;
using PriorityGrad.domain.Models;
using PriorityGrad.infrastructure.Data;

namespace PriorityGrad.infrastructure.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly AppDbContext _context;

        public TareaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Tarea> ObtenerTodas()
        {
            return _context.Tareas.ToList();
        }

        public void Guardar(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            _context.SaveChanges();
        }

        public void Actualizar(Tarea tarea)
        {
            // Buscamos la tarea original en la base de datos para preservar el UsuarioEmail
            var tareaOriginal = _context.Tareas.Find(tarea.Id);
            if (tareaOriginal != null)
            {
                tareaOriginal.Titulo = tarea.Titulo;
                tareaOriginal.Materia = tarea.Materia;
                tareaOriginal.Valor = tarea.Valor;
                tareaOriginal.Dificultad = tarea.Dificultad;
                tareaOriginal.Fecha = tarea.Fecha;
                tareaOriginal.Comentario = tarea.Comentario;
                tareaOriginal.NombreProfesor = tarea.NombreProfesor;
                tareaOriginal.Instrucciones = tarea.Instrucciones;
                tareaOriginal.HoraEntrega = tarea.HoraEntrega;
                tareaOriginal.ImagenUrl = tarea.ImagenUrl;
                tareaOriginal.ColorHex = tarea.ColorHex;
                tareaOriginal.ColorTexto = tarea.ColorTexto;

                // Si el controlador mandó un correo válido, lo aseguramos
                if (!string.IsNullOrEmpty(tarea.UsuarioEmail))
                {
                    tareaOriginal.UsuarioEmail = tarea.UsuarioEmail;
                }

                _context.SaveChanges();
            }
        }

        public void Eliminar(int id)
        {
            var tarea = _context.Tareas.Find(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                _context.SaveChanges();
            }
        }
    }
}