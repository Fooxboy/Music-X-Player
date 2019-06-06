using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.Resources.Templates
{
    public partial class TrackResourceTemplate
    {
        public TrackResourceTemplate()
        {
            InitializeComponent();
        }


        private void MusicItem_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}
