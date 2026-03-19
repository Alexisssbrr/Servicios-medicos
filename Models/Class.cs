using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioMedico.Models
{
    public class VisitaMedica
    {
        [Key]
        public int Id { get; set; }

        // --- Datos del Alumno ---
        [Required(ErrorMessage = "La matrícula es obligatoria")]
        public string Matricula { get; set; }

        // El atributo [NotMapped] le dice a Entity Framework que ignore estas propiedades 
        // al crear o actualizar la tabla en la base de datos, ya que estos datos 
        // vienen de PreinscripcionEntity, pero la vista los necesita para mostrarlos.
        [NotMapped]
        public string? NombreCompleto { get; set; }

        [NotMapped]
        public string? Carrera { get; set; }

        [Required]
        public DateTime FechaVisita { get; set; } = DateTime.Now;

        // --- Datos Físicos ---
        [NotMapped]
        public DateTime? FechaNacimiento { get; set; }

        public int Edad { get; set; } // Se calcula, pero lo guardamos por registro histórico
        public double? Talla { get; set; }
        public double? Peso { get; set; }

        // --- Antecedentes ---
        public bool TieneAlergias { get; set; }
        public string? EspecificarAlergia { get; set; }
        public string? EnfermedadesCronicas { get; set; }

        // --- Signos Vitales ---
        public string? FrecuenciaCardiaca { get; set; }
        public string? FrecuenciaRespiratoria { get; set; }
        public string? Saturacion { get; set; }
        public double? Temperatura { get; set; }
        public string? PresionArterial { get; set; }

        // --- Diagnóstico ---
        [Required(ErrorMessage = "El diagnóstico es obligatorio")]
        public string Diagnostico { get; set; }
    }
}