using ForAdventure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForAdventure.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        { 
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
		public DbSet<Report> Reports { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<EquipmentReservation> EquipmentReservations { get; set; }
		public DbSet<ActivityReservation> ActivityReservations { get; set; }
	}
}
