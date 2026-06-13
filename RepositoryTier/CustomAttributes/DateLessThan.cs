using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.CustomAttributes
{
    public class DateLessThanAttribute : ValidationAttribute
    {
        private string _probertyName;
        public DateLessThanAttribute(string probertyName)
        {
            _probertyName = probertyName;
        } 

        protected override ValidationResult?
            IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateOnly smallerDate)
                return ValidationResult.Success;

            var otherProperty = validationContext.ObjectType
            .GetProperty(_probertyName);

            if (otherProperty is null)
            {
                return new ValidationResult(
                    $"Property '{_probertyName}' not found.");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (otherValue is not DateOnly greaterDate)
                return ValidationResult.Success;

            if (smallerDate >= greaterDate)
            {
                return new ValidationResult($"{validationContext.DisplayName}" +
                    $" must be earlier than {_probertyName}.");
            }

            return ValidationResult.Success;
        }
    }
}
