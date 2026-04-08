using System.Collections.Generic;
using System.Threading.Tasks;
using W_M_S_Project.Models;

namespace W_M_S_Project.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<TaskEntity?> GetTaskByIdAsync(int id);
        Task<TaskEntity> AddTaskAsync(TaskEntity task);
        Task UpdateTaskAsync(TaskEntity task);
        Task DeleteTaskAsync(int id);
        Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId);
    }

    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(int taskId);
        Task<Comment> AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
    }
}
