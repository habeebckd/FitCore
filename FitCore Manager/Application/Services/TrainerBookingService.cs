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
            // 1. Check if user is premium
            if (!await _repo.IsPremiumMemberAsync(userId))
                return new ApiResponse<string>(false, "Only premium members can book trainers", null, null);

            // 2. Check membership expiry
            var membershipEndDate = await _repo.GetMembershipExpiryDateAsync(userId);
            if (membershipEndDate == null)
                return new ApiResponse<string>(false, "No active membership found", null, null);

            if (dto.BookingDate.Date > membershipEndDate.Value.Date)
                return new ApiResponse<string>(false, "Booking date exceeds membership expiry", null, null);

            // ✅ 3. Ensure user has not already booked a trainer during this membership period
            var alreadyBookedTrainer = await _repo.HasUserAlreadyBookedTrainerAsync(userId, membershipEndDate.Value);
            if (alreadyBookedTrainer)
                return new ApiResponse<string>(false, "You have already booked a trainer for your membership period", null, null);

            // ✅ 4. Ensure the selected trainer is not already booked at this time slot on this date
            var availableTrainers = await _repo.GetAvailableTrainersAsync(dto.TimeSlotId, dto.BookingDate);
            if (!availableTrainers.Any())
                return new ApiResponse<string>(false, "No trainers available for this time slot", null, null);

            var selectedTrainer = availableTrainers.FirstOrDefault(t => t.UserId == dto.TrainerId);
            if (selectedTrainer == null)
                return new ApiResponse<string>(false, "Selected trainer is not available for this slot", null, null);

            // 5. Book the trainer
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
                // ✅ Step 1: Check if this slot already exists
                var existing = await _repo.DoesSlotExistAsync(dto.TrainerId, dto.TimeSlotId);
                if (existing)
                {
                    return new ApiResponse<string>(false, "This slot already exists for the trainer on the same date", null, null);
                }

                // ✅ Step 2: Add new slot
                var slot = new TrainerTimeSlot
                {
                    TrainerId = dto.TrainerId,
                    TimeSlotId = dto.TimeSlotId,
                    //BookingDate = dto.BookingDate.Date,
                    UserId = 0
                };

                await _repo.AddTraninerSlotAsync(slot);
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
                    SlotId = S.TimeSlot.Id,
                    TrainerId= S.Trainer != null ? S.Trainer.UserId :0,
                    TrainerName = S.Trainer != null ? S.Trainer.UserName : "Unknown",
                    //BookingDate = S.BookingDate,
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


        public async Task<ApiResponse<string>> DeleteSlotByTrainerAsync(int trainerId, int timeSlotId)
        {
            var canDelete = await _repo.CanDeleteSlotAsync(trainerId, timeSlotId);

            if (!canDelete)
                return new ApiResponse<string>(false, "Cannot delete slot: booked by a premium member.", null, null);

            var deleted = await _repo.DeleteTrainerSlotAsync(trainerId, timeSlotId);

            if (!deleted)
                return new ApiResponse<string>(false, "Slot not found or already deleted", null, null);

            return new ApiResponse<string>(true, "Slot deleted successfully", "Deleted", null);
        }



    }
}
