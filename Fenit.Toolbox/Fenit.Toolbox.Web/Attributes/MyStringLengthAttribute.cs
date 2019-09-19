using System;
using System.ComponentModel.DataAnnotations;

namespace Fenit.Toolbox.Web.Attributes
{
    public class MyStringLengthAttribute : StringLengthAttribute
    {
        public MyStringLengthAttribute(int maximumLength, int minLength)
            : base(maximumLength)
        {
            MinimumLength = minLength;
        }

        public override bool IsValid(object value)
        {
            var val = Convert.ToString(value);
            if (val.Length < MinimumLength || val.Length > MaximumLength)
            {
                base.ErrorMessage = "Błąd";
            }
            return base.IsValid(value);
        }
    }
}
