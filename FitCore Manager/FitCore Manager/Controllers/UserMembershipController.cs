using Application.Dto;
using Application.Dto.MembershipPlan;
using Application.Interface.Serv.membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMembershipController : ControllerBase
    {
        private readonly IUserMembershipService _userService;
        public UserMembershipController(IUserMembershipService userService)
        {
            _userService = userService;
        }



        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToPlan([FromQuery] int planId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized(new ApiResponse<string>(false, "Invalid token.", null, null));

                var userId = int.Parse(userIdClaim.Value);
                var result = await _userService.SubscribeToPlanAsync(userId, planId);

                if (!result)
                    return BadRequest(new ApiResponse<string>(false, "Subscription failed.", null, null));

                return Ok(new ApiResponse<string>(true, "Subscribed successfully.", "Done", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Subscription failed.", null, ex.Message));
            }
        }



        [HttpGet("my-membership")]
        public async Task<IActionResult> GetMyMembership()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var membership = await _userService.GetUserMembershipAsync(userId);
                if (membership == null)
                    return NotFound(new ApiResponse<string>(false, "No active membership found.", null, null));

                return Ok(new ApiResponse<UserMembershipDto>(true, "Membership retrieved successfully.", membership, null));
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new ApiResponse<string>(false, "Failed to retrieve membership.", null, ex.Message));
            }
        }


        [HttpPost("cancel-subscription")]
        public async Task<IActionResult> CancelSubscription()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                var result = await _userService.CancelSubscriptionAsync(userId);
                return Ok(new ApiResponse<bool>(true, "Subscription cancelled.", result,null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message, null,null));
            }
        }

    }
}
