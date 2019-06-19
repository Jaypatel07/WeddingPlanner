using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Wedding_Planner.Models {
    public class Wedding {
        public int WeddingId { get; set; }
        [Required]
        [Display(Name = "Wedder One:")]
        public string WedderOne { get; set; }
        [Required]
        [Display(Name = "Wedder Two:")]
        public string WedderTwo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Wedding Date:")]
        public DateTime WeddingDate { get; set; }
        [Required]
        [Display(Name = "Wedding Address:")]
        public string Address { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Rsvp> Guests { get; set; }
        public Wedding() {
            Guests = new List<Rsvp>();
        }
    }
}