using Application.Dto.Workout;
using Application.Interface.Serv.Workout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlanDayDetailsController : ControllerBase
    {
        private readonly IWorkoutPlanDayDetailsService _Service;
        public WorkoutPlanDayDetailsController(IWorkoutPlanDayDetailsService service)
        {
            _Service = service;
        }

        [HttpGet("AllPlans")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _Service.GetAllAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult>GetByid(int id)
        {
            var result = await _Service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateWorkoutPlanDayDetailsDto dto)
        {
            var result = await _Service.CreateAsync(dto);
            return result.IsSuccess ? Ok(result) :BadRequest(result);
        }



       

        [HttpGet("UserWorkoutPlanDetails")]
        public async Task<IActionResult> GetUserWorkoutPlanDetails()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _Service.GetUserWorkoutPlanDetailsAsync(userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
