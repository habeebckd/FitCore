using Application.Dto;
using Application.Dto.Traiiner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.Trainer
{
    public interface ITrainerBookingService
    {
        Task<ApiResponse<string>> BookTrainerAsync(int userId, TrainerBookingRequestDto dto);
        Task<ApiResponse<List<TrainerSessionViewDto>>> GetUserSessionsAsync(int userId);

        Task<ApiResponse<string>> AddTraninerSlotAsync(CreateTrainerSlotDto dto);
        Task<ApiResponse<List<AvailableSlotViewDto>>> GetAvailableSlotsAsync();
        Task<ApiResponse<string>> AddTimeSlotAsync(CreateTimeSlotDto dto);

        Task<ApiResponse<List<TrainerDto>>> GetAllTrainersAsync();

    }
}
