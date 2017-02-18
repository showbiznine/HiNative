using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HiNative.Controls
{
    public sealed partial class FluencyControl : UserControl
    {
        static Brush _orange = new SolidColorBrush(Color.FromArgb(255, 255, 112, 64));
        static Brush _grey = new SolidColorBrush(Color.FromArgb(128, 128, 128, 128));

        public FluencyControl()
        {
            this.InitializeComponent();

            FluencyLevel = 0;

            //this.Loading += OnLoading;
            //this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            throw new NotImplementedException();
        }

        public static readonly DependencyProperty FluencyLevelProperty = 
            DependencyProperty.Register("Fluency Level", typeof(int), 
                typeof(FluencyControl), 
                new PropertyMetadata(0, new PropertyChangedCallback(LevelChanged)));

        public static void LevelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FluencyControl item = sender as FluencyControl;
            int newval = (int)e.NewValue;

            var rec1 = (Rectangle)item.FindName("rec1");
            var rec2 = (Rectangle)item.FindName("rec2");
            var rec3 = (Rectangle)item.FindName("rec3");
            var rec4 = (Rectangle)item.FindName("rec4");

            switch (newval)
            {
                case 0:
                    rec1.Fill = _grey;
                    rec2.Fill = _grey;
                    rec3.Fill = _grey;
                    rec4.Fill = _grey;
                    break;
                case 1:
                    rec1.Fill = _orange;
                    rec2.Fill = _grey;
                    rec3.Fill = _grey;
                    rec4.Fill = _grey;
                    break;
                case 2:
                    rec1.Fill = _orange;
                    rec2.Fill = _orange;
                    rec3.Fill = _grey;
                    rec4.Fill = _grey;
                    break;
                case 3:
                    rec1.Fill = _orange;
                    rec2.Fill = _orange;
                    rec3.Fill = _orange;
                    rec4.Fill = _grey;
                    break;
                case 4:
                    rec1.Fill = _orange;
                    rec2.Fill = _orange;
                    rec3.Fill = _orange;
                    rec4.Fill = _orange;
                    break;
                default:
                    break;
            }
        }

        private int _max = 4;

        public int FluencyLevel
        {
            get
            {
                return (int)GetValue(FluencyLevelProperty);
            }
            set
            {
                if (value < 0)
                {
                    SetValue(FluencyLevelProperty, 0);
                }
                else if (value > _max)
                {
                    SetValue(FluencyLevelProperty, _max);
                }
                else
                {
                    SetValue(FluencyLevelProperty, value);
                }
            }
        }

    }
}
