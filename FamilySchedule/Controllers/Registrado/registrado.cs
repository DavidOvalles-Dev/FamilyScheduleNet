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

            if(string.Equals(correoUsuario, correoUsuarioInvitado, StringComparison.OrdinalIgnoreCase))
            {
                TempData["MismoCorreo"] = "No te puedes auto agregar. ";
                return View("BuscarFamiliares");
            }
            try
            {
                var usuarioBd = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correoUsuarioInvitado);

                if (usuarioBd == null)
                {
                    TempData["InvitacionFallida"] = "Esta direccion no esta registrada";
                    return View("BuscarFamiliares");
                }
                    
                if(usuarioBd.invitacionGrupo != true)
                {
                    var notificacionesFamiliares = new NotificacionesModel
                    {

                        usuarioId = usuarioBd.Id,
                        Tipo = 1,
                        Mensaje = correoUsuario + " te esta invitando a unirte a su grupo familiar",
                        Fecha = DateTime.Now,
                        UsuarioCorreo = correoUsuarioInvitado,
                        Admin = correoUsuario
                    };

                    var agregarDatosUsuario = new Usuario
                    {
                        Admin = correoUsuario,
                        invitacionGrupo = true,
                        Apellido = usuarioBd.Apellido,
                    };

                    //TO DO priority = 2/10 = por ahora se pueden mandar varias notificaciones repetidas, 
                    //hacer que se valide el tipo de invitacion ya que si es familiar no es necesario mandar mas de una

                    _context.Notificaciones.Add(notificacionesFamiliares);
                    _context.Usuarios.Update(agregarDatosUsuario);

                    usuarioBd.Admin = correoUsuario;

                    _context.Usuarios.Update(usuarioBd);
                    await _context.SaveChangesAsync();

                    TempData["AlertMessage"] = "Invitacion enviada exitosamente";
                }
                else
                {
                    TempData["agrupado"] = "Este usuario ya forma parte de un grupo familiar";
                    return View("BuscarFamiliares");
                }
         
                    
            }
            catch (Exception)
            {
                TempData["errorInvitacion"] = "Ocurrio un error al enviar la invitacion";
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
                buscarUsuario.invitacionGrupo = true;
                buscarUsuario.Admin2 = buscarInvitacionBd.Admin;
                buscarInvitacionBd.Admin = correoUsuario;

                _context.Notificaciones.Update(buscarInvitacionBd);
                _context.Usuarios.Update(buscarUsuario);

                await _context.SaveChangesAsync();

                TempData["Exito"] = "La invitación ha sido aceptada exitosamente.";
            }
            catch (Exception ex) {

                TempData["noF"] = "Error inesperado";
                return View("IndexRegistrado");
            }

            return View("IndexRegistrado");
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
            var buscarUsuarioBd = await _context.Notificaciones.FirstOrDefaultAsync(u => u.UsuarioCorreo == correoUsuario);
            
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
            
    }
            
 }





