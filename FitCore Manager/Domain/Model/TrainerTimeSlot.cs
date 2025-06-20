using FitCore_Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class TrainerTimeSlot
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }
        public int TimeSlotId { get; set; }
        public int? UserId { get; set; }
        public DateTime BookingDate { get; set; }



        public virtual User User { get; set; }

        public virtual TimeSlot TimeSlot { get; set; }
        public virtual User Trainer { get; set; }

    }
}
