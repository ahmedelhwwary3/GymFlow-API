using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.CustomAttributes
{
    public class NotFutureDate:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            if (value is not DateOnly date)
                return ValidationResult.Success;

            return date > DateOnly.FromDateTime(DateTime.UtcNow)
                ? new ValidationResult("Date can not be in the future") 
                : ValidationResult.Success;
        }
    }
}
