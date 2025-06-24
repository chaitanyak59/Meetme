using API.DTO;
using API.Entities;
using API.Repository;
using API.Services;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Member")]
    public class Users : BaseApiController
    {
        private readonly ILogger<Users> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public Users(IUserRepository repository, ILogger<Users> logger, IMapper mapper, IPhotoService photoService)
        {
            _logger = logger;
            _userRepository = repository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
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

        [HttpPost("photo")]
        public async Task<ActionResult> UploadProfile(IFormFile file)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Unauthorized");
            }
            var user = await _userRepository.GetUserByNameAsync(username);
            if (null == user)
            {
                return NotFound("User details not found");
            }
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var userPhoto = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = user.Photos.ToList().Count() == 0
            };

            user.Photos.Add(userPhoto);
            await _userRepository.UpdateUserAsync(user);
            return Ok(_mapper.Map<PhotoDTO>(userPhoto));
        }
    }
}
