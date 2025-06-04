using Application.Dto.MembershipPlan;
using Application.Interface.Reppo.membership;
using Application.Interface.Serv.membership;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminMembershipService : IAdminMembershipService
    {
        private readonly IMembershipPlanRepository _PlanRepository;
        private readonly IMapper _Mapper;
        public AdminMembershipService(IMembershipPlanRepository planRepository,IMapper mapper)
        {
            _PlanRepository = planRepository;
            _Mapper = mapper;
        }



        public async Task<bool> CreatePlanAsync(CreateMembershipPlanDto dto)
        {
            //var plan = new MembershipPlans
            //{
            //    PlanName = dto.PlanName,
            //    Description = dto.Description,
            //    Price = dto.Price,
            //    Duration = dto.Duration,

            //};
            var plan = _Mapper.Map<MembershipPlans>(dto);
            await _PlanRepository.AddPlanAsync(plan);
            return true;
        }



        public async Task<IEnumerable<MembershipPlanDto>> GetAllPlansAsync()
        {
            var plans = await _PlanRepository.GetAllPlansAsync();
            return plans.Select(p => new MembershipPlanDto
            {
                PlanId = p.Id,
                PlanName = p.PlanName,
                Description = p.Description,
                Price = p.Price,
                DurationInDays = p.Duration
            });
        }



        public async Task<MembershipPlans> GetPlanByIdAsync(int planId)
        {
            return await _PlanRepository.GetPlanByIdAsync(planId);
        }


        //public async Task<bool> UpdatePlanAsync(UpdateMembershipPlanDto dto)
        //{
        //    var plan = await _PlanRepository.GetPlanByIdAsync(dto.PlanId);
        //    if(plan == null) return false;

        //    //plan.PlanName = dto.PlanName;
        //    //plan.Description = dto.Description;
        //    //plan.Price = dto.Price;
        //    //plan.Duration = dto.Duration;
        //    _Mapper.Map(dto, plan);

        //    await _PlanRepository.UpdatePlanAsync(plan);
        //    return true;
        //}




        public async Task<IEnumerable<ViewUserMembershipDto>> GetAllUserMembershipsAsync() 
        {
            var memberships = await _PlanRepository.GetAllMembershipsWithUsersAndPlansAsync();
            return memberships.Select(x => new ViewUserMembershipDto
            {
                UserName = x.User.UserName,
                Email = x.User.Email,
                PlanName = x.MembershipPlans.PlanName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsActive = x.IsActive,
            }).ToList();
        }



        public async Task<ViewUserMembershipDto> GetUserMembershipByUserIdAsync(int userId)
        {
            var membership = await _PlanRepository.GetUserMembershipByUserIdAsync(userId);
            if (membership == null) return null;

            var dto = new ViewUserMembershipDto 
            {
                UserName = membership.User.UserName,
                Email = membership.User.Email,
                PlanName = membership.MembershipPlans.PlanName,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive
            };
            return dto;
        }




        public async Task<bool> UpdatePlanAsync(UpdateMembershipPlanDto dto)
        {
            var plan = await _PlanRepository.GetPlanByIdAsync(dto.PlanId);
            if (plan == null) return false;

            // Only update if value is not null or empty
            if (!string.IsNullOrWhiteSpace(dto.PlanName))
                plan.PlanName = dto.PlanName;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                plan.Description = dto.Description;

            if (dto.Price > 0)
                plan.Price = dto.Price;

            if (dto.Duration > 0)
                plan.Duration = dto.Duration;

            await _PlanRepository.UpdatePlanAsync(plan);
            return true;
        }





        public async Task<bool> DeletePlanAsync(int planId)
        {
            var plan = await _PlanRepository.GetPlanByIdAsync(planId);
            if(plan == null) return false;

            await _PlanRepository.DeletePlanAsync(planId);
            return true;
        }

    }
}
