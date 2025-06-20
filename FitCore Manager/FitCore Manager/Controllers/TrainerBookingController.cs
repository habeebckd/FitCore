using Application.Dto.Traiiner;
using Application.Interface.Serv.Trainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerBookingController : ControllerBase
    {
        private readonly ITrainerBookingService _service;

        public TrainerBookingController(ITrainerBookingService service)
        {
            _service = service;
        }

        [HttpPost("Book")]
        public async Task<IActionResult> BookTrainer([FromBody] TrainerBookingRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _service.BookTrainerAsync(userId, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("MySessions")]
        public async Task<IActionResult> GetSessions()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _service.GetUserSessionsAsync(userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


      

        [HttpPost("add-slot")]
        //[Authorize(Roles ="Trainer")]
        public async Task<IActionResult> AddSlot(CreateTrainerSlotDto dto)
        {
            var result = await _service.AddTraninerSlotAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        //[HttpGet("available-slots")]
        //public async Task<IActionResult> GetAvailableSlots()
        //{
        //    var result = await _service.GetAvailableSlotsAsync();
        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots()
        {
            var result = await _service.GetAvailableSlotsAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }



        [HttpPost("addTimeSlot")]
        public async Task<IActionResult> AddTimeSlot(CreateTimeSlotDto dto)
        {
            var result = await _service.AddTimeSlotAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllTrainers()
        {
            var result = await _service.GetAllTrainersAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
