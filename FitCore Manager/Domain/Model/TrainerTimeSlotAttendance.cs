using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class TrainerTimeSlotAttendance
    {

        public int Id { get; set; }

        public int TrainerTimeSlotId { get; set; }
        public DateTime? PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }


        public virtual TrainerTimeSlot TrainerTimeSlot { get; set; }

    }
}
