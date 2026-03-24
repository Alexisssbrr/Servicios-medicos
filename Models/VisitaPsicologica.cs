using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioMedico.Models
{
    public class VisitaPsicologica
    {
        [Key]
        public int Id { get; set; }

        // --- Datos del Alumno ---
        [Required(ErrorMessage = "La matrícula es obligatoria")]
        public string Matricula { get; set; }

        [NotMapped]
        public string? NombreCompleto { get; set; }

        [NotMapped]
        public string? Carrera { get; set; }

        [Required]
        public DateTime FechaVisita { get; set; } = DateTime.Now;

        [NotMapped]
        public DateTime? FechaNacimiento { get; set; }

        public int Edad { get; set; }

        // --- Antecedentes Psicológicos (Adaptado) ---
        public bool TerapiaPrevia { get; set; }
        public string? MotivoConsultaPrevia { get; set; }
        public string? MedicacionPsiquiatrica { get; set; }

        // --- Diagnóstico / Motivo de Consulta actual ---
        [Required(ErrorMessage = "El motivo de consulta es obligatorio")]
        public string MotivoConsulta { get; set; }
    }
}