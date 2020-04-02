using System.Linq;
using Fooxboy.MusicX.Uwp.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Input;
using Fooxboy.MusicX.Uwp.Services;
using DryIoc;
using Fooxboy.MusicX.Uwp.Views;
using VkNet.Model.Attachments;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class PlaylistControl : UserControl
    {
        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Album",
            typeof(AudioPlaylist), typeof(PlaylistControl), new PropertyMetadata(null));

        public PlaylistControl()
        {
            this.InitializeComponent();
            PlayCommand = new RelayCommand(() => { });

            DeleteCommand = new RelayCommand(() => { });
        }


        public AudioPlaylist Album
        {
            get => (AudioPlaylist) GetValue(PlaylistProperty);
            set { SetValue(PlaylistProperty, value); }
        }

        public RelayCommand PlayCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        private async void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            await PlaylistControlGrid.Scale(centerX: 50.0f,
                centerY: 50.0f,
                scaleX: 1.1f,
                scaleY: 1.1f,
                duration: 200, delay: 0, easingType: EasingType.Back).StartAsync();
        }

        private async void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            await PlaylistControlGrid.Scale(centerX: 50.0f,
                centerY: 50.0f,
                scaleX: 1.0f,
                scaleY: 1.0f,
                duration: 200, delay: 0, easingType: EasingType.Back).StartAsync();
        }

        private void ImageEx_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderShadow.Height = e.NewSize.Height;
            BorderShadow.Width = e.NewSize.Width;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Album.MainArtists != null && Album.MainArtists.Any())
            {
                ArtistsText.Text = string.Join(", ", Album.MainArtists.Select(artist => artist.Name));
            }
            else
            {
                ArtistsText.Text = "Без исполнителя";
            }
        }

        private void PlaylistControlGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}