using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Workout
{
    public class CreateWorkoutPlanDayDetailsDto
    {
        public int WorkoutPlanId { get; set; }
        public int WeekNumber { get; set; }
        public int DayNumber {  get; set; }
        public string WorkoutDetails { get; set; }
        public string DietPlan {  get; set; }
    }
}
