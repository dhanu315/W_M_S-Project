using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Services;

namespace W_M_S_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;
            if (limit > 100) limit = 100;

            var (users, totalCount) = await _userService.GetUsersAsync(page, limit, search);

            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Users = users
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserResponseByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateUserAsync(dto);
            if (createdUser == null)
            {
                return BadRequest(new { message = "Failed to create user. Email may be taken or Role may be invalid." });
            }

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(id, dto);
            if (updatedUser == null)
            {
                return BadRequest(new { message = "Failed to update user. Email may be taken, Role invalid, or User not found." });
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound(new { message = "User not found." });
            }

            return NoContent(); // 204
        }
    }
}
