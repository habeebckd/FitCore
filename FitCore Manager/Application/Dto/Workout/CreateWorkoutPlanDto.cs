using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Workout
{
    public class CreateWorkoutPlanDto
    {
        public string Title {  get; set; }
        public string Description { get; set; }
        public int DurationInWeeks {  get; set; }
        public string Goal {  get; set; }
        public string Type { get; set; }

    }
}
