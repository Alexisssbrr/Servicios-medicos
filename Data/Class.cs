using Microsoft.EntityFrameworkCore;
using ServicioMedico.Models;

namespace ServicioMedico.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VisitaMedica> Visitas { get; set; }
        public DbSet<VisitaPsicologica> VisitasPsicologicas { get; set; }

        // Esta es tu tabla Inscripciones
        public DbSet<PreinscripcionEntity> Inscripciones { get; set; }

        // Esta es la tabla de datos personales
        public DbSet<PreinscripcionDatosPersonalesEntity> DatosPersonales { get; set; }
    }

}

