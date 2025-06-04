using FitCore_Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserMembership
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PlanId { get; set; }

        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string PaymentStatus { get; set; } = "Pending";


        public virtual MembershipPlans? MembershipPlans { get; set; }
        public virtual User? User { get; set; }

    }
}
