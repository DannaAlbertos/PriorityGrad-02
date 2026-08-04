using System.Collections.Generic;

namespace PriorityGrad.domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;

        // Propiedades que tu AuthController necesita:
        public string Institucion { get; set; } = string.Empty;
        public string CarreraGrado { get; set; } = string.Empty;

        public string? FotoPerfil { get; set; }

        // Relación con las materias configuradas
        public virtual ICollection<MateriaConfig> Materias { get; set; } = new List<MateriaConfig>();
    }
}