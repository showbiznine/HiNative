using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HiNative.Converters
{
    class NativeAnswersColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var blue = new SolidColorBrush { Color = (Color)Application.Current.Resources["HiNativeBlue"] };
            var orange = new SolidColorBrush { Color = (Color)Application.Current.Resources["HiNativeOrange"] };

            SolidColorBrush brush = (int)value > 0 ? orange : blue;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
