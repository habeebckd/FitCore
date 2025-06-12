using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.Workout
{
    public interface IWorkoutPlanRepository
    {
        Task<ICollection<WorkoutPlan>> GetAllAsync();
        Task<WorkoutPlan> GetByIdAsync(int id);
        Task AddAsync(WorkoutPlan plan);
        Task UpdateAsync(WorkoutPlan plan);
        Task DeleteAsync(int id);
    }
}
