using Application.Interface.Serv.Attantance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerPunchController : ControllerBase
    {
        private readonly ITrainerPunchService _trainerPunchService;
            public TrainerPunchController(ITrainerPunchService trainerPunchService)
        {
            _trainerPunchService = trainerPunchService;
        }
        [HttpPost("punch-in")]
        public async Task<IActionResult> PunchIn()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _trainerPunchService.PunchInAsync(userId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpPost("punch-out")]
        public async Task<IActionResult> PunchOut()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _trainerPunchService.PunchOutAsync(userId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
