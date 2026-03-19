using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioMedico.Data; // Asegúrate de que esto coincida con tu namespace
using ServicioMedico.Models;
using System.Threading.Tasks;

namespace ServicioMedico.Controllers
{
    public class VisitaMedicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        // El constructor recibe la base de datos
        public VisitaMedicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Esta acción muestra la vista (tu formulario HTML)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
            // Esto buscará el archivo Views/VisitaMedica/Create.cshtml
        }

        // 2. Esta es la API que tu JavaScript va a llamar para traer los datos
        [HttpGet]
        public async Task<IActionResult> ObtenerDatosAlumno(string matricula)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return BadRequest("Matrícula vacía");
            }

            // Buscamos en tu base de datos (DbSet Preinscripcion)
            var alumno = await _context.Preinscripcion
                .Include(p => p.DatosPersonales)
                .FirstOrDefaultAsync(p => p.Folio == matricula);

            if (alumno == null || alumno.DatosPersonales == null)
            {
                return NotFound(); // Retorna 404 si no existe
            }

            // Armamos el JSON que espera tu JavaScript
            var resultado = new
            {
                nombreCompleto = $"{alumno.DatosPersonales.Nombre} {alumno.DatosPersonales.ApellidoPaterno} {alumno.DatosPersonales.ApellidoMaterno}".Trim(),
                carrera = alumno.CarreraSolicitada,
                // Formato ISO para el input type="date"
                fechaNacimiento = alumno.DatosPersonales.FechaNacimiento.ToString("yyyy-MM-dd")
            };

            return Json(resultado);
        }
    }
}