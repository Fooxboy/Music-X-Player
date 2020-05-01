using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Controls
{
    public sealed partial class RepeatButton : UserControl
    {

        public RepeatButton()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty RepeatModeProperty = 
            DependencyProperty.Register("RepeatMode", typeof(int), typeof(RepeatButton), new PropertyMetadata(0));

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var currentRepeatMode = RepeatMode +1;
            if (currentRepeatMode > 2) currentRepeatMode = 0;
           

            var button = (ToggleButton)sender;

            if (currentRepeatMode == 0)
            {
                button.IsChecked = false;
                Repeat.Visibility = Visibility.Visible;
                RepeatOne.Visibility = Visibility.Collapsed;
            }
            else if (currentRepeatMode == 1)
            {
                button.IsChecked = true;
                Repeat.Visibility = Visibility.Collapsed;
                RepeatOne.Visibility = Visibility.Visible;

            }
            else if (currentRepeatMode == 2) 
            {
                button.IsChecked = true;
                Repeat.Visibility = Visibility.Visible;
                RepeatOne.Visibility = Visibility.Collapsed;
            }

            if (RepeatMode == 2) RepeatMode = 0;
            else RepeatMode += 1;
        }

        public int RepeatMode
        {
            get => (int)GetValue(RepeatModeProperty);
            set
            {
                SetValue(RepeatModeProperty, value);
            }
        }


        private void RepeatButton_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (RepeatMode == 0)
            {
                Button.IsChecked = false;
                Repeat.Visibility = Visibility.Visible;
                RepeatOne.Visibility = Visibility.Collapsed;
            }
            else if (RepeatMode == 1)
            {
                Button.IsChecked = true;
                Repeat.Visibility = Visibility.Collapsed;
                RepeatOne.Visibility = Visibility.Visible;

            }
            else if (RepeatMode == 2)
            {
                Button.IsChecked = true;
                Repeat.Visibility = Visibility.Visible;
                RepeatOne.Visibility = Visibility.Collapsed;
            }
        }
    }
}
