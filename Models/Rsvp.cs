using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Wedding_Planner.Models {
    public class Rsvp {
        public int RsvpId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }
    }
}