using Application.Dto;
using Application.Dto.MembershipPlan;
using Application.Interface.Serv.membership;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMembershipController : ControllerBase
    {
        private readonly IAdminMembershipService _adminService;
        public AdminMembershipController(IAdminMembershipService adminService)
        {
            _adminService = adminService;
        }


        [HttpPost("Create")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> CreatePlan(CreateMembershipPlanDto dto)
        {
            try
            {
                bool isCreated = await _adminService.CreatePlanAsync(dto);
                if (!isCreated)
                {
                    return BadRequest(new ApiResponse<string>(false, "Plan already exists or creation failed", null, null));

                }
                return Ok(new ApiResponse<string>(true, "Plan created successfully", "Done", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Server error", null, null));
            }

        }





        [HttpPatch("update")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> UpdatePlan(UpdateMembershipPlanDto dto)
        {
            try
            {
                var result = await _adminService.UpdatePlanAsync(dto);
                if (!result)
                {
                    return NotFound(new ApiResponse<bool>(false, "Plan not found.", false, null));
                }
                return Ok(new ApiResponse<bool>(true, "Plan updated successfully.", true, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>(false, "faild to update plan", false, ex.Message));
            }
        }





        [HttpDelete]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            try
            {
                var result = await _adminService.DeletePlanAsync(planId);
                if(!result)
                {
                    return NotFound(new ApiResponse<string>(false, "Plan not found.", null, null));
                }
                return Ok(new ApiResponse<string>(true, "Plan deleted successfully.", "Success", null));
            }
            catch
            {
                return StatusCode(500, new ApiResponse<string>(false, "Failed to delete plan.", null, null));
            }
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlans()
        {
            try
            {
                var plans = await _adminService.GetAllPlansAsync();
                if (plans == null || !plans.Any())
                {
                    return NotFound(new ApiResponse<IEnumerable<MembershipPlanDto>>(false, "No plans found", null, null));
                }
                return Ok(new ApiResponse<IEnumerable<MembershipPlanDto>>(true, "Plans retrieved successfully", plans, null));
            }

            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<MembershipPlanDto>>(false, "Server error", null, null));
            }
        }




        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlan(int planId)
        {
            try
            {
                var plan = await _adminService.GetPlanByIdAsync(planId);
                if (plan == null)
                {
                    return NotFound(new ApiResponse<string>(false, "Plan not found.", null, null));
                }
                return Ok(new ApiResponse<string>(true, "Plan recived successfully.", null, null));
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new ApiResponse<string>(false, "Failed to resive plan.", null, null));
            }
        }




        [HttpGet("all user membership")]
        [Authorize (Roles ="owner,Trainer")]
        public async Task<IActionResult> GetAllUserMemberships()
        {
            var result =await _adminService.GetAllUserMembershipsAsync();
            return Ok(new ApiResponse<IEnumerable<ViewUserMembershipDto>>(true, "fetched Succsessfully", result, null));
        }



        [HttpGet("user membership")]
        [Authorize(Roles ="owner")]
        public async Task<IActionResult> GetUserMembership(int userid)
        {
            var membership = await _adminService.GetUserMembershipByUserIdAsync(userid);

            if (membership == null)
                return NotFound(new ApiResponse<string>(false, "user no membership", null, null));
            return Ok(new ApiResponse<ViewUserMembershipDto>(true, "fetched successfully", membership, null));
        }



    }
}
