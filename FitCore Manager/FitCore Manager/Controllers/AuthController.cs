﻿using Application.Dto;
using Application.Dto.user;
using Application.Services;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegistrationDto newUser)
        {
            try
            {
                bool isRegistered = await _authService.Register(newUser);

                if (!isRegistered)
                {
                    return BadRequest(new ApiResponse<string>(false, "User already exists", null, null));
                }

                return Ok(new ApiResponse<string>(true, "User registered successfully", "Done", null));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Server error", null, null));
            }
        }



        [HttpPost(" TrainerRegister")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> TrinerRegister([FromForm] UserRegistrationDto newUser)
        {
            try
            {
                bool isRegistered = await _authService.TrinerRegisteration(newUser);

                if (!isRegistered)
                {
                    return BadRequest(new ApiResponse<string>(false, "Trainer already exists", null, null));
                }

                return Ok(new ApiResponse<string>(true, "Trainer registered successfully", "Done", null));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Server error", null, null));
            }
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto login)
        {
            try
            {
                var result = await _authService.Login(login);

                if (result.Error == "Not Found")
                {
                    return NotFound(new ApiResponse<string>(false, "Email not found", null, null));
                }

                if (result.Error == "Invalid Password")
                {
                    return BadRequest(new ApiResponse<string>(false, "Invalid password", null, null));
                }

                if (result.Error == "User Blocked")
                {
                    return StatusCode(403, new ApiResponse<string>(false, "User is blocked by admin", null, null));
                }

                return Ok(new ApiResponse<UserResponseDto>(true, "Login successful", result, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Server error", null, ex.Message));
            }
        }

        [HttpGet("userProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _authService.GetUserProfile(userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("profileUpdating")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _authService.UpdateUserProfile(userId, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }
}
