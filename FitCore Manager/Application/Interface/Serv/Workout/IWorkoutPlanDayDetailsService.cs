using Application.Dto;
using Application.Dto.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.Workout
{
    public interface IWorkoutPlanDayDetailsService
    {
        Task<ApiResponse<ICollection<WorkoutPlanDayDetailsDto>>> GetAllAsync();
        Task<ApiResponse<WorkoutPlanDayDetailsDto>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(CreateWorkoutPlanDayDetailsDto dto);

        Task<ApiResponse<List<WorkoutPlanDayDetailsDto>>> GetUserWorkoutPlanDetailsAsync(int userId);

    }
}
