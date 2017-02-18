using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HiNative.Converters
{
    class ActivityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var b = (bool)value;
            if (b)
            {
                var color = (Color)Application.Current.Resources["HiNativeOrange"];
                return new SolidColorBrush(color);
            }
            else
            {
                var color = (Color)Application.Current.Resources["SystemBaseMediumColor"];
                return new SolidColorBrush(color);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
