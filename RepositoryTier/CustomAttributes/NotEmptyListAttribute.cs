using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.CustomAttributes
{
    public class NotEmptyListAttribute:ValidationAttribute
    {

        protected override ValidationResult?
            IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IEnumerable<object> list)
                return ValidationResult.Success;

            string listName = validationContext.DisplayName;
            return list.Count() > 0 ? ValidationResult.Success :
                new ValidationResult($"{listName} list should not be empty");
        }
    }
}
