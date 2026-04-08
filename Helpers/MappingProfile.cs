using AutoMapper;
using W_M_S_Project.DTOs;
using W_M_S_Project.Models;

namespace W_M_S_Project.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User to UserResponseDto
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            // CreateUserDto to User
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // UpdateUserDto to User
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // TaskEntity to TaskResponseDto
            CreateMap<TaskEntity, TaskResponseDto>()
                .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser.Name));

            // CreateTaskDto to TaskEntity
            CreateMap<CreateTaskDto, TaskEntity>();
            
            // UpdateTaskDto to TaskEntity
            CreateMap<UpdateTaskDto, TaskEntity>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Comment to CommentResponseDto
            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            // CreateCommentDto to Comment
            CreateMap<CreateCommentDto, Comment>();
        }
    }
}
