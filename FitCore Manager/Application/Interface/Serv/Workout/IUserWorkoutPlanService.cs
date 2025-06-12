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
    public interface IUserWorkoutPlanService
    {
        Task<ApiResponse<string>> AssignWorkoutPlan(int userId, string goal);
        Task<ApiResponse<ICollection<WorkoutPlan>>> AvailablePlans();
        Task<ApiResponse<ICollection<UserWorkoutPlanDto>>> GetAllUserPlans();
    }
}
