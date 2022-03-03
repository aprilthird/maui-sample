using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApiSample.Data.Seeder
{
    public class DataSeeder : IDataSeeder
    {
        public async Task Initialize(ApplicationDbContext db)
        {
            if(!db.AppUsers.Any())
            {
                var users = new List<AppUser>()
                {
                    new AppUser("Ram", "ram@gmail.com", 20, DateTime.Parse("19-09-2001")),
                    new AppUser("Asim", "asim@gmail.com", 19, DateTime.Parse("13-05-2002")),
                    new AppUser("Gali", "gali@gmail.com", 22, DateTime.Parse("08-11-1999")),
                };
                await db.AppUsers.AddRangeAsync(users);
                await db.SaveChangesAsync();
            }
        }
    }
}
