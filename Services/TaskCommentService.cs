using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Models;
using W_M_S_Project.Repositories;

namespace W_M_S_Project.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
        }

        public async Task<TaskResponseDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return null;
            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = _mapper.Map<TaskEntity>(createTaskDto);
            task.CreatedAt = DateTime.UtcNow;
            
            var createdTask = await _taskRepository.AddTaskAsync(task);
            
            // Re-fetch to include assigned user
            var taskWithUser = await _taskRepository.GetTaskByIdAsync(createdTask.Id);
            return _mapper.Map<TaskResponseDto>(taskWithUser);
        }

        public async Task<TaskResponseDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return null;

            _mapper.Map(updateTaskDto, task);
            await _taskRepository.UpdateTaskAsync(task);
            
            var updatedTask = await _taskRepository.GetTaskByIdAsync(id);
            return _mapper.Map<TaskResponseDto>(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return false;

            await _taskRepository.DeleteTaskAsync(id);
            return true;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
        }
    }

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsByTaskIdAsync(int taskId)
        {
            var comments = await _commentRepository.GetCommentsByTaskIdAsync(taskId);
            return _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
        }

        public async Task<CommentResponseDto> AddCommentAsync(int userId, CreateCommentDto createCommentDto)
        {
            var comment = _mapper.Map<Comment>(createCommentDto);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            var createdComment = await _commentRepository.AddCommentAsync(comment);
            
            // Re-fetch to include user info (or could just map if context allows)
            // For now, let's assume we need to return the full DTO
            return _mapper.Map<CommentResponseDto>(createdComment);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            await _commentRepository.DeleteCommentAsync(id);
            return true;
        }
    }
}
