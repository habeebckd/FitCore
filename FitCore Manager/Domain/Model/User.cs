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
        public DateTime CreatedAt { get; set; }

        public virtual UserMembership UserMembership { get; set; }
    }
}
