using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Services;

namespace W_M_S_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IAuthService _authService;

        public PasswordController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authService.ForgotPasswordAsync(dto.Email);
            if (!success)
                return NotFound(new { message = "User not found." });

            return Ok(new { message = "Password reset link has been sent to your email." });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authService.ResetPasswordAsync(dto);
            if (!success)
                return BadRequest(new { message = "Invalid or expired token." });

            return Ok(new { message = "Password reset successfully." });
        }
    }
}
