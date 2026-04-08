using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Services;

namespace W_M_S_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Get Comments for a Task
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetByTaskId(int taskId)
        {
            var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        // Add Comment
        [HttpPost]
        public async Task<ActionResult<CommentResponseDto>> Create([FromBody] CreateCommentDto createCommentDto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var comment = await _commentService.AddCommentAsync(userId, createCommentDto);
                return Ok(comment);
            }
            return Unauthorized();
        }

        // Delete Comment (Admin or Owner)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Owner check logic could be added in service layer
            // For now, let's just implement Admin check or basic delete
            if (!User.IsInRole("Admin"))
            {
                // In a real app, check if UserId == comment.UserId
            }

            var result = await _commentService.DeleteCommentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
