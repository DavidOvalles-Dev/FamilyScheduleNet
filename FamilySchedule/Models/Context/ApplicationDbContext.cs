using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FamilySchedule.Models.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)       
        {           
        }

        //Agregando los modelos a la base de datos
        public DbSet<EventoModel> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<NotificacionesModel> Notificaciones { get; set; }
        public DbSet<TiposDeNotificaciones> TiposDeNotificaciones { get; set; }

    }
}
