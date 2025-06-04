using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.membership
{
    public interface IMembershipPlanRepository
    {
        Task AddPlanAsync(MembershipPlans plan);
        Task<IEnumerable<MembershipPlans>> GetAllPlansAsync();
        Task<IEnumerable<UserMembership>> GetAllMembershipsWithUsersAndPlansAsync();
        Task<UserMembership> GetUserMembershipByUserIdAsync(int userId);

        Task<MembershipPlans> GetPlanByIdAsync(int planId);
        Task UpdatePlanAsync (MembershipPlans plan);
        Task DeletePlanAsync(int planId);
    }
}
