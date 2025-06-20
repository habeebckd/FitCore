using Application.Dto;
using Application.Dto.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.Rating
{
    public interface IGymRatingService
    {
        Task<ApiResponse<GymRatingViewDto>> AddRatingAsync(int userId, GymRatingCreateDto dto);
        Task<ApiResponse<List<GymRatingViewDto>>> GetAllRatingsAsync();
    }
}
