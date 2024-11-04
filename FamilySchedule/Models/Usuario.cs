using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilySchedule.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string NombreDeUsuario { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Contraseña { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string? ConfirmarContraseña { get; set; }
        public bool invitacionGrupo { get; set; }
        public string? Admin2 { get; set; }
        //Relacion con las notificaciones

        public ICollection<NotificacionesModel> Notificaciones { get; set; } = new List<NotificacionesModel>();

    }

    
}
