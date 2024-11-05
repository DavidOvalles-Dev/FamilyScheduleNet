using FamilySchedule.Migrations;
using FamilySchedule.Models;
using FamilySchedule.Models.Context;
using FamilySchedule.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace FamilySchedule.Controllers.Registrado
{
    
    public class registrado : Controller
    {
        private readonly ApplicationDbContext _context;
        public registrado(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult IndexRegistrado()
        {
            return View();
        }
        //metodo que busca y agrega a los familiares
        [HttpPost]       
        public async Task<IActionResult> MainUser(string correoUsuarioInvitado)
        {
            var correoUsuario = HttpContext.Session.GetString("Correo");

            if (string.Equals(correoUsuario, correoUsuarioInvitado, StringComparison.OrdinalIgnoreCase))
            {
                TempData["MismoCorreo"] = "No te puedes auto agregar.";
                return View("BuscarFamiliares");
            }
            try
            {
                //busca al usuario en la bd

                var usuarioBd = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == correoUsuarioInvitado);

                if (usuarioBd == null)
                {
                    TempData["InvitacionFallida"] = "Esta dirección no está registrada";
                    return View("BuscarFamiliares");
                }

                if (usuarioBd.invitacionGrupo != true)
                {
                    var notificacionesFamiliares = new NotificacionesModel
                    {
                        usuarioId = usuarioBd.Id,
                        Tipo = 1,
                        Mensaje = correoUsuario + " te está invitando a unirte a su grupo familiar",
                        Fecha = DateTime.Now,
                        UsuarioCorreo = correoUsuarioInvitado,
                        Admin = correoUsuario
                    };

                    // Actualizar directamente el objeto usuarioBd
                    usuarioBd.Admin2 = correoUsuario;
                    usuarioBd.invitacionGrupo = true;

                    // Agregar la notificación y actualizar el usuario
                    _context.Notificaciones.Add(notificacionesFamiliares);
                    _context.Usuarios.Update(usuarioBd);
                    await _context.SaveChangesAsync();

                    TempData["AlertMessage"] = "Invitación enviada exitosamente";
                }
                else
                {
                    TempData["agrupado"] = "Este usuario ya forma parte de un grupo familiar";
                    return View("BuscarFamiliares");
                }
            }
            catch (Exception)
            {
                TempData["errorInvitacion"] = "Ocurrió un error al enviar la invitación";
            }
            return View("BuscarFamiliares");
    }

        public async Task<IActionResult> cerrarSeccionAsync()
        {
            HttpContext.Session.Clear();


            return RedirectToAction("Index", "Evento");
        }
        public IActionResult BuscarFamiliares()
        {
            return View();
        }
        public async Task<IActionResult> AceptarInvitacion(int id)
        {
            var buscarInvitacionBd = await _context.Notificaciones.FindAsync(id);
            var correoUsuario = HttpContext.Session.GetString("Correo");
            var buscarUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correoUsuario);

            try
            {
                buscarUsuario.invitacionGrupo = false;
                buscarUsuario.Admin2 = buscarInvitacionBd.Admin;
                buscarInvitacionBd.Admin = correoUsuario;

                _context.Notificaciones.Update(buscarInvitacionBd);
                _context.Usuarios.Update(buscarUsuario);

                await _context.SaveChangesAsync();             
            }
            catch (Exception ex) {

                TempData["noF"] = "Error inesperado";
                return View("IndexRegistrado");
            }

            TempData["Exito"] = "La invitación ha sido aceptada exitosamente.";
            return RedirectToAction("IndexRegistrado");
        }
        public async Task<IActionResult> RechazarInvitacion(int id) { 

            var correoUsuario = HttpContext.Session.GetString("Correo");
            var buscarInvitacionBd = await _context.Notificaciones.FindAsync(id);
            var buscarUsuarioBd = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correoUsuario);
            
            if (buscarInvitacionBd != null)
            {
                
                _context.Notificaciones.Remove(buscarInvitacionBd);
                buscarUsuarioBd.invitacionGrupo = false;
                
                _context.Usuarios.Update(buscarUsuarioBd);
                await _context.SaveChangesAsync();
                //TODO no funcionan las alertas
                TempData["Rechazado"] = "Rechazado";

            }
            else {

                TempData["Inesperado"] = "error inesperado";
                return View();
            }
            
            return RedirectToAction("IndexRegistrado");
            
        }
        // metodo que  muestra las notificaciones que cada usuario tiene
        [HttpGet]
        public async Task<IActionResult> IndexRegistrado(Usuario usuario)
        {
            var correoUsuario = HttpContext.Session.GetString("Correo");

            //Verifica si el correo está disponible
            if (string.IsNullOrEmpty(correoUsuario))
            {
                TempData["error"] = "Correo no disponible en la sesión.";
                return RedirectToAction("Index", "Evento");
            }

            // Busca el usuario en la base de datos
            var buscarUsuarioBd = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correoUsuario);
            
            if(buscarUsuarioBd.invitacionGrupo != false)
            {
                var notificaciones = await _context.Notificaciones
                    .Where(n => n.UsuarioCorreo == correoUsuario)
                    .ToListAsync();

                if (!notificaciones.Any())
                {
                    TempData["NoNotificaciones"] = "No tienes notificaciones pendientes.";
                }
                // Retorna la vista con el modelo
                return View("IndexRegistrado", notificaciones);
            }
             
               return View();
        }
            
    }
            
 }





