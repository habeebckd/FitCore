using Application.Dto;
using Application.Dto.Traiiner;
using Application.Interface.Reppo.Trainer;
using Application.Interface.Serv.Trainer;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TrainerBookingService : ITrainerBookingService
    {
        private readonly ITrainerBookingRepository _repo;
        private readonly IMapper _mapper;

        public TrainerBookingService(ITrainerBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> BookTrainerAsync(int userId, TrainerBookingRequestDto dto)
        {
            if (!await _repo.IsPremiumMemberAsync(userId))
                return new ApiResponse<string>(false, "Only premium members can book trainers", null, null);

            var availableTrainers = await _repo.GetAvailableTrainersAsync(dto.TimeSlotId, dto.BookingDate);

            if (!availableTrainers.Any())
                return new ApiResponse<string>(false, "No trainers available for this time slot", null, null);

            var selectedTrainer = availableTrainers.First();

            var booking = new TrainerTimeSlot
            {
                BookingDate = dto.BookingDate,
                TimeSlotId = dto.TimeSlotId,
                UserId = userId,
                TrainerId = selectedTrainer.UserId
            };

            await _repo.AddBookingAsync(booking);

            return new ApiResponse<string>(true, "Booking successful", "Trainer booked", null);
        }

        public async Task<ApiResponse<List<TrainerSessionViewDto>>> GetUserSessionsAsync(int userId)
        {
            var sessions = await _repo.GetUserSessionsAsync(userId);
            return new ApiResponse<List<TrainerSessionViewDto>>(
                     true,
                     "Fetched successfully",
                      sessions.Select(s => new TrainerSessionViewDto
                      {
                       TrainerName = s.Trainer.UserName,
                       StartTime = s.TimeSlot.StartTime,
                        EndTime = s.TimeSlot.EndTime,
                       BookingDate = s.BookingDate
                        }).ToList(),
                       null
                      );

        }


        public async Task<ApiResponse<string>> AddTraninerSlotAsync(CreateTrainerSlotDto dto)
        {
            try
            {
                var slot = new TrainerTimeSlot
                {
                    TrainerId = dto.TrainerId,
                    TimeSlotId = dto.TimeSlotId,
                    BookingDate = dto.BookingDate.Date,
                    UserId = null 
                };

                await _repo.AddTrainerSlotAsync(slot);
                await _repo.SaveAsync();

                return new ApiResponse<string>(true, "Slot added", "Done", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(false, "Error", null, ex.Message);
            }
        }

        //public async Task<ApiResponse<List<AvailableSlotViewDto>>> GetAvailableSlotsAsync()
        //{
        //    try
        //    {
        //        var slots = await _repo.GetAvailableSlotsAsync();
        //        var result = _mapper.Map<List<AvailableSlotViewDto>>(slots);
        //        return new ApiResponse<List<AvailableSlotViewDto>>(true, "Available slots", result, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<List<AvailableSlotViewDto>>(false, "Error", null, ex.Message);
        //    }
        //}

        public async Task<ApiResponse<List<AvailableSlotViewDto>>> GetAvailableSlotsAsync()
        {
            try
            {
                var slots = await _repo.GetAvailableSlotsAsync();
                //var result = _mapper.Map<List<AvailableSlotViewDto>>(slots);
                var result = slots.Select(S => new AvailableSlotViewDto
                {
                    SlotId = S.Id,
                    TrainerName = S.Trainer != null ? S.Trainer.UserName : "Unknown",
                    BookingDate = S.BookingDate,
                    StartTime = S.TimeSlot != null ? S.TimeSlot.StartTime : TimeOnly.MinValue,
                    EndTime = S.TimeSlot != null ? S.TimeSlot.EndTime : TimeOnly.MinValue
                }).ToList();
                return new ApiResponse<List<AvailableSlotViewDto>>(true, "Available slots", result, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AvailableSlotViewDto>>(false, "Error", null, ex.Message);
            }
        }



        public async Task<ApiResponse<string>> AddTimeSlotAsync(CreateTimeSlotDto dto)
        {
            try
            {
                var model = _mapper.Map<TimeSlot>(dto);
                await _repo.AddAsync(model);
                await _repo.SaveAsync();
                return new ApiResponse<string>(true, "Time slot added", "Success", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(false, "Error adding time slot", null, ex.Message);
            }
        }


        public async Task<ApiResponse<List<TrainerDto>>> GetAllTrainersAsync()
        {
            try
            {
                var trainers = await _repo.GetAllTrainersAsync();
                var dto = _mapper.Map<List<TrainerDto>>(trainers);
                return new ApiResponse<List<TrainerDto>>(true, "Fetched trainers", dto, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TrainerDto>>(false, "Error", null, ex.Message);
            }
        }
    }
}
