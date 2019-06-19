using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Wedding_Planner.Models {
    public class User {
        [Key]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email:")]
        public string Email { get; set; }
        [Required]
        [DataType (DataType.Password)]
        [MinLength(8)]
        [Display(Name = "Password:")]
        public string Password { get; set; }
        [NotMapped]
        [Compare ("Password")]
        [DataType (DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPW { get; set; }
        public List<Rsvp> Rsvps { get; set; }
        public User() {
            Rsvps = new List<Rsvp>();
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}