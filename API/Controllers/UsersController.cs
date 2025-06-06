using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class Users : BaseApiController
    {
        private readonly ILogger<Users> _logger;
        private readonly MeetMeDBContext context;

        public Users(MeetMeDBContext _context, ILogger<Users> logger)
        {
            context = _context;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult> AllUsers()
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (null == user)
            {
                return NotFound("Cannot be found");
            }
            return Ok(user);
        }
    }
}
