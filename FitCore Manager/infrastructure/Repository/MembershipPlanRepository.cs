using Application.Interface;
using Application.Interface.Reppo.membership;
using Domain.Model;
using infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class MembershipPlanRepository : IMembershipPlanRepository
    {
        private readonly AppDbContext _context;
        public MembershipPlanRepository (AppDbContext context)
        {
             _context = context;
        }

        public async Task AddPlanAsync(MembershipPlans plan)
        {
            await _context.membershipPlans.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MembershipPlans>> GetAllPlansAsync()
        {
            return await _context.membershipPlans.ToListAsync();
        }

       public async Task<MembershipPlans> GetPlanByIdAsync(int planId)
        {
            return await _context.membershipPlans.FindAsync(planId);
        }


        public async Task UpdatePlanAsync (MembershipPlans plan)
        {
            _context.membershipPlans.Update(plan);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlanAsync(int planId)
        {
            var plan = await _context.membershipPlans.FindAsync(planId);
            if (plan != null)
            {
                _context.membershipPlans.Remove(plan);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<UserMembership>> GetAllMembershipsWithUsersAndPlansAsync() 
        {
            return await _context.UserMembership
                .Include(a=>a.User)
                .Include(a=>a.MembershipPlans)
                .ToListAsync();
        }


        public async Task<UserMembership?> GetUserMembershipByUserIdAsync(int userId) 
        {
            return await _context.UserMembership
                .Include(a=>a.User)
                .Include(a=>a.MembershipPlans)
                .FirstOrDefaultAsync(a=>a.UserId==userId);
        }


    }
}
