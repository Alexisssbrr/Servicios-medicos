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

                // Esta consulta busca la última visita de cada matrícula y jala el nombre y carrera de las otras tablas
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
                                // Agrega aquí los demás campos que necesites mostrar (Edad, Diagnostico, etc.)
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
            // ASIGNACIÓN AUTOMÁTICA DE FECHA Y HORA
            visita.FechaVisita = DateTime.Now;

            // Removemos la validación de FechaVisita porque la estamos asignando manualmente aquí
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}