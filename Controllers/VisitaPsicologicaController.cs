using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioMedico.Data;
using ServicioMedico.Models;
using System.Threading.Tasks;

namespace ServicioMedico.Controllers
{
    public class VisitaPsicologicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitaPsicologicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vista del Formulario
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Guardar la Visita
        [HttpPost]
        public async Task<IActionResult> Register(VisitaPsicologica model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Guardar en BD
            model.FechaVisita = DateTime.Now;
            _context.VisitasPsicologicas.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Directorio
        public async Task<IActionResult> Index()
        {
            var lista = new List<VisitaPsicologica>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                // Usamos la misma consulta SQL robusta que ya tienes
                var query = @"
                SELECT 
                    v.Matricula,
                    ISNULL(NULLIF(LTRIM(RTRIM(dp.Nombre + ' ' + dp.ApellidoPaterno + ' ' + dp.ApellidoMaterno)), ''), 'FALTA NOMBRE EN BD') AS NombreCompleto,
                    ISNULL(NULLIF(LTRIM(RTRIM(i.CarreraSolicitada)), ''), 'FALTA CARRERA EN BD') AS Carrera
                FROM VisitasPsicologicas v
                LEFT JOIN Inscripciones i ON v.Matricula = i.Matricula
                LEFT JOIN PreinscripcionDatosPersonales dp ON i.PreinscripcionId = dp.PreinscripcionId
                GROUP BY v.Matricula, dp.Nombre, dp.ApellidoPaterno, dp.ApellidoMaterno, i.CarreraSolicitada";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new VisitaPsicologica
                            {
                                Matricula = reader["Matricula"].ToString(),
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Carrera = reader["Carrera"].ToString()
                            });
                        }
                    }
                }
            }
            return View(lista);
        }

        // API para llenar datos automáticos (Reutilizando tu lógica DTO)
        [HttpGet]
        public async Task<IActionResult> ObtenerDatosAlumno(string matricula)
        {
            if (string.IsNullOrEmpty(matricula)) return BadRequest("Matrícula vacía");

            AlumnoDTO alumno = null;
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                var query = @"
                SELECT 
                    (dp.Nombre + ' ' + dp.ApellidoPaterno + ' ' + dp.ApellidoMaterno) AS NombreCompleto,
                    p.CarreraSolicitada AS Carrera,
                    CONVERT(varchar, dp.FechaNacimiento, 23) AS FechaNacimiento
                FROM Inscripciones i
                INNER JOIN Preinscripciones p ON i.PreinscripcionId = p.Id
                INNER JOIN PreinscripcionDatosPersonales dp ON dp.PreinscripcionId = p.Id
                WHERE i.Matricula = @matricula";

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
            if (alumno == null) return NotFound("No se encontró el alumno");

            return Json(new { nombreCompleto = alumno.NombreCompleto, carrera = alumno.Carrera, fechaNacimiento = alumno.FechaNacimiento });
        }

        public async Task<IActionResult> History(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var historial = await _context.VisitasPsicologicas
                .Where(v => v.Matricula == id)
                .OrderByDescending(v => v.FechaVisita)
                .ToListAsync();

            ViewData["Matricula"] = id;

            return View(historial);
        }


    }

}