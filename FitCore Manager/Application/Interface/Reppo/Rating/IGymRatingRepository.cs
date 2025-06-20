using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Reppo.Rating
{
    public interface IGymRatingRepository
    {
        Task<Feedback> AddRatingAsync(Feedback rating);
        Task<List<Feedback>> GetAllRatingsAsync();
    }
}
