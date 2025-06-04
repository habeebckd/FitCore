using Application.Dto.MembershipPlan;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IUserMembershipRepository
    {
        Task <UserMembership> GetActiveMembershipByUserIdAsync(int userId);
        Task AddUserMembershipAsync(UserMembership membership);
    }
}
