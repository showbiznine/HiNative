using HiNativeShared.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HiNative.Converters
{
    public class LanguageCountrySelector : DataTemplateSelector
    {
        public DataTemplate languagesTemplate { get; set; }
        public DataTemplate countriesTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ComboBox element = container as ComboBox;
            if (item is HNLanguage)
            {
                return languagesTemplate;
            }
            else if (item is HNCountry)
            {
                return countriesTemplate;
            }
            else return null;
        }
    }
}
