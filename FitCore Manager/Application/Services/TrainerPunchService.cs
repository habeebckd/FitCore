using Application.Dto;
using Application.Dto.Attantance;
using Application.Interface.Reppo.Attantance;
using Application.Interface.Serv.Attantance;
using infrastructure.Repository;

public class TrainerPunchService : ITrainerPunchService
{
    private readonly ITrainerPunchRepository _repository;
    private readonly IUserRepository _userRepo;

    public TrainerPunchService(ITrainerPunchRepository repository, IUserRepository userRepo)
    {
        _repository = repository;
        _userRepo = userRepo;
    }

  
    public async Task<ApiResponse<PunchResponseDto>> PunchInAsync(int userId)
    {
        var userMembership = await _userRepo.GetUserMembershipWithPlanAsync(userId); 

        if (userMembership == null ||
            userMembership.MembershipPlans == null ||
            userMembership.MembershipPlans.PlanName != "Premium" ||
            !userMembership.IsActive ||
            userMembership.StartDate > DateTime.Now ||
            userMembership.EndDate < DateTime.Now)
        {
            return new ApiResponse<PunchResponseDto>(false, "Only active premium users can punch in.", null, null);
        }

        var slot = await _repository.GetValidBookingSlotAsync(userId);

        if (slot == null)
        {
            return new ApiResponse<PunchResponseDto>(false, "You don't have any active session now.", null, null);
        }

        var existingAttendance = await _repository.GetAttendanceAsync(slot.Id);

        if (existingAttendance != null && existingAttendance.PunchInTime != null)
        {
            return new ApiResponse<PunchResponseDto>(false, "Already punched in for this session.", null, null);
        }

        var result = await _repository.CreatePunchInAsync(slot.Id);

        return new ApiResponse<PunchResponseDto>(true, "Punch in successful.", result, null);
    }

  
    public async Task<ApiResponse<string>> PunchOutAsync(int userId)
    {
        var userMembership = await _userRepo.GetUserMembershipWithPlanAsync(userId); 

        if (userMembership == null ||
            userMembership.MembershipPlans == null ||
            userMembership.MembershipPlans.PlanName != "Premium" ||
            !userMembership.IsActive ||
            userMembership.StartDate > DateTime.Now ||
            userMembership.EndDate < DateTime.Now)
        {
            return new ApiResponse<string>(false, "Only active premium users can punch out.", null, null);
        }

        var slot = await _repository.GetValidBookingSlotAsync(userId);

        if (slot == null)
        {
            return new ApiResponse<string>(false, "No active session found for punch out.", null, null);
        }

        var existingAttendance = await _repository.GetAttendanceAsync(slot.Id);

        if (existingAttendance == null || existingAttendance.PunchInTime == null)
        {
            return new ApiResponse<string>(false, "You must punch in before punching out.", null, null);
        }

        if (existingAttendance.PunchOutTime != null)
        {
            return new ApiResponse<string>(false, "You have already punched out.", null, null);
        }

        var success = await _repository.CreatePunchOutAsync(slot.Id);

        if (!success)
        {
            return new ApiResponse<string>(false, "Failed to punch out.", null, null);
        }

        return new ApiResponse<string>(true, "Punch out successful.", "Success", null);
    }
}
