using System.Collections.Generic;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;

namespace W_M_S_Project.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();
        Task<TaskResponseDto?> GetTaskByIdAsync(int id);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskResponseDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<TaskResponseDto>> GetTasksByUserIdAsync(int userId);
    }

    public interface ICommentService
    {
        Task<IEnumerable<CommentResponseDto>> GetCommentsByTaskIdAsync(int taskId);
        Task<CommentResponseDto> AddCommentAsync(int userId, CreateCommentDto createCommentDto);
        Task<bool> DeleteCommentAsync(int id);
    }
}
