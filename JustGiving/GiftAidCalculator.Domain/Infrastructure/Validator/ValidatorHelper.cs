using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftAidCalculator.Domain.Infrastructure.Validator
{
    public class ValidatorHelper
    {
        public static EntityValidationResult IsValidEntity<T>(T entity) where T : class
        {
            try
            {
                var validationResults = new List<ValidationResult>();

                var vc = new ValidationContext(entity, null, null);

                bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(entity, vc, validationResults, true);

                return new EntityValidationResult { IsValid = isValid, ValidationResult = validationResults };
            }
            catch (Exception ex)
            {
                return new EntityValidationResult { IsValid = false, ValidationResult = new List<ValidationResult> {new ValidationResult(ex.Message) } };
            }
        }
    }
}
