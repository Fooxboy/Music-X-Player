using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace Fooxboy.MusicX.Uwp.Resources.Templates
{
    public partial class PlaylistResourceTemplate
    {
        public PlaylistResourceTemplate()
        {
            InitializeComponent();
        }

        private async void CoverPlaylist_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            await image.Scale(centerX: 50,
                              centerY: 50,
                              scaleX: 1.2f,
                              scaleY: 1.2f,
                              duration: 10,
                              delay: 0).StartAsync();

        }

        private async void CoverPlaylist_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            await image.Scale(centerX: 50,
                              centerY: 50,
                              scaleX: 1.0f,
                              scaleY: 1.0f,
                              duration: 10,
                              delay: 0).StartAsync();
        }
    }
}
