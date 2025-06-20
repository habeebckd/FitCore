using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.membership
{
    public interface IUserMembershipRepository
    {
        Task <UserMembership> GetActiveMembershipByUserIdAsync(int  userId);
        Task AddUserMembershipAsync(UserMembership Membership);

        Task UpdateUserMembershipAsync(UserMembership membership);
    }
}
