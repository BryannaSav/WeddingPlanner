using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding : BaseEntity
    {
        public int WeddingId { get; set; }
        public string NameOne { get; set; }
        public string NameTwo { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public List<RSVPList> GuestList { get; set; }
        public Wedding()
        {
            GuestList = new List<RSVPList>();
        }
    }
}