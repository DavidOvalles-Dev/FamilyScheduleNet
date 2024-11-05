namespace FamilySchedule.Models.ViewModel
{
    public class EventoNotificacionesViewModel
    {
        public List<NotificacionesModel> Notificaciones { get; set; } = new List<NotificacionesModel>();
        public EventoModel Evento { get; set; }
    }
}
