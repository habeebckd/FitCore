using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationInWeeks {  get; set; }
        public string Goal {  get; set; }
        public string Type { get; set; }


        public bool IsActive {  get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;


        public ICollection<UserWorkoutPlan> UserWorkoutPlans {  get; set; }
        public ICollection<WorkoutPlanDayDetails> WorkoutPlanDayDetails { get; set; }

    }
}
