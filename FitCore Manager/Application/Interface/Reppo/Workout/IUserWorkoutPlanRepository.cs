using Application.Dto.Workout;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.Workout
{
    public interface IUserWorkoutPlanRepository
    {
        Task<WorkoutPlan> GetPlanByGoal(string goal);
        Task<UserWorkoutPlan> GetUserAssignedPlan(int userId, int planId);
        Task AddUserWorkoutPlan(UserWorkoutPlan Plan);
        Task<ICollection<WorkoutPlan>> GetAllPlans();
        Task<ICollection<UserWorkoutPlanDto>> GetAllUserWorkoutPlans();
    }
}
