using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class MembershipPlans
    {
        public int Id { get; set; }
        public string? PlanName { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<UserMembership>? UserMembership { get; set; }
    }
}
