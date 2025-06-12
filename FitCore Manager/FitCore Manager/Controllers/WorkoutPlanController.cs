using Application.Dto.Workout;
using Application.Interface.Serv.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _Service;
        public WorkoutPlanController(IWorkoutPlanService Service)
        {
            _Service = Service;
        }


        [HttpGet]
        [Authorize(Roles ="owner,Trainer")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _Service.GetAllPlansAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "owner,Trainer")]

        public async Task<IActionResult> GetById(int id) 
        {
            var response = await _Service.GetPlanByIdAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound();
        }



        [HttpPost]
        [Authorize(Roles = "owner,Trainer")]

        public async Task<IActionResult> Create(CreateWorkoutPlanDto dto)
        {
            var response = await _Service.CreatePlanAsync(dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "owner,Trainer")]

        public async Task<IActionResult> Update(int id,CreateWorkoutPlanDto dto)
        {
            var response = await _Service.UpdatePlanAsync(id, dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "owner,Trainer")]

        public async Task<IActionResult>Delete(int id)
        {
            var response = await _Service.DeletePlanAsync(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }



    }
}
