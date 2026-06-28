namespace PriorityGrad.domain.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Materia { get; set; } = string.Empty;
        public int Valor { get; set; }
        public int Dificultad { get; set; }
        public DateOnly Fecha { get; set; }
    }
}