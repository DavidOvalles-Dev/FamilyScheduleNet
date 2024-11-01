namespace FamilySchedule.Models.ViewModel
{
    public class IndexRegistradoViewModel
    {
        public Usuario Usuario { get; set; }
        public IEnumerable<NotificacionesModel> Notificaciones { get; set; } = new List<NotificacionesModel>();
    }
}
