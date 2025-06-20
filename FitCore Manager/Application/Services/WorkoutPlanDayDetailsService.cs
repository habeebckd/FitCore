using Application.Dto;
using Application.Dto.Workout;
using Application.Interface.Reppo.Workout;
using Application.Interface.Serv.Workout;
using AutoMapper;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkoutPlanDayDetailsService : IWorkoutPlanDayDetailsService
    {
        private readonly IWorkoutPlanDayDetailsRepository _repo;
        private readonly IMapper _mapper;
        public WorkoutPlanDayDetailsService(IWorkoutPlanDayDetailsRepository repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        public async Task<ApiResponse<ICollection<WorkoutPlanDayDetailsDto>>> GetAllAsync()
        {
            try
            {

                var data = await _repo.GetAll();
                var mapped = _mapper.Map<ICollection<WorkoutPlanDayDetailsDto>>(data);
                return new ApiResponse<ICollection<WorkoutPlanDayDetailsDto>>(true, "feched Sucsessfully", mapped, null);

            }
            catch(Exception ex) 
            {
                return new ApiResponse<ICollection<WorkoutPlanDayDetailsDto>>(false, "error occurred", null, ex.Message);
            }
        }

        public async Task<ApiResponse<WorkoutPlanDayDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var data = await _repo.GetById(id);
                if (data == null)
                    return new ApiResponse<WorkoutPlanDayDetailsDto>(false, "Not Found", null, null);
                var mapped = _mapper.Map<WorkoutPlanDayDetailsDto>(data);
                return new ApiResponse<WorkoutPlanDayDetailsDto>(true, "Fetched Succsessfully", mapped, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<WorkoutPlanDayDetailsDto>(false, "Erprr occurred", null, ex.Message);
            }
        }


        public async Task<ApiResponse<string>> CreateAsync(CreateWorkoutPlanDayDetailsDto dto)
        {
            try
            {
                var exists = await _repo.ExistsAsync(dto.WorkoutPlanId, dto.WeekNumber, dto.DayNumber);
                if (exists)
                {
                    return new ApiResponse<string>(false, "Workout already exists for the selected plan, week, and day.", null, null);
                }

                var model = _mapper.Map<WorkoutPlanDayDetails>(dto);
                await _repo.AddAsync(model);
                await _repo.SaveAsync();

                return new ApiResponse<string>(true, "Created successfully", "Done", null);
            }
            catch (DbUpdateException dbEx)
            {
                return new ApiResponse<string>(false, "Duplicate entry detected by database constraint.", null, dbEx.Message);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(false, "An unexpected error occurred", null, ex.Message);
            }
        }




       public async Task<ApiResponse<List<WorkoutPlanDayDetailsDto>>> GetUserWorkoutPlanDetailsAsync(int userId)
{
    try
    {
        // Step 1: Get the user's assigned workout plan
        var userWorkoutPlan = await _repo.GetUserWorkoutPlanAsync(userId);
        if (userWorkoutPlan == null)
            return new ApiResponse<List<WorkoutPlanDayDetailsDto>>(false, "Workout plan not assigned", null, null);

        // Step 2: Fetch all day details using the WorkoutPlanId
        var planDetails = await _repo.GetWorkoutPlanDayDetailsAsync(userWorkoutPlan.WorkoutPlanId);

        // Step 3: Map to DTO
        var dto = _mapper.Map<List<WorkoutPlanDayDetailsDto>>(planDetails);
        return new ApiResponse<List<WorkoutPlanDayDetailsDto>>(true, "Fetched successfully", dto, null);
    }
    catch (Exception ex)
    {
        return new ApiResponse<List<WorkoutPlanDayDetailsDto>>(false, "Error occurred", null, ex.Message);
    }
}





    }
}
