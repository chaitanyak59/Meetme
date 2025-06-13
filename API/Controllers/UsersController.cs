using API.Data;
using API.Repository;
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
        private readonly IUserRepository _userRepository;

        public Users(IUserRepository repository, ILogger<Users> logger)
        {
            _logger = logger;
            _userRepository = repository;
        }

        [HttpGet()]
        public async Task<ActionResult> AllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (null == user)
            {
                return NotFound("Cannot be found");
            }
            return Ok(user);
        }
    }
}
