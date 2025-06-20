using Application.Interface.Reppo.Trainer;
using Domain.Model;
using FitCore_Manager.Model;
using infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class TrainerBookingRepository : ITrainerBookingRepository
    {
        private readonly AppDbContext _context;

        public TrainerBookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsPremiumMemberAsync(int userId)
        {
            var membership = await _context.UserMembership
                .Include(x => x.MembershipPlans)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

            return membership?.MembershipPlans?.PlanName == "Premium";
        }

        public async Task<bool> IsTrainerAvailableAsync(int trainerId, int timeSlotId, DateTime date)
        {
            return !await _context.TrainerTimeSlots.AnyAsync(x =>
                x.TrainerId == trainerId &&
                x.TimeSlotId == timeSlotId &&
                x.BookingDate.Date == date.Date);
        }

        public async Task<List<User>> GetAvailableTrainersAsync(int timeSlotId, DateTime date)
        {
            return await _context.Users
                .Where(u => u.Role == "Trainer" &&  
                            !_context.TrainerTimeSlots.Any(x =>
                                x.TrainerId == u.UserId &&
                                x.TimeSlotId == timeSlotId &&
                                x.BookingDate.Date == date.Date))
                .ToListAsync();
        }

        public async Task AddBookingAsync(TrainerTimeSlot booking)
        {
             _context.TrainerTimeSlots.Add (booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TrainerTimeSlot>> GetUserSessionsAsync(int userId)
        {
            return await _context.TrainerTimeSlots
                .Include(t => t.TimeSlot)
                .Include(t => t.Trainer)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }


        public async Task AddTrainerSlotAsync(TrainerTimeSlot slot)
        {
            await _context.TrainerTimeSlots.AddAsync(slot);
        }

        //public async Task<List<TrainerTimeSlot>> GetAvailableSlotsAsync()
        //{
        //    return await _context.TrainerTimeSlots
        //        .Include(s => s.Trainer)
        //        .Include(s => s.TimeSlot)
        //        .Where(s => s.UserId == null && s.BookingDate >= DateTime.Today)
        //        .ToListAsync();
        //}
        public async Task<List<TrainerTimeSlot>> GetAvailableSlotsAsync()
        {
            return await _context.TrainerTimeSlots
                .Include(s => s.Trainer)
                .Include(s => s.TimeSlot)
                .Where(s => s.UserId == null && s.BookingDate >= DateTime.Today)
                .ToListAsync();
        }



        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task AddAsync(TimeSlot timeSlot)
        {
            await _context.TimeSlots.AddAsync(timeSlot);
        }


        public async Task<List<User>> GetAllTrainersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Trainer" && !u.isBlocked)
                .ToListAsync();
        }



    }
}
