using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Attantance
{
    public class PunchResponseDto
    {
        public string UserName { get; set; }
        public string TrainerName { get; set; }
        public int SessionId { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
