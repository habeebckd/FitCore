using Application.Interface.Reppo.Rating;
using Domain.Model;
using infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class GymRatingRepository : IGymRatingRepository
    {
        private readonly AppDbContext _context;
        public GymRatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Feedback> AddRatingAsync(Feedback rating)

        {
            _context.feedbacks.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<List<Feedback>> GetAllRatingsAsync()

        {
            return await _context.feedbacks.ToListAsync();
        }

    }
}
