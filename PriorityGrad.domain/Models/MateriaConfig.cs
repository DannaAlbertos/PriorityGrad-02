namespace PriorityGrad.domain.Models
{
    public class MateriaConfig
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Propiedad agregada para evitar el error de base de datos
        public string NombreProfesor { get; set; } = string.Empty;

        public string ColorHex { get; set; } = string.Empty;
        public string ColorTexto { get; set; } = string.Empty;

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}