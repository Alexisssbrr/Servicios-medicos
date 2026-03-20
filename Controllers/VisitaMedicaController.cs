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

        [HttpPost]
        public async Task<IActionResult> Register(VisitaMedica model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            // 🔥 AQUÍ ES DONDE SE LLENAN LOS DATOS AUTOMÁTICOS
            if (!string.IsNullOrEmpty(model.Matricula))
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    var query = @"
                SELECT 
                    (dp.Nombre + ' ' + dp.ApellidoPaterno + ' ' + dp.ApellidoMaterno) AS NombreCompleto,
                    p.CarreraSolicitada AS Carrera
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
                        param.Value = model.Matricula;
                        command.Parameters.Add(param);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // 🔥 FORZAMOS los valores desde BD
                                model.NombreCompleto = reader["NombreCompleto"].ToString();
                                model.Carrera = reader["Carrera"].ToString();
                            }
                        }
                    }
                }
            }

            // Guardar en BD
            _context.Visitas.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


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
                p.CarreraSolicitada AS Carrera,
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

        public async Task<IActionResult> Index()
        {
            var lista = new List<VisitaMedica>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                // 1. Usamos LEFT JOIN en lugar de INNER JOIN.
                // 2. Usamos ISNULL y NULLIF para atrapar espacios en blanco o nulos directamente desde SQL.
                var query = @"
        SELECT 
            v.Matricula,
            ISNULL(NULLIF(LTRIM(RTRIM(dp.Nombre + ' ' + dp.ApellidoPaterno + ' ' + dp.ApellidoMaterno)), ''), 'FALTA NOMBRE EN BD') AS NombreCompleto,
            ISNULL(NULLIF(LTRIM(RTRIM(i.CarreraSolicitada)), ''), 'FALTA CARRERA EN BD') AS Carrera
        FROM Visitas v
        LEFT JOIN Inscripciones i ON v.Matricula = i.Matricula
        LEFT JOIN PreinscripcionDatosPersonales dp ON i.PreinscripcionId = dp.PreinscripcionId
        ";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new VisitaMedica
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


    }
}



