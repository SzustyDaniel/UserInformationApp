using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppData.ValidationRules
{
    public class ValidEmailRule : ValidationRule
    {
        private char atChar = '@';

        public List<string> ValidEndings { get; set; } = new List<string>() { ".com", ".co", ".org", ".edu", ".gov", ".or", ".net" };

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string address = value.ToString();

            try
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (address.Contains(atChar))
                    {
                        var parts = address.Split(atChar);

                        if (parts[0].Length == 0 && string.IsNullOrEmpty(parts[0]))
                        {
                            return new ValidationResult(false, "Email must have a name longer than 0 ###@###.###");
                        }
                        else
                        {
                            var sub = parts[1].Split('.');

                            if (string.IsNullOrWhiteSpace(sub[0]))
                                return new ValidationResult(false, "Email domain must be longer than 1");

                            bool valid = false;
                            foreach (string ending in ValidEndings)
                            {
                                if (parts[1].ToLower().Contains(ending))
                                {
                                    valid = true;
                                    break;
                                }
                            }

                            if (!valid)
                                return new ValidationResult(false, "Email must end with acceptable ending ");
                            else
                                return new ValidationResult(true, null);
                        }
                    }
                    else
                    {
                        return new ValidationResult(false, "Email must contain @");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Email is not valid");
                }
            }
            catch
            {
                return new ValidationResult(false, "Email is not valid due to error");
            }
        }
    }
}
