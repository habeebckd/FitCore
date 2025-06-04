using Application.Dto.MembershipPlan;
using Application.Interface.Reppo.membership;
using Application.Interface.Serv.membership;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserMembershipService : IUserMembershipService
    {
        private readonly IUserMembershipRepository _userMembershipRepository;
        private readonly IMembershipPlanRepository _planRepository;

        public UserMembershipService( IUserMembershipRepository userMembershipRepository,IMembershipPlanRepository planRepository)
        {
            _userMembershipRepository = userMembershipRepository;
            _planRepository = planRepository;
        }



        public async Task<bool> SubscribeToPlanAsync(int userId, int planId)
        {
            var existingMembership = await _userMembershipRepository.GetActiveMembershipByUserIdAsync(userId);
            if (existingMembership != null)
                throw new InvalidOperationException("You already have an active membership.");
            var plan = await _planRepository.GetPlanByIdAsync(planId);
            if (plan == null)
                throw new ArgumentException("Invalid plan selected.");

            var membership = new UserMembership
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(plan.Duration),
                IsActive = true
            };
            await _userMembershipRepository.AddUserMembershipAsync(membership);
            return true;
        }



        public async Task<UserMembershipDto> GetUserMembershipAsync(int userId)
        {
            var membership = await _userMembershipRepository.GetActiveMembershipByUserIdAsync(userId);
            if (membership == null)
                return null;

            return new UserMembershipDto
            {
                UserMembershipId = membership.UserId,
                PlanName = membership.MembershipPlans.PlanName,
                Price = membership.MembershipPlans.Price,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive
            };
        }








    }
}
