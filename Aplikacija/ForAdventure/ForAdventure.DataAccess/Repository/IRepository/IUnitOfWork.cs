namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IApplicationUserRepository ApplicationUser { get; }
        public IHotelRepository Hotel{ get; }
        public IRoomRepository Room{ get; }
        public IRoomTypeRepository RoomType{ get; }
        public IEquipmentRepository Equipment{ get; }
        public IEquipmentTypeRepository EquipmentType { get; }
        void Save();
    }
}
