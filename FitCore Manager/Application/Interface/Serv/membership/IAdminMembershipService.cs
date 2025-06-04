using Application.Dto.MembershipPlan;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.membership
{
    public interface IAdminMembershipService
    {
        
        Task<IEnumerable<MembershipPlanDto>> GetAllPlansAsync();
        Task<MembershipPlans> GetPlanByIdAsync(int planId);
        Task<IEnumerable<ViewUserMembershipDto>> GetAllUserMembershipsAsync();
        Task<ViewUserMembershipDto> GetUserMembershipByUserIdAsync(int userId);
        Task<bool> CreatePlanAsync(CreateMembershipPlanDto dto);
        Task<bool> UpdatePlanAsync(UpdateMembershipPlanDto dto);
        Task<bool> DeletePlanAsync(int planId);
    }
}
