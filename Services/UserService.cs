using AutoMapper;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Helpers;
using W_M_S_Project.Models;
using W_M_S_Project.Repositories;

namespace W_M_S_Project.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<PaginatedResult<UserResponseDto>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            var (users, totalCount) = await _userRepository.GetPaginatedUsersAsync(pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return new PaginatedResult<UserResponseDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (await _userRepository.UserExistsAsync(createUserDto.Email))
            {
                throw new InvalidOperationException("Email already in use.");
            }

            var user = _mapper.Map<User>(createUserDto);
            
            // Hash password using BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            
            var createdUser = await _userRepository.AddUserAsync(user);
            
            // Re-fetch to include Role information if needed
            var userWithRole = await _userRepository.GetUserByIdAsync(createdUser.Id);
            return _mapper.Map<UserResponseDto>(userWithRole);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            if (updateUserDto.Email != null && 
                updateUserDto.Email != user.Email && 
                await _userRepository.UserExistsAsync(updateUserDto.Email))
            {
                throw new InvalidOperationException("Email already in use.");
            }

            // Map non-null values
            _mapper.Map(updateUserDto, user);
            
            await _userRepository.UpdateUserAsync(user);
            
            // Re-fetch to include Role information
            var updatedUser = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserResponseDto>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteUserAsync(id);
            return true;
        }
    }
}
