using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.Workout
{
    public interface IWorkoutPlanDayDetailsRepository
    {
        Task<IEnumerable<WorkoutPlanDayDetails>> GetAll();
        Task<WorkoutPlanDayDetails>GetById(int id);
        Task AddAsync(WorkoutPlanDayDetails workoutPlanDayDetails);
        Task SaveAsync();
        Task<bool> ExistsAsync(int planId, int week, int day);

        Task<UserWorkoutPlan?> GetUserWorkoutPlanAsync(int userId);

        Task<List<WorkoutPlanDayDetails>> GetWorkoutPlanDayDetailsAsync(int workoutPlanId);


    }
}
