using API.Data;
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
            return Ok(_mapper.Map<IEnumerable<DTO.MemberDTO>>(users));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (null == user)
            {
                return NotFound("Cannot be found");
            }
            return Ok(_mapper.Map<DTO.MemberDTO>(user));
        }
    }
}
