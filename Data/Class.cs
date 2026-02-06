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
    }
}