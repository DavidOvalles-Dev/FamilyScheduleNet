using System.Linq.Expressions;

namespace FamilySchedule.Models.ViewModel
{
    public class EventoNotificacionesViewModel
    {
        public List<NotificacionesModel> Notificaciones { get; set; } = new List<NotificacionesModel>();
        public List<EventoModel> Evento { get; set; } = new List<EventoModel>();
    }
}
