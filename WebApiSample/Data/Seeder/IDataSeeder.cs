using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApiSample.Data.Seeder
{
    public interface IDataSeeder
    {
        public Task Initialize(ApplicationDbContext db);
    }
}
