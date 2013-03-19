using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace SkypeTest
{
    #region AvatarUriConverter
    /// <summary>
    /// A converter for returning a placeholder url for contacts that don't provide an avatar uri
    /// </summary>
    public class AvatarUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.IsNullOrWhiteSpace((value as String)) ? "Images/Avatar_64x64_male.png" : (value as String);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region StatusToBrushConverter
    /// <summary>
    /// A converter for changing the status value of a contact to a predefined colour.
    /// In this appliation, online==green, away==orange and offline==gray
    /// </summary>
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return null;

            String status = value as string;
            if (status == "online")
            {
                return new SolidColorBrush(Color.FromArgb(255, 128, 201, 8));
            }
            else if (status == "away")
            {
                return new SolidColorBrush(Color.FromArgb(255, 245, 194, 0));
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 167, 167, 167));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

}
