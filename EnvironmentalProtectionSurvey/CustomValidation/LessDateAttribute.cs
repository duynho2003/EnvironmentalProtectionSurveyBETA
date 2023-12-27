using System.ComponentModel.DataAnnotations;

namespace EnvironmentalProtectionSurvey.CustomValidation
{
    public class LessDateAttribute : ValidationAttribute
    {
        public LessDateAttribute() : base("{0} Date should greater than past")
        {

        }

        public override bool IsValid(object value)
        {
            DateTime? date = value as DateTime?;
            if (date == null)
            {
                return true; // Allow null values
            }

            if (date.Value < DateTime.Today)
            {
                ErrorMessage = "Date should be greater than or equal to today.";
                return false;
            }
            return true;
        }
    }
}
