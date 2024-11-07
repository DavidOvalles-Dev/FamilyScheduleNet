using FamilySchedule.Migrations;
using FamilySchedule.Models;
using FamilySchedule.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace FamilySchedule.Controllers
{
    public class EventoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventoController(ApplicationDbContext context)
        {
            _context = context;
        }

        //metodo que retorna la lista de eventos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Eventos.ToListAsync());
        }

        //Metodos Get y post para crear un evento
        [HttpGet]
        public IActionResult Crear()
        {

            return PartialView("_Crear");
        }
        public async Task<IActionResult> Crear(EventArgs evento)
        {

            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] = "Evento Creado exitosamente";
                return Json(new { success = true, message = "Evento creado exitosamente." });
            }

            return PartialView("_Crear", evento);

        }

        //metodo que valida que el evento que se edite exista mediante el id
        public async Task<IActionResult> Editar(int id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var eventInfoFound = await _context.Eventos.FindAsync(id);

            if(eventInfoFound == null)
            {
                return NotFound();
            }

            return View(eventInfoFound);
        }

        //metodo que edita
        [HttpPost]
        public async Task<IActionResult> Editar(EventoModel evento)
        {
            if (ModelState.IsValid)
            {
                var eventoEncontrado = await _context.Eventos.FindAsync(evento.Id);
                if(eventoEncontrado != null)
                {
                    eventoEncontrado.Titulo = evento.Titulo;
                    eventoEncontrado.Creador = evento.Creador;
                    eventoEncontrado.Fecha = evento.Fecha;
                    eventoEncontrado.Descripcion = evento.Descripcion;

                    _context.Update(eventoEncontrado);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "Evento Actualizado exitosamente";
                    return RedirectToAction("Index");
                    
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                var eventoEncontrado = await _context.Eventos.FindAsync(id);

                if (eventoEncontrado == null || id == null)
                {
                    return NotFound();
                }             
                   
                _context.Eventos.Remove(eventoEncontrado);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Evento Eliminado exitosamente";

                return RedirectToAction("Index");

            }
            
            return NotFound();
        }

        public async Task<IActionResult> GetEventos()
        {
            var eventos = await _context.Eventos
                .Select(e => new {
                    id = e.Id,
                    title = e.Titulo,
                    start = e.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"), // Formato ISO 8601
                    description = e.Descripcion
                }).ToListAsync();
            return Json(eventos);
        }

    }
}
