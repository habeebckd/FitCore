using Application.Dto;
using Application.Dto.Workout;
using Application.Interface.Reppo.Workout;
using Application.Interface.Serv.Workout;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserWorkoutPlanService : IUserWorkoutPlanService
    {


        private readonly IUserWorkoutPlanRepository _Repository;
        public UserWorkoutPlanService(IUserWorkoutPlanRepository repository)
        {
            _Repository = repository;
        }


        public async Task<ApiResponse<string>> AssignWorkoutPlan(int userId, string goal)
        {
            try
            {
                var plan = await _Repository.GetPlanByGoal(goal);
                if (plan == null)
                    return new ApiResponse<string>(false, "No plan found for this goal", null, null);

                var existing = await _Repository.GetUserAssignedPlan(userId, plan.Id);
                if (existing != null)
                    return new ApiResponse<string>(false, "Plan already assigned", null, null);

                var assignment = new UserWorkoutPlan
                {
                    UserId = userId,
                    WorkoutPlanId = plan.Id,
                    AssignedDate = DateTime.Now,
                    IsCompleted = false,
                };
                await _Repository.AddUserWorkoutPlan(assignment);
                return new ApiResponse<string>(true, "Workout plan assigned successfully", "Done", null);
            }


            catch (Exception ex)
            {
                return new ApiResponse<string>(false, "Failed to assign workout plan", null, ex.Message);
            }
        }




        public async Task<ApiResponse<ICollection<WorkoutPlan>>> AvailablePlans()
        {
            try
            {
                var plans = await _Repository.GetAllPlans();
                return new ApiResponse<ICollection<WorkoutPlan>>(true, "Fetched successfully", plans, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ICollection<WorkoutPlan>>(false, "Failed to fetch plans", null, ex.Message);
            }
        }




        public async Task<ApiResponse<ICollection<UserWorkoutPlanDto>>> GetAllUserPlans()
        {
            try
            {
                var data = await _Repository.GetAllUserWorkoutPlans();
                return new ApiResponse<ICollection<UserWorkoutPlanDto>>(true, "success", data, null);
            }
            catch (Exception ex) 
            {
                return new ApiResponse<ICollection<UserWorkoutPlanDto>>(false, "Error retrieving data", null, ex.Message);
            }
        }


    }
}
