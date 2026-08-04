using Microsoft.EntityFrameworkCore;
using PriorityGrad.domain.Models;

namespace PriorityGrad.infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<MateriaConfig> MateriaConfig { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamos tu clase MateriaConfig a la tabla existente en PostgreSQL
            modelBuilder.Entity<MateriaConfig>().ToTable("MateriaConfigurada");

            // Mapeamos la tabla de Tareas en singular tal como está en tu base de datos
            modelBuilder.Entity<Tarea>().ToTable("Tarea");
        }
    }
}