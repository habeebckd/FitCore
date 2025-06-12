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
    public class WorkoutPlanService : IWorkoutPlanService
    {

        private readonly IWorkoutPlanRepository _repo;
        public WorkoutPlanService(IWorkoutPlanRepository repo)
        {
            _repo = repo;
        }


        public async Task<ApiResponse<ICollection<WorkoutPlan>>> GetAllPlansAsync()

        {
            try
            {
                var plans = await _repo.GetAllAsync();
                return new ApiResponse<ICollection<WorkoutPlan>>(true, "Fetched successfully", plans, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ICollection<WorkoutPlan>>(false, "Failed to fetch", null, ex.Message);
            }
        }





        public async Task<ApiResponse<WorkoutPlan>> GetPlanByIdAsync(int id)
        {
            try
            {
                var plan = await _repo.GetByIdAsync(id);

                if (plan == null)
                {
                    return new ApiResponse<WorkoutPlan>(false, "Workout plan not found", null, null);
                }

                return new ApiResponse<WorkoutPlan>(true, "Fetched successfully", plan, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<WorkoutPlan>(false, "Error occurred", null, ex.Message);
            }
        }





        public async Task<ApiResponse<string>> CreatePlanAsync(CreateWorkoutPlanDto dto)
        {
            try
            {
                var plan = new WorkoutPlan
                {
                    Name = dto.Title,
                    Description = dto.Description,
                    DurationInWeeks = dto.DurationInWeeks,
                    Goal = dto.Goal,
                    Type = dto.Type,

                };

                await _repo.AddAsync(plan);
                return new ApiResponse<string>(true, "Plan created", "Done", null);
            }
            catch (Exception ex) 
            {
                return new ApiResponse<string>(false, "plan creation faild", null, ex.Message);
            }
        }




        public async Task<ApiResponse<string>> UpdatePlanAsync(int id, CreateWorkoutPlanDto dto) 
        {
            try
            {
                var plan = await _repo.GetByIdAsync(id);
                if (plan == null)
                    return new ApiResponse<string>(false, "Wprkout Plan not found", null, null);
                plan.Name = dto.Title;
                plan.Description = dto.Description;
                plan.DurationInWeeks = dto.DurationInWeeks;
                plan.Goal = dto.Goal;
                plan.Type = dto.Type;

                await _repo.UpdateAsync(plan);
                return new ApiResponse<string>(true, "Workout Plan updated", "Done", null);
            }
            catch (Exception ex) 
            {
                return new ApiResponse<string>(false, "updation failed", null, ex.Message);
            }
        }



        public async Task<ApiResponse<string>> DeletePlanAsync(int id)
        {
            try
            {
                var exisitingplan = await _repo.GetByIdAsync(id);
                if (exisitingplan == null)
                {
                    return new ApiResponse<string>(false, "not found ", null, null);
                }
                await _repo.DeleteAsync(id);


                return new ApiResponse<string>(true, "WorkOut Plan deleted", "Done", null);
            }
            catch (Exception ex) 
            {
                return new ApiResponse<string>(false, "WorkOut Plan Delete failed", null, ex.Message);
            }
        }




    }
}
