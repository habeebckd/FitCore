using Application.Dto;
using Application.Dto.user;
using Application.Interface;
using AutoMapper;
using CloudinaryDotNet;
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
        private readonly ICloudinaryServices _cloudinaryServices;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper,ICloudinaryServices cloudinaryServices)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
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


        public async Task<ApiResponse<UserProfileDto>> GetUserProfile(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return new ApiResponse<UserProfileDto>(false, "User not found", null, null);

                var dto = new UserProfileDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.Phone,
                    ImageUrl = user.ImageUrl
                };
                return new ApiResponse<UserProfileDto>(true, "Success", dto, null);
            }
            catch (Exception ex) 
            {
                return new ApiResponse<UserProfileDto>(false, "Error", null, ex.Message);
            }
        }



        public async Task<ApiResponse<string>> UpdateUserProfile(int userId, UpdateUserProfileDto dto)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return new ApiResponse<string>(false, "User not found", null, null);

                user.UserName = !string.IsNullOrWhiteSpace(dto.UserName) ? dto.UserName : user.UserName;
                user.Phone = !string.IsNullOrWhiteSpace(dto.Phone) ? dto.Phone : user.Phone;

                if (dto.ImageFile != null)
                {
                    var imageUrl = await _cloudinaryServices.UploadImageAsync(dto.ImageFile);
                    user.ImageUrl = imageUrl;
                }

                await _userRepository.UpdateUser(user);
                return new ApiResponse<string>(true, "Profile updated", "Done", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(false, "Update failed", null, ex.Message);
            }
        }




    }
}
