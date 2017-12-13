namespace WeddingPlanner.Models{

    public class RSVPList : BaseEntity
    {
        public int RSVPListId { get; set; }
        public bool RSVPStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }

    }
}

