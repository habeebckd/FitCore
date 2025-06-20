using Domain.Model;
using FitCore_Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.Trainer
{
    public interface ITrainerBookingRepository
    {
        Task<bool> IsPremiumMemberAsync(int userId);
        Task<bool> IsTrainerAvailableAsync(int trainerId, int timeSlotId, DateTime date);
        Task<List<User>> GetAvailableTrainersAsync(int timeSlotId, DateTime date);
        Task AddBookingAsync(TrainerTimeSlot booking);
        Task<List<TrainerTimeSlot>> GetUserSessionsAsync(int userId);

        Task AddTrainerSlotAsync(TrainerTimeSlot slot);
        Task<List<TrainerTimeSlot>> GetAvailableSlotsAsync();
        Task SaveAsync();
        Task AddAsync(TimeSlot timeSlot);

        Task<List<User>> GetAllTrainersAsync();


    }
}
