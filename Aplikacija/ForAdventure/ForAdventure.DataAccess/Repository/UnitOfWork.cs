using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;

namespace ForAdventure.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IHotelRepository Hotel { get; private set; }
        public IEquipmentRepository Equipment { get; set; }
        public IEquipmentTypeRepository EquipmentType { get; set; }
        public IRoomRepository Room { get; private set; }
        public IRoomTypeRepository RoomType { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Hotel = new HotelRepository(_db);
            Equipment = new EquipmentRepository(_db);
            EquipmentType = new EquipmentTypeRepository(_db);
            Room = new RoomRepository(_db); 
            RoomType = new RoomTypeRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
