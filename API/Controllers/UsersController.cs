using API.Data;
using API.DTO;
using API.Entities;
using API.Repository;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public Users(IUserRepository repository, ILogger<Users> logger, IMapper mapper)
        {
            _logger = logger;
            _userRepository = repository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult> AllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(_mapper.Map<IEnumerable<MemberDTO>>(users));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (null == user)
            {
                return NotFound("Cannot be found");
            }
            return Ok(_mapper.Map<MemberDTO>(user));
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetUserByName(string name)
        {
            var user = await _userRepository.GetUserByNameAsync(name);
            if (null == user)
            {
                return NotFound("Cannot be found");
            }
            return Ok(_mapper.Map<MemberDTO>(user));
        }

        [HttpPost()]
        public async Task<ActionResult> UpdateUserProfile([FromBody] UpdateMemberDTO updateMemberDTO)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Unauthorized");
            }
            var user = await _userRepository.GetUserByNameAsync(username);
            if (null == user)
            {
                return NotFound("Cannot be found");
            }
            _mapper.Map<UpdateMemberDTO, AppUser?>(updateMemberDTO, user);
            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }
    }
}
