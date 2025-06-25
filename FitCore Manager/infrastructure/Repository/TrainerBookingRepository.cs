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
                    _context.TrainerTimeSlots.Any(ts =>
                        ts.TrainerId == u.UserId &&
                        ts.TimeSlotId == timeSlotId &&
                        ts.BookingDate.Date == date.Date &&
                        ts.UserId == 0 // Slot is available (not booked)
                    ) &&
                    !_context.TrainerTimeSlots.Any(ts =>
                        ts.TrainerId == u.UserId &&
                        ts.TimeSlotId == timeSlotId &&
                        ts.BookingDate.Date == date.Date &&
                        ts.UserId != 0 // Already booked by a user
                    ))
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


        public async Task<bool> AddTraninerSlotAsync(TrainerTimeSlot slot)
        {
            // Check if the trainer already has this slot on the same date
            var exists = await _context.TrainerTimeSlots.AnyAsync(ts =>
                ts.TrainerId == slot.TrainerId &&
                ts.TimeSlotId == slot.TimeSlotId &&
                ts.BookingDate.Date == slot.BookingDate.Date);

            if (exists)
                return false;

            await _context.TrainerTimeSlots.AddAsync(slot);
            return true;
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
       .Where(s =>
           s.UserId == 0 &&
           !_context.TrainerTimeSlots.Any(b =>
               b.TrainerId != s.TrainerId && // Exclude same trainer if needed
               b.TimeSlotId == s.TimeSlotId &&
               b.BookingDate.Date == s.BookingDate.Date &&
               b.UserId != 0 // means already booked
           )
       )
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




        public async Task<DateTime?> GetMembershipExpiryDateAsync(int userId)
        {
            var membership = await _context.UserMembership
                .Where(u => u.UserId == userId && u.IsActive)
                .OrderByDescending(a=>a.EndDate)
                .FirstOrDefaultAsync();

            return membership?.EndDate;
        }


        public async Task<bool> HasExistingBookingAsync(int userId,int timeSlotId,DateTime bookingDate)
        {
            return await _context.TrainerTimeSlots.AnyAsync(a=>
            a.UserId == userId &&
            a.TimeSlotId == timeSlotId &&
            a.BookingDate.Date == bookingDate.Date);
        }

        public async Task<bool> CanDeleteSlotAsync(int trainerId, int timeSlotId)
        {
            var slot = await _context.TrainerTimeSlots
                .Include(t => t.User)
                .Include(t => t.User.UserMembership)
                .FirstOrDefaultAsync(x =>
                    x.TrainerId == trainerId &&
                    x.TimeSlotId == timeSlotId &&
                    x.UserId != null);

            if (slot == null)
                return true; // No user booked it

            // Check if booked by a premium member
            var membership = await _context.UserMembership
                .Include(m => m.MembershipPlans)
                .FirstOrDefaultAsync(m => m.UserId == slot.UserId && m.IsActive);

            return membership == null || membership.MembershipPlans?.PlanName != "Premium";
        }



        public async Task<bool> DeleteTrainerSlotAsync(int trainerId, int timeSlotId)
        {
            var slot = await _context.TrainerTimeSlots.FirstOrDefaultAsync(s =>
                s.TrainerId == trainerId &&
                s.TimeSlotId == timeSlotId 
              );

            if (slot == null)
                return false;

            _context.TrainerTimeSlots.Remove(slot);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasUserAlreadyBookedTrainerAsync(int userId, DateTime membershipEndDate)
        {
            return await _context.TrainerTimeSlots
                .AnyAsync(b => b.UserId == userId && b.BookingDate <= membershipEndDate);
        }


        public async Task<bool> DoesSlotExistAsync(int trainerId, int timeSlotId)
        {
            return await _context.TrainerTimeSlots.AnyAsync(slot =>
                slot.TrainerId == trainerId &&
                slot.TimeSlotId == timeSlotId);
        }



    }
}
