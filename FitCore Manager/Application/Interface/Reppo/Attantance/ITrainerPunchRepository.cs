using Application.Dto.Attantance;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.Attantance
{
    public interface ITrainerPunchRepository
    {
        Task<TrainerTimeSlot?> GetValidBookingSlotAsync(int userId);
        Task<TrainerTimeSlotAttendance?> GetAttendanceAsync(int trainerTimeSlotId);
        Task<PunchResponseDto?> CreatePunchInAsync(int trainerTimeSlotId);
        Task<bool> CreatePunchOutAsync(int trainerTimeSlotId);
    }
}
