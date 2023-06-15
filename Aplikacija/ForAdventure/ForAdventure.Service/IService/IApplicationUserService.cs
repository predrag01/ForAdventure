using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IApplicationUserService
    {
        public IApplicationUserRepository ApplicationUser { get; }
        ApplicationUser GetUserByEmail(string email);
        string GetImageURLByEmail(string email);
        string GetUsernameByEmail(string email);
        bool DeletePicture(string image);
        ApplicationUser GetUserById(string id);
        string? GetIdByEmail(string email);
        string GetProfilePictureById(string id);
        string GetUsernameById(string id);
        string GetEmailByUserId(string id);
        void RemoveUser(string id);
    }
}
