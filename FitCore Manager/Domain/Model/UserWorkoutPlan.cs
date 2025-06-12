using FitCore_Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserWorkoutPlan
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int WorkoutPlanId { get; set; }
        public DateTime AssignedDate { get; set; } =  DateTime.UtcNow;
        public bool IsCompleted { get; set; }


        public virtual User User { get; set; }
        public virtual WorkoutPlan WorkoutPlan { get; set; }

    }
}
