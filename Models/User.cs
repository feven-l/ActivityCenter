using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Display(Name="First Name")]
        [Required]
        [MinLength(2)]
        public string FirstName {get;set;}

        [Display(Name="Last Name")]
        [Required]
        [MinLength(2)]
        public string LastName {get;set;}

        [Display(Name="Email")]
        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Display(Name="Password")]
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least 1 number, 1 letter, and a special character")]

        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Display(Name="Confirm Password")]
        [Required]
        [MinLength(8)]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}

        public List<Plan> Involvedplans {get;set;}
        public List<Association> AllPlans {get;set;}

    }
}
