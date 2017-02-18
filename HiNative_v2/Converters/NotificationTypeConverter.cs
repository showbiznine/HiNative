using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HiNative.Converters
{
    class NotificationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var type = (string)value;
            switch (type)
            {
                case "Answer":
                    return "answered";
                case "Mention":
                    return "mentioned you in";
                case "Like":
                    return "liked your answer to";
                default:
                    return "did something to";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
