using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.CustomAttributes
{
    public class AgeRangeAttribute:ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult? IsValid(
            object? value, 
            ValidationContext validationContext)
        {
            //No validation if incorrect date (Error with ModelBinding)
            if (value is not DateOnly dateOfBirth)
                return ValidationResult.Success;

            var today = DateOnly.FromDateTime(DateTime.Today);

            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
                age--;

            // ValidationResult means Invalid
            if (age < _minAge || age > _maxAge)
            {
                return new ValidationResult(
                    $"Age must be between {_minAge} and {_maxAge} years.");
            }
            //Success and modelBinder will bind correctly
            return ValidationResult.Success;
        }
    }
}
