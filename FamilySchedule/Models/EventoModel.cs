using System.ComponentModel.DataAnnotations;

namespace FamilySchedule.Models
{
    public class EventoModel
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Este campo es requerido")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Creador { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

    }
}
