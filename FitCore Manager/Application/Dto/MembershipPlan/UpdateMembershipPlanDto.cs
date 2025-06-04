using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.MembershipPlan
{
    public class UpdateMembershipPlanDto
    {
        public int PlanId {  get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }
}
