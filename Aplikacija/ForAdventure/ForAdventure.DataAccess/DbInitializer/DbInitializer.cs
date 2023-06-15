using Microsoft.AspNetCore.Identity;
using ForAdventure.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {

            }

            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "foradventure2023@gmail.com",
                    Email = "foradventure2023@gmail.com",
                    Name = "Predrag",
                    NameInApplication = "Predrag",
                    LastName = "Tosic",
                    PhoneNumber = "11111111",
                    Address = "Bulevar Nikole Tesle 15",
                    City = "Nis",
                    EmailConfirmed = true,
                }, "Admin1?").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "foradventure2023@gmail.com");

                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }
            return;
        }
    }
}
