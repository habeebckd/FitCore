using Application.Dto;
using Application.Dto.Workout;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.Workout
{
    public interface IWorkoutPlanService
    {
        Task<ApiResponse<ICollection<WorkoutPlan>>> GetAllPlansAsync();
        Task<ApiResponse<WorkoutPlan>> GetPlanByIdAsync(int id);
        Task<ApiResponse<string>> CreatePlanAsync(CreateWorkoutPlanDto dto);
        Task<ApiResponse<string>> UpdatePlanAsync(int id, CreateWorkoutPlanDto dto);
        Task<ApiResponse<string>> DeletePlanAsync(int id);
    }
}
