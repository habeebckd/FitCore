using Application.Dto.Workout;
using Application.Interface.Reppo.Workout;
using Domain.Model;
using infrastructure.Context;
using infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class UserWorkoutPlanRepository : IUserWorkoutPlanRepository
    {
        private readonly AppDbContext _Context;
        public UserWorkoutPlanRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<WorkoutPlan> GetPlanById(int planId)
        {
            return await _Context.workoutPlans.FirstOrDefaultAsync(p => p.Id == planId);
        }


        public async Task<UserWorkoutPlan> GetUserAssignedPlan(int userId, int planId)
        {
            return await _Context.userWorkoutPlans.FirstOrDefaultAsync(p => p.UserId == userId && p.WorkoutPlanId == planId);
        }


        public async Task AddUserWorkoutPlan(UserWorkoutPlan Plan)
        {
            await _Context.userWorkoutPlans.AddAsync(Plan);
            await _Context.SaveChangesAsync();
        }

        public async Task<ICollection<WorkoutPlan>> GetAllPlans()
        {
            return await _Context.workoutPlans.ToListAsync();
        }



        public async Task<ICollection<UserWorkoutPlanDto>> GetAllUserWorkoutPlans()
        {
            try
            {
                var result = await _Context.userWorkoutPlans
                    .Include(a => a.User)
                    .Include(b => b.WorkoutPlan)
                    .Select(c => new UserWorkoutPlanDto
                    {
                        UserId = c.UserId,
                        UserName = c.User.UserName,
                        WorkoutGoal = c.WorkoutPlan.Goal,
                        PlanTitle = c.WorkoutPlan.Name,
                        AssignedDate = c.AssignedDate
                    }).ToListAsync();
                return result;
            }
            catch
            {
                return new List<UserWorkoutPlanDto>();
            }
        }



    }
}
