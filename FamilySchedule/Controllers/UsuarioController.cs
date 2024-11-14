using FamilySchedule.Migrations;
using FamilySchedule.Models;
using FamilySchedule.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;

namespace FamilySchedule.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
    
        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult IniciarSeccion()
        {
            return View();
        }
        //metodo que valida el uusario
        [HttpPost]
        public async Task<IActionResult> IniciarSeccion(string correo, string clave)
        {       
            //asi se valida que el correo que se inserte sea el mismo que el correo que esta en la base de datos
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);

            // Verifica si el usuario existe y si la clave coincide
            if (usuario == null || usuario.Contraseña != clave)
            {
                TempData["errorUsuario"] = "El correo o la clave son incorrectos";
                return View("IniciarSeccion");
            }
            //guardar informacion del usuario para utilizarla en cualquier clase 

            HttpContext.Session.SetString("Correo", usuario.Correo);
            HttpContext.Session.SetString("Clave", usuario.Contraseña);

            TempData["InicioAlert"] = true;

            return RedirectToAction("IndexRegistrado", "Registrado");

        }
        //metodos para crear usuario y vista
        public IActionResult RegistrarUsuario(){ 
            
               return View();
        }
        public async Task<IActionResult> CrearUsuario(Usuario solicitudCrearUsuario)
        {

            //sacando el correo y la clave para el inicio de sesion

            string InicioCorreo = solicitudCrearUsuario.Correo;
            string InicioClave = solicitudCrearUsuario.Contraseña;


            //lo agrego a la variable para sacar la accion
            var resultado  =  await IniciarSeccion(InicioCorreo, InicioClave);

            #region validaciones
            if (string.IsNullOrEmpty(solicitudCrearUsuario.Nombre))
            {
                TempData["noNull"] = "Debe llenar todos los campos";
                return View("RegistrarUsuario", solicitudCrearUsuario);
            }
            if (string.IsNullOrEmpty(solicitudCrearUsuario.Apellido))
            {
                TempData["noNull"] = "Debe llenar todos los campos";
                return View("RegistrarUsuario", solicitudCrearUsuario);
            }
            if (string.IsNullOrEmpty(solicitudCrearUsuario.Correo))
            {
                TempData["noNull"] = "Debe llenar todos los campos";
                return View("RegistrarUsuario", solicitudCrearUsuario);
            }
            if (string.IsNullOrEmpty(solicitudCrearUsuario.NombreDeUsuario))
            {
                TempData["noNull"] = "Debe llenar todos los campos";
                return View("RegistrarUsuario", solicitudCrearUsuario);
            }
            if (string.IsNullOrEmpty(solicitudCrearUsuario.Contraseña))
            {
                TempData["noNull"] = "Debe llenar todos los campos";
                return View("RegistrarUsuario", solicitudCrearUsuario);
            }
            if (string.IsNullOrEmpty(solicitudCrearUsuario.ConfirmarContraseña))
            {
                TempData["noNull"] = "Debe llenar todos los campos";
                return View("RegistrarUsuario", solicitudCrearUsuario);
            }
            #endregion


            
            if (solicitudCrearUsuario.Contraseña != solicitudCrearUsuario.ConfirmarContraseña)
            {
                TempData["ContraseñaIncorrecta"] = "password must be similar";
                return View("RegistrarUsuario", solicitudCrearUsuario);

            }
            // Validación del estado del modelo y existencia del correo en la base de datos
            if (ModelState.IsValid)
            {
                var existeUsuario = await _context.Usuarios
                    .AnyAsync(u => u.Correo == solicitudCrearUsuario.Correo);

                if (existeUsuario)
                {
                    TempData["CorreoExistente"] = "Este correo ya existe";
                    return View("RegistrarUsuario", solicitudCrearUsuario);
                }

                // Guardar el usuario en la base de datos
                _context.Add(solicitudCrearUsuario);
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] = "Su usuario se creó exitosamente";

                // Iniciar sesión automáticamente después de crear el usuario
                return await IniciarSeccion(solicitudCrearUsuario.Correo, solicitudCrearUsuario.Contraseña);
            }
            else {
                TempData["noNull"] = "Error inesperado";
            }
            //TO DO: redireccionar al inicio de seccion
            return View("RegistrarUsuario", solicitudCrearUsuario);
        }

      
    }
}
