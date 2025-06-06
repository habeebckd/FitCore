﻿using Application.Dto.user;
using AutoMapper;
using FitCore_Manager.Model;
using infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> Register(UserRegistrationDto dto)
        {
            dto.UserName = dto.UserName.Trim();
            dto.Email = dto.Email.Trim();
            dto.Password = dto.Password.Trim();
            dto.Phone= dto.Phone.Trim();
            

            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null) return false;

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = _mapper.Map<User>(dto);
            user.Password = hashPassword;
            user.CreatedAt = DateTime.Now;

            user.Role = "User";
            await _userRepository.AddUserAsync(user);
         
            return true;
        }


        public async Task<bool> TrinerRegisteration(UserRegistrationDto dto)
        {
            dto.UserName = dto.UserName.Trim();
            dto.Email = dto.Email.Trim();
            dto.Password = dto.Password.Trim();
            dto.Phone = dto.Phone.Trim();


            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null) return false;

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = _mapper.Map<User>(dto);
            user.Password = hashPassword;
            user.CreatedAt = DateTime.Now;

            user.Role = "Trainer";
            await _userRepository.AddUserAsync(user);

            return true;
        }



        //public async Task<UserResponseDto> Login(UserLoginDto dto)
        //{
        //    dto.Email = dto.Email.Trim();
        //    dto.Password = dto.Password.Trim();

        //    var user = await _userRepository.GetUserByEmailAsync(dto.Email);
        //    if (user == null) return new UserResponseDto { Error = "Not Found" };

        //    if (!ValidatePassword(dto.Password, user.Password))
        //        return new UserResponseDto { Error = "Invalid Password" };

        //    if (user.isBlocked == true)
        //    {
        //        return new UserResponseDto { Error = "User Blocked" };
        //    }
        //    var token = GenerateToken(user);
        //    return new UserResponseDto
        //    {
        //        UserId = user.UserId,
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        Role = user.Role,
        //        Token = token,
        //    };
        //}


        public async Task<UserResponseDto> Login(UserLoginDto dto)
        {
            if (dto == null)
                return new UserResponseDto { Error = "Invalid input. Please provide Email and Password." };

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return new UserResponseDto { Error = "Email and Password are required." };

            dto.Email = dto.Email.Trim();
            dto.Password = dto.Password.Trim();

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return new UserResponseDto { Error = "User not found." };

            if (!ValidatePassword(dto.Password, user.Password))
                return new UserResponseDto { Error = "Invalid Password." };

            if (user.isBlocked)
                return new UserResponseDto { Error = "User is Blocked." };

            var token = GenerateToken(user);
            return new UserResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Token = token,
            };
        }



        //public async Task<List<UserListDto>> GetAllUsers()
        //{
        //    var users = await _userRepository.GetAllUsersAsync();
        //    return _mapper.Map<List<UserListDto>>(users);
        //}


        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool ValidatePassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
