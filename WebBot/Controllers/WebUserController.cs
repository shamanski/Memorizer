using Memorizer.DbModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Entities;
using Model.Services;

namespace WebBot.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetByIdAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
            };

            return userDto;
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUserAsync(int id)
        {
            var u = await _userService.GetUserAsync(id);

            var userDtos =new UserDTO
            {
                Id = u.Id,
                Name = u.Name, 
            };

            return userDtos;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateAsync(UserDTO userCreateDto)
        {
            var user = new User
            {
                Name = userCreateDto.Name,
                Email = userCreateDto.Email,
            };

            await _userService.AddUserAsync(user);

            var userDto = new UserDTO
            {
                Id = user.Id,
                FirstName = user.Name,
                Email = user.Email,
            };

            return CreatedAtAction(nameof(GetByIdAsync), new { id = userDto.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserDTO userUpdateDto)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = userUpdateDto.Name;
            user.Email = userUpdateDto.Email;

            await _userService.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
