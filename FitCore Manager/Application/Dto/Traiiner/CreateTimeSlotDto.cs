using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Traiiner
{
    public class CreateTimeSlotDto
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public CreateTimeSlotDto()
        {
            var now = DateTime.Now.TimeOfDay;
            StartTime = TimeOnly.FromTimeSpan(now);
            EndTime = TimeOnly.FromTimeSpan(now.Add(TimeSpan.FromHours(1)));
        }
    }
}
