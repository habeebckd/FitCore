using Domain.Model;

namespace FitCore_Manager.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string Phone {  get; set; }
        public string Role { get; set; }
        public bool isBlocked { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual UserMembership UserMembership { get; set; }
        public virtual ICollection<UserWorkoutPlan> UserWorkoutPlans { get; set; }
        public virtual ICollection<TrainerTimeSlot> TrainerBookings { get; set; } // As Trainer
        //public virtual ICollection<TrainerTimeSlot> UserTrainerBookings { get; set; } // As User




    }
}
