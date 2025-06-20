using Application.Dto;
using Application.Dto.Rating;
using Application.Interface.Reppo.Rating;
using Application.Interface.Serv.Rating;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GymRatingService : IGymRatingService
    {
        private readonly IGymRatingRepository _repository;
        private readonly IMapper _mapper;
        public GymRatingService(IGymRatingRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GymRatingViewDto>> AddRatingAsync(int userId, GymRatingCreateDto dto)
        {
            var rating = _mapper.Map<Feedback>(dto);
            rating.UserId = userId;
            var result = await _repository.AddRatingAsync(rating);
            var mapped = _mapper.Map<GymRatingViewDto>(dto);
            return new ApiResponse<GymRatingViewDto>(true, "Thank you for your feedback!",mapped,null);
        }


        public async Task<ApiResponse<List<GymRatingViewDto>>> GetAllRatingsAsync()
        {
            var ratings = await _repository.GetAllRatingsAsync();
            var dtolist = _mapper.Map<List<GymRatingViewDto>>(ratings);
            return new ApiResponse<List<GymRatingViewDto>>(true, "All ratings", dtolist, null);
        }

    }
}
