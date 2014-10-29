using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GiftAidCalculator.Domain.Infrastructure.Validator
{
    public class EntityValidationResult
    {
        public bool IsValid { get; set; }
        public List<ValidationResult> ValidationResult { get; set; }

        public string Errors
        {
            get
            {
                if (!IsValid && ValidationResult.Any())
                    return String.Join(" , ",ValidationResult.Select(c => string.Format("{0} - {1}", c.MemberNames, c.ErrorMessage)));
                return string.Empty;
            }
        }
        
    }
}
