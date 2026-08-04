using System.Collections.Generic;

namespace PriorityGrad.web.Models
{
    public static class AppData
    {
        // Diccionario seguro para guardar los datos del usuario por su Correo (Clave única)
        public static Dictionary<string, UsuarioInfo> UsuariosRegistrados = new(System.StringComparer.OrdinalIgnoreCase);
    }

    public class UsuarioInfo
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Institucion { get; set; }
        public string Carrera { get; set; }
        public List<MateriaInfo> Materias { get; set; } = new();
        public string FotoPerfil { get; set; }
    }

    public class MateriaInfo
    {
        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public string Color { get; set; }
    }
}