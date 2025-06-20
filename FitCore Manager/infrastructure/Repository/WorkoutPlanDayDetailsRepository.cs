using Application.Interface.Reppo.Workout;
using Domain.Model;
using infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class WorkoutPlanDayDetailsRepository : IWorkoutPlanDayDetailsRepository
    {
        private readonly AppDbContext _Context;
        public WorkoutPlanDayDetailsRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<IEnumerable<WorkoutPlanDayDetails>> GetAll()
        {
            return await _Context.workoutPlanDayDetails.Include(a=>a.WorkoutPlan).ToListAsync();
        }

        public async Task<WorkoutPlanDayDetails> GetById(int id)
        {
            return await _Context.workoutPlanDayDetails
                .Include(a=>a.WorkoutPlan)
                .FirstOrDefaultAsync(b=>b.Id==id);
        }

        public async Task AddAsync(WorkoutPlanDayDetails workoutPlanDayDetails)
        {
            await _Context.workoutPlanDayDetails.AddAsync(workoutPlanDayDetails);
            await _Context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _Context.SaveChangesAsync();
        }


        public async Task<bool> ExistsAsync(int planId, int week, int day)
        {
            return await _Context.workoutPlanDayDetails.AnyAsync(a=>
            a.WorkoutPlanId == planId &&
            a.WeekNumber == week &&
            a.DayNumber == day);
        }





        public async Task<UserWorkoutPlan?> GetUserWorkoutPlanAsync(int userId)
        {
            return await _Context.userWorkoutPlans
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<List<WorkoutPlanDayDetails>> GetWorkoutPlanDayDetailsAsync(int workoutPlanId)
        {
            return await _Context.workoutPlanDayDetails
                .Where(x => x.WorkoutPlanId == workoutPlanId)
                .OrderBy(x => x.WeekNumber)
                .ThenBy(x => x.DayNumber)
                .ToListAsync();
        }

    }
}
