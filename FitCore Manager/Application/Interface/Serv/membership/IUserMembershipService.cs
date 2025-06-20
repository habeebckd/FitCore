using Application.Dto.MembershipPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.membership
{
     public interface IUserMembershipService
    {
        Task<bool> SubscribeToPlanAsync(int userId, int planId);
        Task<UserMembershipDto> GetUserMembershipAsync(int userId);

        Task<bool> CancelSubscriptionAsync(int userId);
    }
}
