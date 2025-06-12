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
    public class WorkoutPlanRepository : IWorkoutPlanRepository
    {



        private readonly AppDbContext _Context;
        public WorkoutPlanRepository (AppDbContext context)
        {
            _Context = context;
        }



       public async Task<ICollection<WorkoutPlan>> GetAllAsync()
        {
            return await _Context.workoutPlans.ToListAsync();
        }



        public async Task<WorkoutPlan> GetByIdAsync(int id)
        {
            return await _Context.workoutPlans.FindAsync(id);
        }


        public async Task AddAsync(WorkoutPlan plan)
        {
            _Context.workoutPlans.Add(plan);
            await _Context.SaveChangesAsync();
        }


        public async Task UpdateAsync(WorkoutPlan plan)
        {
            _Context.workoutPlans.Update(plan);
            await _Context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var plan = await _Context.workoutPlans.FindAsync(id);
            if (plan != null)
            {
                _Context.workoutPlans.Remove(plan);
                await _Context.SaveChangesAsync();
            }
        }



    }
}
