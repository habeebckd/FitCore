using Application.Dto.Rating;
using Application.Interface.Serv.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymRatingController : ControllerBase
    {
        private readonly IGymRatingService _Service;
        public GymRatingController(IGymRatingService service)
        {
            _Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating(GymRatingCreateDto dto)
        {
            var userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _Service.AddRatingAsync(userid, dto);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _Service.GetAllRatingsAsync();
            return Ok(response);
        }
    }
}
