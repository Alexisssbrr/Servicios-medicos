using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioMedico.Data;
using ServicioMedico.Models;
using System.Diagnostics;

namespace ServicioMedico.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = new List<VisitaMedica>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                var query = @"
            SELECT v.*, 
                   (dp.Nombre + ' ' + dp.ApellidoPaterno + ' ' + dp.ApellidoMaterno) AS NombreCompleto,
                   i.CarreraSolicitada AS Carrera
            FROM Visitas v
            INNER JOIN (
                SELECT Matricula, MAX(FechaVisita) as MaxFecha
                FROM Visitas
                GROUP BY Matricula
            ) latest ON v.Matricula = latest.Matricula AND v.FechaVisita = latest.MaxFecha
            LEFT JOIN Inscripciones i ON v.Matricula = i.Matricula
            LEFT JOIN PreinscripcionDatosPersonales dp ON i.PreinscripcionId = dp.PreinscripcionId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            alumnos.Add(new VisitaMedica
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Matricula = reader["Matricula"].ToString(),
                                NombreCompleto = reader["NombreCompleto"]?.ToString() ?? "No encontrado",
                                Carrera = reader["Carrera"]?.ToString() ?? "No encontrada",
                                FechaVisita = Convert.ToDateTime(reader["FechaVisita"])
                            });
                        }
                    }
                }
            }

            return View(alumnos);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VisitaMedica visita)
        {
            visita.FechaVisita = DateTime.Now;
            ModelState.Remove("FechaVisita");

            if (ModelState.IsValid)
            {
                _context.Add(visita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(visita);
        }

        public async Task<IActionResult> History(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var historial = await _context.Visitas
                .Where(v => v.Matricula == id)
                .OrderByDescending(v => v.FechaVisita)
                .ToListAsync();

            ViewData["Matricula"] = id;
            return View(historial);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // ==========================================================
        // NUEVA SECCIÓN: GESTIÓN DE ROLES DE USUARIOS
        // ==========================================================

        public async Task<IActionResult> GestionUsuarios()
        {
            var modelo = new UsuariosViewModel();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                // Traemos los usuarios y el nombre de su rol mediante un JOIN
                var query = @"
                    SELECT u.management_user_ID, u.management_user_Username, u.management_user_Email, r.management_user_RoleID 
                    FROM management_user_table u
                    LEFT JOIN management_user_table r ON u.management_user_RoleID = r.management_user_ID";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            modelo.Users.Add(new UserDetalle
                            {
                                Id = Convert.ToInt32(reader["management_user_ID"]),
                                Nombre = reader["management_user_Username"].ToString(),
                                Email = reader["management_user_Email"].ToString(),
                                Rol = Convert.ToString(reader["management_user_RoleID"]).Trim() switch
                                {
                                    "1" => "Administrador",
                                    "2" => "Alumno",
                                    "3" => "Docente",
                                    "4" => "Jefe de Enfermería",
                                    "5" => "Enfermero",
                                    "6" => "Psicólogo",
                                    "1002" => "Administrativo",
                                    "" => "Sin Rol"

                                }
                            });
                        }
                    }
                }
            }
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarRol(int usuarioId, int nuevoRolId)
        {
            // Query directa para actualizar el RolId del usuario seleccionado
            var query = "UPDATE management_user_table SET management_user_RoleID = @rolId WHERE management_user_ID = @userId"; 

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    var p1 = command.CreateParameter();
                    p1.ParameterName = "@rolId";
                    p1.Value = nuevoRolId;
                    command.Parameters.Add(p1);

                    var p2 = command.CreateParameter();
                    p2.ParameterName = "@userId";
                    p2.Value = usuarioId;
                    command.Parameters.Add(p2);

                    await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction(nameof(GestionUsuarios));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}