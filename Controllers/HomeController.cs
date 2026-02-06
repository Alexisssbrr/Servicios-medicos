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
            // Verifica que la tabla Visitas exista gracias al paso 1
            var alumnos = await _context.Visitas
                .GroupBy(v => v.Matricula)
                .Select(g => g.OrderByDescending(v => v.FechaVisita).First())
                .ToListAsync();

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
    } // Cierre de la clase
} // Cierre del namespace