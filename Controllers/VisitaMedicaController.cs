using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioMedico.Data; // Asegúrate de que esto coincida con tu namespace
using ServicioMedico.Models;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ServicioMedico.Models;


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
        [HttpGet]
        [HttpGet]
        [HttpGet]
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> ObtenerDatosAlumno(string matricula)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return BadRequest("Matrícula vacía");
            }

            AlumnoDTO alumno = null;

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                var query = @"
            SELECT 
                (dp.Nombre + ' ' + dp.ApellidoPaterno + ' ' + dp.ApellidoMaterno) AS NombreCompleto,
                i.CarreraSolicitada AS Carrera,
                CONVERT(varchar, dp.FechaNacimiento, 23) AS FechaNacimiento
            FROM Inscripciones i
            INNER JOIN Preinscripciones p ON i.PreinscripcionId = p.Id
            INNER JOIN PreinscripcionDatosPersonales dp ON dp.PreinscripcionId = p.Id
            WHERE i.Matricula = @matricula
        ";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    var param = command.CreateParameter();
                    param.ParameterName = "@matricula";
                    param.Value = matricula;
                    command.Parameters.Add(param);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            alumno = new AlumnoDTO
                            {
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Carrera = reader["Carrera"].ToString(),
                                FechaNacimiento = reader["FechaNacimiento"].ToString()
                            };
                        }
                    }
                }
            }

            if (alumno == null)
            {
                return NotFound("No se encontró el alumno");
            }

            return Json(new
            {
                nombreCompleto = alumno.NombreCompleto,
                carrera = alumno.Carrera,
                fechaNacimiento = alumno.FechaNacimiento
            });
        }
    }
}



