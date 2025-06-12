using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Workout
{
    public class UserWorkoutPlanDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string WorkoutGoal { get; set; }
        public string PlanTitle { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
