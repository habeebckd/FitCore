using Application.Dto.Attantance;
using Application.Interface.Reppo.Attantance;
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
    public class TrainerPunchRepository : ITrainerPunchRepository
    {
        private readonly AppDbContext _context;

        public TrainerPunchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TrainerTimeSlot?> GetValidBookingSlotAsync(int userId)
        {
            var now = DateTime.UtcNow;
            var currentTime = TimeOnly.FromDateTime(now);

            return await _context.TrainerTimeSlots
                .Include(s => s.TimeSlot)
                .Include(s => s.User)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId &&
                    s.BookingDate.Date == now.Date &&
                    s.TimeSlot.StartTime <= currentTime &&
                    s.TimeSlot.EndTime >= currentTime);
        }


        public async Task<TrainerTimeSlotAttendance?> GetAttendanceAsync(int trainerTimeSlotId)
        {
            return await _context.TrainerTimeSlotAttendances
                .FirstOrDefaultAsync(a => a.TrainerTimeSlotId == trainerTimeSlotId);
        }


        public async Task<PunchResponseDto?> CreatePunchInAsync(int trainerTimeSlotId)
        {
            var slot = await _context.TrainerTimeSlots
                .Include(s => s.User)
                .Include(s => s.Trainer)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(s => s.Id == trainerTimeSlotId);

            if (slot == null)
                return null;

            var attendance = new TrainerTimeSlotAttendance
            {
                TrainerTimeSlotId = trainerTimeSlotId,
                PunchInTime = DateTime.UtcNow
            };

            _context.TrainerTimeSlotAttendances.Add(attendance);
            await _context.SaveChangesAsync();

            return new PunchResponseDto
            {
                UserName = slot.User?.UserName,
                TrainerName = slot.Trainer?.UserName,
                SessionId = slot.Id,
                StartTime = slot.TimeSlot.StartTime,
                EndTime = slot.TimeSlot.EndTime,
                BookingDate = slot.BookingDate
            };
        }

        public async Task<bool> CreatePunchOutAsync(int trainerTimeSlotId)
        {
            var attendance = await _context.TrainerTimeSlotAttendances
                .FirstOrDefaultAsync(a => a.TrainerTimeSlotId == trainerTimeSlotId);

            if (attendance == null || attendance.PunchInTime == null || attendance.PunchOutTime != null)
                return false;

            attendance.PunchOutTime = DateTime.UtcNow;

            _context.TrainerTimeSlotAttendances.Update(attendance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsPremiumUser(int userId)
        {
            var userMembership = await _context.UserMembership
                .Include(um => um.MembershipPlans)
                .Where(um => um.UserId == userId &&
                             um.IsActive &&
                             um.MembershipPlans.PlanName == "Premium" &&
                             um.StartDate <= DateTime.UtcNow &&
                             um.EndDate >= DateTime.UtcNow &&
                             um.PaymentStatus == "Completed")
                .FirstOrDefaultAsync();

            return userMembership != null;
        }

    }
}
