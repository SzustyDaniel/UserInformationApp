using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace UserInformationApp.Resources.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convert from string value to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parameterString = parameter as string;
            //Test for valid value conversion
            if (parameterString == null)
                return DependencyProperty.UnsetValue;


            return parameterString.Equals(value as string);
        }

        /// <summary>
        /// convert back from bool to string value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parameterString = parameter as string;

            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return parameterString;
        }
    }
}
