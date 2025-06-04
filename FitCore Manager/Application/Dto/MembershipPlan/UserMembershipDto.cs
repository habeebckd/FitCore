using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.MembershipPlan
{
    public class UserMembershipDto
    {

        //public int PlanId { get; set; }
        //public string PlanName { get; set; }
        //public string Description { get; set; }
        //public decimal Price { get; set; }
        //public int DurationInDays { get; set; }
        public int UserMembershipId { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

    }

}
