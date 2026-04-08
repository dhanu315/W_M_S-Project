using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Services;

namespace W_M_S_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Get All Tasks (Admin/Manager/Member)
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Member,Guest")] // Guest can read too
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        // Get Task By Id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Member,Guest")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Create Task (Admin/Manager)
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TaskResponseDto>> Create([FromBody] CreateTaskDto createTaskDto)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // Update Task (Admin/Manager)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TaskResponseDto>> Update(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Delete Task (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // Get Personal Tasks (Member)
        [HttpGet("my-tasks")]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetMyTasks()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var tasks = await _taskService.GetTasksByUserIdAsync(userId);
                return Ok(tasks);
            }
            return Unauthorized();
        }
    }
}
