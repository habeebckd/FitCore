using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Traiiner
{
    public class TrainerBookingRequestDto
    {
        public int TimeSlotId { get; set; }
        public int TrainerId {  get; set; }
        public DateTime BookingDate { get; set; }
    }
}
