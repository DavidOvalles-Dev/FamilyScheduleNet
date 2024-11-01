using System.ComponentModel.DataAnnotations;

namespace FamilySchedule.Models
{
    public class TiposDeNotificaciones
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TipoDeNotificacion { get; set; }
       
    }
}
