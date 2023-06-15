using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.DataAccess.Repository;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using ForAdventure.DataAccess.DbInitializer;

namespace ForAdventure.Service.Utility
{
    public class ServiceRegistrator
    {
        public void GiveScope(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
            builder.Services.AddScoped<IEquipmentService, EquipmentService>();
            builder.Services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IActivityTypeService, ActivityTypeService>();
            builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddScoped<IActivityReservationService, ActivityReservationService>();
            builder.Services.AddScoped<IEquipmentReservationService, EquipmentReservationService>();
            builder.Services.AddScoped<IRoomReservationService, RoomReservationService>();
            ///////////////////////////////////////////////////////////////////////////////////////
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            builder.Services.AddScoped<IEquipmentTypeRepository, EquipmentTypeRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IActivityReservationRepository, ActivityReservationRepository>();
            builder.Services.AddScoped<IEquipmentReservationRepository, EquipmentReservationRepository>();
            builder.Services.AddScoped<IRoomReservationRepository, RoomReservationRepository>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
        }
    }
}
