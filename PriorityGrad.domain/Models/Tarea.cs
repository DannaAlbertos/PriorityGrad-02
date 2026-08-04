namespace PriorityGrad.domain.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string UsuarioEmail { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Materia { get; set; } = string.Empty;
        public int Valor { get; set; }
        public int Dificultad { get; set; }
        public DateOnly Fecha { get; set; }
        public string? Comentario { get; set; }
        public string? NombreProfesor { get; set; }
        public string? Instrucciones { get; set; }
        public TimeOnly? HoraEntrega { get; set; }
        public string? ImagenUrl { get; set; }
        public string? ColorHex { get; set; } = "#d4a5a5";
        public string? ColorTexto { get; set; } = "#ffffff";
    }
}