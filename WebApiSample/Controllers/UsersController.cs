using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Data;

namespace WebApiSample.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ApplicationDbContext DbContext { get; set; }

        public UsersController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search)
        {
            var query = DbContext.AppUsers.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search));
            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await DbContext.AppUsers.FirstOrDefaultAsync(x => x.Email == user.Email) != null)
                return BadRequest("Email is already taken.");
            try
            {
                await DbContext.AppUsers.AddAsync(user);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Created($"{Request.Scheme}://{Request.Host.Value}/", user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AppUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var similarUser = await DbContext.AppUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (similarUser != null && similarUser.Id != id)
                return BadRequest("Email is already taken.");
            var userInDb = await DbContext.AppUsers.FindAsync(id);
            if(userInDb == null)
                return NotFound("User does not exist.");
            try
            {
                userInDb.Name = user.Name;
                userInDb.Email = user.Email;
                userInDb.Age = user.Age;
                userInDb.BirthDate = user.BirthDate;
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await DbContext.AppUsers.FindAsync(id);
            if (user == null)
                return NotFound("User does not exist.");
            try
            {
                DbContext.AppUsers.Remove(user);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
