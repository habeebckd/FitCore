﻿using Application.Dto;
using Application.Dto.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegistrationDto dto);

        Task<bool> TrinerRegisteration(UserRegistrationDto dto);
        Task<UserResponseDto> Login (UserLoginDto dto);
        //Task<List<UserListDto>> GetAllUsers();
        Task<ApiResponse<UserProfileDto>> GetUserProfile(int userId);
        Task<ApiResponse<string>> UpdateUserProfile(int userId, UpdateUserProfileDto dto);
    }
}
