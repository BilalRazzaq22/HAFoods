using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ERP.WpfClient.Controls.Converters
{
    public class InvertBooleanVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility) || !(value is bool))
                throw new InvalidOperationException("Source must be boolean and target must be Visibility");
            if (!(bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("Source must be Visibility and target must be boolean when converting back");
            }
            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return false;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                    return true;
                default:
                    return true;
            }
        }

        #endregion
    }
}
