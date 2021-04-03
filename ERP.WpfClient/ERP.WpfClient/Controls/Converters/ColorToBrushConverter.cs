using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ERP.WpfClient.Controls.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Brushes.Black; // Default color

            var color = (Color)ColorConverter.ConvertFromString(value.ToString());

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
