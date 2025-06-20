using Application.Dto.MembershipPlan;
using Application.Interface.Reppo.membership;
using Application.Interface.Serv.membership;
using Application.Interface.Serv.Notifications;
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
        private readonly INotificationService _notification;

        public UserMembershipService( IUserMembershipRepository userMembershipRepository,IMembershipPlanRepository planRepository,INotificationService notification)
        {
            _userMembershipRepository = userMembershipRepository;
            _planRepository = planRepository;
            _notification = notification;
        }



        public async Task<bool> SubscribeToPlanAsync(int userId, int planId)
        {
            var existingMembership = await _userMembershipRepository.GetActiveMembershipByUserIdAsync(userId);
            if (existingMembership != null)
                throw new InvalidOperationException("You already have an active membership.");
            var plan = await _planRepository.GetPlanByIdAsync(planId);
            if (plan == null)
                throw new ArgumentException("Invalid plan selected.");

            if (plan.Duration <= 0)
                throw new InvalidOperationException("The selected plan has an invalid duration.");

            var membership = new UserMembership
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(plan.Duration),
                IsActive = true
            };
            await _userMembershipRepository.AddUserMembershipAsync(membership);
            var message = $"You have successfully subscribed to the {plan.PlanName}plan";
            await _notification.NotifyUserAsync(userId, message);

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






        public async Task<bool> CancelSubscriptionAsync(int userId)
        {
            var membership = await _userMembershipRepository.GetActiveMembershipByUserIdAsync(userId);
            if (membership == null)
                throw new InvalidOperationException("No active membership found to cancel.");

            membership.IsActive = false;
            membership.EndDate = DateTime.UtcNow; // optionally end it immediately
            await _userMembershipRepository.UpdateUserMembershipAsync(membership);

            return true;
        }




    }
}
