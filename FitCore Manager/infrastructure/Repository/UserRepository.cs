using Domain.Model;
using FitCore_Manager.Model;
using infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserMembership?> GetUserMembershipWithPlanAsync(int userId)
        {
            return await _context.UserMembership
                .Include(um => um.MembershipPlans)
                .FirstOrDefaultAsync(um =>
                    um.UserId == userId &&
                    um.IsActive &&
                    um.StartDate <= DateTime.Now &&
                    um.EndDate >= DateTime.Now
                );
        }

    }
}
