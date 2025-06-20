using Application.Dto.Workout;
using Application.Interface.Serv.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorkoutPlanController : ControllerBase
    {
        private readonly IUserWorkoutPlanService _Service;
        public UserWorkoutPlanController(IUserWorkoutPlanService service) 
        {
            _Service = service;
        }



        [HttpPost("assign")]
        public async Task<IActionResult> AssignPlan(AssignWorkoutPlanDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var response = await _Service.AssignWorkoutPlan(userId, dto.WorkoutPlanId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }



        [HttpGet("available-plans")]
        public async Task<IActionResult> GetAvailablePlans()
        {
            var response = await _Service.AvailablePlans();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }


        [HttpGet("AllUserWorkoutPlans")]
        [Authorize(Roles = "owner,Trainer")]
        public async Task<IActionResult> GetAllUserWorkoutPlans()
        {
            var response = await _Service.GetAllUserPlans();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
