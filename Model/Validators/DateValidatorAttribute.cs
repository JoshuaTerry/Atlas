using System;
using System.ComponentModel.DataAnnotations;

namespace DriveCentric.Core.Validators
{
    public class DateValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
            => Convert.ToDateTime(value) >= DateTime.Now;
    }
}