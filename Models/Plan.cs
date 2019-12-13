using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            var dt = value as DateTime? ?? new DateTime();
            if(dt < DateTime.Today){
                return new ValidationResult("Date cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }
    public class Plan
    {
        [Key]
        public int PlanId {get;set;}

        [Required]
        public string Title {get;set;}

        [Required]
        
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}

        [Required]
        [FutureDate]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}

        [Required]
        [Range(1, 10000, ErrorMessage="Cannot be a negative number.")]
        public int Duration {get;set;}

        [Required]
        public string DurationDescription {get;set;} 

        [Required]
        public string Description {get;set;}

        [Required]
        public int UserId {get;set;}

        public User Creator {get;set;}

        public List<Association> participants {get;set;}
    }
}