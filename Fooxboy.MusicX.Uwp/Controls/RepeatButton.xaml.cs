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
            var button = (ToggleButton)sender;
            if (RepeatMode == 2) RepeatMode = 0;
            else RepeatMode += 1;

            if (RepeatMode == 0)
            {
                button.IsChecked = false;
                Repeat.Visibility = Visibility.Visible;
                RepeatOne.Visibility = Visibility.Collapsed;
            }
            else if (RepeatMode == 1)
            {
                button.IsChecked = true;
                Repeat.Visibility = Visibility.Collapsed;
                RepeatOne.Visibility = Visibility.Visible;

            }
            else if (RepeatMode == 2) 
            {
                button.IsChecked = true;
                Repeat.Visibility = Visibility.Visible;
                RepeatOne.Visibility = Visibility.Collapsed;
            }
        }

        public int RepeatMode
        {
            get => (int)GetValue(RepeatModeProperty);
            set
            {
                SetValue(RepeatModeProperty, value);
            }
        }
    }
}
