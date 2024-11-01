using System.ComponentModel.DataAnnotations;

namespace FamilySchedule.Models
{
    public class NotificacionesModel
    {
        [Key]
        public int Id { get; set; }
        public int Tipo { get; set; }  // Invitacion, Tarea, Evento
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioCorreo { get; set; }
        public string Admin {  get; set; }


        // relacion con el usuario

        public int usuarioId {  get; set; }
        public Usuario Usuario { get; set; }
        
    }
}
