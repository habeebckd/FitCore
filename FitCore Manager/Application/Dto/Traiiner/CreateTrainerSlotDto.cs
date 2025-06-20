using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Traiiner
{
    public class CreateTrainerSlotDto
    {
        public int TrainerId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
