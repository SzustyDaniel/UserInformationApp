using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppData.ValidationRules
{
    /// <summary>
    /// Validation of the phone number given in the textboxes
    /// </summary>
    public class ValidPhoneNumberRule : ValidationRule
    {
        private const char HYPHEN = '-';
        private const int PHONENUMBERLENGTH = 10;
        private const int SUBCOUNT = 3;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneNumber = value as string;
            string[] subParts = phoneNumber.Split('-');
            int numCount = 0;

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ValidationResult(false, "Phone number must have a value");

            if (subParts.Length != SUBCOUNT)
                return new ValidationResult(false, "Phone number must be in format of ###-###-####");

            foreach (string part in subParts)
            {
                for (int i = 0; i < part.Length; i++)
                {
                    if (!char.IsDigit(part[i]))
                        return new ValidationResult(false, "Phone number cannot contain letters");

                    numCount++;
                }
            }

            if (numCount != PHONENUMBERLENGTH)
                return new ValidationResult(false, $"Phone number contains {numCount}, it must have 10 digits");

            return new ValidationResult(true, null);
        }
    }
}
