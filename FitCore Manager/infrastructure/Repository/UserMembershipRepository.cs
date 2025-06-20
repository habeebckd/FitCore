using Application.Interface.Reppo.membership;
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
    public class UserMembershipRepository : IUserMembershipRepository
    {
        private readonly AppDbContext _context;
        public UserMembershipRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserMembership> GetActiveMembershipByUserIdAsync(int userId)
        {
            return await _context.UserMembership.Include(s => s.MembershipPlans).FirstOrDefaultAsync(a => a.UserId == userId && a.IsActive);
        }
        public async Task AddUserMembershipAsync(UserMembership membership)
        {
            await _context.UserMembership.AddAsync(membership);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateUserMembershipAsync(UserMembership membership)
        {
            _context.UserMembership.Update(membership);
            await _context.SaveChangesAsync();
        }


    }
}
