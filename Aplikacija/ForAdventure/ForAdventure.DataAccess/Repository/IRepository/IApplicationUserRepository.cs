using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository :IRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);
        void Save();
    }
}
