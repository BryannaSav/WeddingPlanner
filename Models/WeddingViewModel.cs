using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class WeddingViewModel : BaseEntity
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        [MinLength(2)]
        public string NameOne { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        [MinLength(2)]
        public string NameTwo { get; set; }

        [Required]
        [FutureDate]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [MinLength(8)]
        public string Address { get; set; }
    }

    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            if(date < DateTime.Now){
               return new ValidationResult("Invalid Date");
            }
            else{
                return ValidationResult.Success;
            }
        }
    }
}