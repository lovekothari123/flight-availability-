using System;
using System.Globalization;
using Xamarin.Forms;

namespace FLightsApp.Models
{
    public class BackgroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            return Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // You probably don't need this, this is used to convert the other way around
            // so from color to yes no or maybe
            throw new NotImplementedException();
        }
    }
}
