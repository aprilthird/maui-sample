using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMAUISample.Services
{
    public interface IApiService
    {
        public Task<List<AppUser>> GetAllAsync(string? search = null);
        public Task SaveAsync(AppUser user);
        public Task DeleteAsync(AppUser user);
    }
}
