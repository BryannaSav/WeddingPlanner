using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<RSVPList> WeddingsToAttend { get; set; }
        public List<Wedding> MyWeddings { get; set; }
        public User()
        {
            WeddingsToAttend = new List<RSVPList>();
            MyWeddings = new List<Wedding>();
        }
    }
}